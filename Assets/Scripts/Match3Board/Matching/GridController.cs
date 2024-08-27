using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using Skills;
using UnityEngine;
using Zenject;

namespace Match3Game
{
	/// <summary>
	/// Controller for the game grid, managing cells and chips on the board.
	/// </summary>
	public class GridController : MonoBehaviour
	{
		ICellFactory cellFactory;
		IChipFactory chipFactory;
		IMatchChecker matchChecker;


		/// <summary>
		/// Gets the number of rows in the grid.
		/// </summary>
		[field: SerializeField]
		public int Rows { get; private set; }

		/// <summary>
		/// Gets the number of columns in the grid.
		/// </summary>
		[field: SerializeField]
		public int Columns { get; private set; }

		readonly HashSet<ICell> cells = new();

		readonly ChipType[] basePieceTypes =
		{
			ChipType.Blue, ChipType.Red, ChipType.Green,
			ChipType.Purple, ChipType.Yellow, ChipType.Orange
		};

		readonly HashSet<MatchInfo> currentMatches = new();
		ISkillPattern skillTargetingPattern;
		IChip selectedChip;

		/// <summary>
		/// Indicates whether the grid is currently interactable.
		/// </summary>
		public bool CanInteract { get; private set; } = true;

		/// <summary>
		/// Gets the two-dimensional array of cells representing the grid.
		/// </summary>
		public ICell[,] Cells { get; private set; }

		/// <summary>
		/// Event raised when a cell is clicked.
		/// </summary>
		public event Action<Vector2Int> OnCellClicked;

		/// <summary>
		/// Event raised when a chip move is completed.
		/// </summary>
		public event Action OnMoveDone;

		/// <summary>
		/// Event raised when a match is processed.
		/// </summary>
		public event Action<MatchInfo> OnMatchProcess;

		/// <summary>
		/// Event raised when a cascade animation is completed.
		/// </summary>
		public event Action CascadeAnimated;

		/// <summary>
		/// Injects dependencies into the GridController.
		/// </summary>
		/// <param name="cellFactory">The factory for creating cells.</param>
		/// <param name="chipFactory">The factory for creating chips.</param>
		/// <param name="matchChecker">The match checker.</param>
		[Inject]
		public void Initialize(ICellFactory cellFactory, IChipFactory chipFactory, IMatchChecker matchChecker)
		{
			this.cellFactory = cellFactory;
			this.chipFactory = chipFactory;
			this.matchChecker = matchChecker;
		}

		/// <summary>
		/// Clears the grid, destroying all cells.
		/// </summary>
		public void ClearGrid()
		{
			foreach (var cell in cells)
			{
				cell.Destroy();
			}

			cells.Clear();
			Cells = new ICell[Rows, Columns];
		}

		/// <summary>
		/// Initializes the grid with the specified number of rows and columns.
		/// </summary>
		/// <param name="rows">The number of rows in the grid.</param>
		/// <param name="columns">The number of columns in the grid.</param>
		public void InitializeGrid(int rows, int columns)
		{
			Cells = new ICell[rows, columns];

			for (int x = 0; x < rows; x++)
			{
				for (int y = 0; y < columns; y++)
				{
					ICell cell = CreateCell(x, y);
					Cells[x, y] = cell;
					cells.Add(cell);

					CreateChip(cell);

					cell.MouseUp.Subscribe(OnMouseUp);
					cell.MouseEnter.Subscribe(OnMouseEnterCell);
				}
			}
		}

		/// <summary>
		/// Reinitializes the grid with the current number of rows and columns.
		/// </summary>
		public void ReinitializeGrid()
		{
			ClearGrid();
			InitializeGrid(Rows, Columns);
		}

		/// <summary>
		/// Toggles the interactability of chips for skill targeting.
		/// </summary>
		/// <param name="targetingActive">True to enable targeting mode, false to disable.</param>
		public void ToggleTargetingSkill(bool targetingActive)
		{
			foreach (var cell in Cells)
			{
				cell.Chip?.ChangeInteractability(!targetingActive);
				cell.SetHighlighted(false);
			}
		}

		/// <summary>
		/// Sets the targeting pattern for skills.
		/// </summary>
		/// <param name="pattern">The skill targeting pattern.</param>
		public void SetTargetingPattern(ISkillPattern pattern)
		{
			skillTargetingPattern = pattern;
		}

		/// <summary>
		/// Destroys the chip at the specified grid position.
		/// </summary>
		/// <param name="position">The grid position of the chip to destroy.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the position is outside the grid bounds.</exception>
		public void DestroyChip(Vector2Int position)
		{
			if (!IsValidPosition(position))
			{
				throw new ArgumentOutOfRangeException(nameof(position), "Position is out of grid bounds.");
			}

			var cell = Cells[position.x, position.y];
			DestroyChip(cell.Chip).Forget();
		}

		/// Checks if the given grid position is valid.
		/// </summary>
		/// <param name="position">The grid position to check.</param>
		/// <returns>True if the position is within the grid bounds, false otherwise.</returns>
		bool IsValidPosition(Vector2Int position)
		{
			return position.x >= 0 && position.x < Rows && position.y >= 0 && position.y < Columns;
		}

		/// <summary>
		/// Creates a new cell at the specified coordinates.
		/// </summary>
		/// <param name="x">The X coordinate of the cell.</param>
		/// <param name="y">The Y coordinate of the cell.</param>
		/// <returns>The created cell.</returns>
		ICell CreateCell(int x, int y)
		{
			ICell cell = cellFactory.CreateCell(x, y);
			Cells[x, y] = cell;
			return cell;
		}

		/// <summary>
		/// Creates a new chip of the specified type.
		/// </summary>
		/// <param name="type">The type of chip to create.</param>
		/// <returns>The created chip.</returns>
		IChip CreateChip(ChipType type)
		{
			return chipFactory.CreateChip(type);
		}

		/// <summary>
		/// Creates a new chip and assigns it to the specified cell.
		/// </summary>
		/// <param name="cell">The cell to assign the chip to.</param>
		void CreateChip(ICell cell)
		{
			ChipType validType = GetValidPieceType(cell.X, cell.Y);
			IChip chip = CreateChip(validType);
			cell.SetChip(chip);
			chip.SetPositions(Vector3.zero, Vector3.zero);
			chip.Click.Subscribe(OnChipClick);
			chip.Swipe.CombineLatest(chip.Click, (direction, c) => (c, direction))
				.Subscribe(tuple =>
				{
					var (c, direction) = tuple;
					OnChipSwipe(c, direction);
				});
		}

		/// <summary>
		/// Checks for a match at the specified cell.
		/// </summary>
		/// <param name="cell">The cell to check for a match.</param>
		/// <returns>MatchInfo if a match is found, otherwise null.</returns>
		MatchInfo CheckMatch(ICell cell)
		{
			return matchChecker.CheckMatch(Cells, cell);
		}

		/// <summary>
		/// Handles the MouseEnter event for a cell, highlighting cells based on the skill targeting pattern.
		/// </summary>
		/// <param name="pos">The grid position of the cell that the mouse entered.</param>
		void OnMouseEnterCell(Vector2Int pos)
		{
			if (skillTargetingPattern == null) return;


			var cellsPos = skillTargetingPattern.GetAffectedCells(pos, Rows, Columns);

			foreach (var cell in Cells)
			{
				cell.SetHighlighted(cellsPos.Contains(new Vector2Int(cell.X, cell.Y)));
			}
		}

		/// <summary>
		/// Handles the MouseUp event for a cell, raising the OnCellClicked event.
		/// </summary>
		/// <param name="pos">The grid position of the cell that the mouse was released over.</param>
		void OnMouseUp(Vector2Int pos)
		{
			OnCellClicked?.Invoke(pos);
		}

		/// <summary>
		/// Handles the Click event for a chip, managing chip selection and swapping.
		/// </summary>
		/// <param name="chip">The chip that was clicked.</param>
		void OnChipClick(IChip chip)
		{

			if (!CanInteract) return;

			if (selectedChip == null)
			{
				selectedChip = chip;
				chip.Highlight(true);
			}
			else
			{
				if (selectedChip == chip)
				{
					selectedChip.Highlight(false);
					selectedChip = null;
				}
				else if (CanSwap(selectedChip.Cell, chip.Cell))
				{
					DoSwap(selectedChip, chip);
				}
				selectedChip.Highlight(false);
				selectedChip = null;
			}
		}

		/// <summary>
		/// Handles the Swipe event for a chip, attempting to swap the chip with its neighbor in the swipe direction.
		/// </summary>
		/// <param name="chip">The chip that was swiped.</param>
		/// <param name="direction">The direction of the swipe.</param>
		void OnChipSwipe(IChip chip, Vector2 direction)
		{
			if (!CanInteract) return;

			Vector2Int targetPos = new Vector2Int(chip.Cell.X + (int)direction.x, chip.Cell.Y + (int)direction.y);

			if (IsValidPosition(targetPos))
			{
				var targetCell = Cells[targetPos.x, targetPos.y];
				if (CanSwap(chip.Cell, targetCell))
				{
					DoSwap(chip, targetCell.Chip);
				}
			}

			selectedChip?.Highlight(false);
			selectedChip = null;
		}

		/// <summary>
		/// Checks if two cells can swap their chips.
		/// </summary>
		/// <param name="cell1">The first cell.</param>
		/// <param name="cell2">The second cell.</param>
		/// <returns>True if the cells can swap chips, false otherwise.</returns>
		bool CanSwap(ICell cell1, ICell cell2)
		{
			if (Mathf.Abs(cell1.X - cell2.X) + Mathf.Abs(cell1.Y - cell2.Y) != 1) return false;
			if (!cell1.Chip.CanSwapWith(cell2.Chip) || !cell2.Chip.CanSwapWith(cell1.Chip)) return false;

			SwapChips(cell1, cell2);

			bool hasMatch = CheckMatch(cell1) != null || CheckMatch(cell2) != null;

			SwapChips(cell1, cell2);

			return hasMatch;
		}

		/// <summary>
		/// Swaps the chips between two cells.
		/// </summary>
		/// <param name="cell1">The first cell.</param>
		/// <param name="cell2">The second cell.</param>
		void SwapChips(ICell cell1, ICell cell2)
		{
			var tmpchip = cell1.Chip;
			cell1.SetChip(cell2.Chip);
			cell2.SetChip(tmpchip);
		}

		/// <summary>
		/// Asynchronously moves a chip by the specified offset.
		/// </summary>
		/// <param name="chip">The chip to move.</param>
		/// <param name="xShift">The horizontal offset.</param>
		/// <param name="yShift">The vertical offset.</param>
		/// <returns>A UniTask that represents the asynchronous move operation.</returns>
		async UniTask MoveChipAsync(IChip chip, int xShift, int yShift)
		{
			chip.SetPositions(new Vector3(xShift, yShift), Vector3.zero);
			await chip.Move();
		}

		/// <summary>
		/// Moves a chip by the specified offset and executes an optional callback when the move is completed.
		/// </summary>
		/// <param name="chip">The chip to move.</param>
		/// <param name="xShift">The horizontal offset.</param>
		/// <param name="yShift">The vertical offset.</param>
		/// <param name="onComplete">An optional action to execute when the move is complete.</param>
		void MoveChip(IChip chip, int xShift, int yShift, Action onComplete)
		{
			MoveChipAsync(chip, xShift, yShift)
				.ContinueWith(() => onComplete?.Invoke())
				.Forget();
		}

		/// <summary>
		/// Asynchronously swaps two chips on the grid.
		/// </summary>
		/// <param name="chip1">The first chip to swap.</param>
		/// <param name="chip2">The second chip to swap.</param>
		/// <returns>A UniTask that represents the asynchronous swap operation.</returns>
		async UniTask DoSwapAsync(IChip chip1, IChip chip2)
		{
			CanInteract = false;

			var cell1 = chip1.Cell;
			var cell2 = chip2.Cell;

			SwapChips(cell1, cell2);

			await UniTask.WhenAll(
				MoveChipAsync(chip1, cell1.X - cell2.X, cell1.Y - cell2.Y),
				MoveChipAsync(chip2, cell2.X - cell1.X, cell2.Y - cell1.Y)
			);

			AfterSwap(chip1, chip2);
		}

		/// <summary>
		/// Swaps two chips on the grid, initiating the swap operation asynchronously.
		/// </summary>
		/// <param name="chip1">The first chip to swap.</param>
		/// <param name="chip2">The second chip to swap.</param>
		void DoSwap(IChip chip1, IChip chip2)
		{
			DoSwapAsync(chip1, chip2).Forget();
		}

		/// <summary>
		/// Performs actions after a chip swap is completed, such as checking for matches and raising the OnMoveDone event.
		/// </summary>
		/// <param name="chip1">The first chip that was swapped.</param>
		/// <param name="chip2">The second chip that was swapped.</param>
		void AfterSwap(IChip chip1, IChip chip2)
		{
			if (CheckForMatches())
			{
				ProcessMatches();
			}
			OnMoveDone?.Invoke();
		}

		/// <summary>
		/// Checks for matches on the grid.
		/// </summary>
		/// <returns>True if any matches were found, false otherwise.</returns>
		bool CheckForMatches()
		{
			currentMatches.Clear();

			foreach (var cell in cells)
			{
				AddMatch(CheckMatch(cell));
			}

			return currentMatches.Count > 0;
		}

		/// <summary>
		/// Adds a match to the current set of matches, merging with existing matches if necessary.
		/// </summary>
		/// <param name="newMatch">The match to add.</param>
		public void AddMatch(MatchInfo newMatch)
		{
			if (newMatch == null) return;

			var intersectingMatches = currentMatches.Where(match => match.FindIntersection(newMatch).Count > 0).ToList();

			foreach (var intersectingMatch in intersectingMatches)
			{
				newMatch.MergeWith(intersectingMatch);
				currentMatches.Remove(intersectingMatch);
			}

			currentMatches.Add(newMatch);
		}

		/// <summary>
		/// Gets a valid chip type for the specified grid position, preventing immediate matches.
		/// </summary>
		/// <param name="x">The X coordinate of the position.</param>
		/// <param name="y">The Y coordinate of the position.</param>
		/// <returns>A valid chip type.</returns>
		ChipType GetValidPieceType(int x, int y)
		{
			List<ChipType> possibleTypes = new List<ChipType>(basePieceTypes);

			if (x > 1 && Cells[x - 1, y]?.Chip.Type == Cells[x - 2, y]?.Chip.Type)
			{
				possibleTypes.Remove(Cells[x - 1, y].Chip.Type);
			}
			if (y > 1 && Cells[x, y - 1]?.Chip.Type == Cells[x, y - 2]?.Chip.Type)
			{
				possibleTypes.Remove(Cells[x, y - 1].Chip.Type);
			}

			return possibleTypes[UnityEngine.Random.Range(0, possibleTypes.Count)];
		}

		/// <summary>
		/// Processes matches on the grid, initiating the asynchronous processing operation.
		/// </summary>
		void ProcessMatches()
		{
			ProcessMatchesAsync().Forget();
		}

		/// <summary>
		/// Asynchronously processes matches on the grid, destroying matched chips and initiating a cascade.
		/// </summary>
		/// <returns>A UniTask that represents the asynchronous match processing operation.</returns>
		async UniTask ProcessMatchesAsync()
		{
			CanInteract = false;

			var destroyTasks = new List<UniTask>();
			foreach (var match in currentMatches)
			{
				foreach (var chip in match.MatchedChips)
				{
					if (chip != null)
					{
						destroyTasks.Add(DestroyChip(chip));
					}
				}
				OnMatchProcess?.Invoke(match);
			}

			await UniTask.WhenAll(destroyTasks);

			currentMatches.Clear();
			await CascadeAndFill();
		}

		/// <summary>
		/// Asynchronously performs a cascade fill operation, moving chips down and creating new ones to fill empty cells.
		/// </summary>
		/// <returns>A UniTask that represents the asynchronous cascade fill operation.</returns>
		async UniTask CascadeAndFill()
		{
			await Task.Delay(50);

			var moveTasks = new List<UniTask>();

			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Columns; y++)
				{
					if (Cells[x, y].Chip != null) continue;

					for (int currentY = y + 1; currentY < Columns; currentY++)
					{
						var movedChip = Cells[x, currentY].Chip;
						if (movedChip == null) continue;

						Cells[x, y].SetChip(movedChip);
						Cells[x, currentY].SetChip(null);
						moveTasks.Add(MoveChipAsync(movedChip, 0, currentY - y));
						break;
					}
				}
			}

			var newChipMoveTasks = new List<UniTask>();
			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Columns; y++)
				{
					if (Cells[x, y].Chip != null) continue;

					CreateChip(Cells[x, y]);
					newChipMoveTasks.Add(MoveChipAsync(Cells[x, y].Chip, 0, Columns - y));
				}
			}
			await UniTask.WhenAll(moveTasks.Concat(newChipMoveTasks));

			if (CheckForMatches())
			{
				ProcessMatches();
			}
			else
			{
				CascadeAnimated?.Invoke();
				CanInteract = true;
			}
		}


		/// <summary>
		/// Asynchronously destroys the specified chip.
		/// </summary>
		/// <param name="chip">The chip to destroy.</param>
		/// <returns>A UniTask that represents the asynchronous destroy operation.</returns>
		async UniTask DestroyChip(IChip chip)
		{
			var cell = Cells[chip.Cell.X, chip.Cell.Y];
			cell.SetChip(null);

			await chip.DestroyChip();


			if (currentMatches.Any(match => match.MatchedChips.Any(c => c != null && c != chip)))
			{

				return;
			}


			await CascadeAndFill();
		}
	}
}