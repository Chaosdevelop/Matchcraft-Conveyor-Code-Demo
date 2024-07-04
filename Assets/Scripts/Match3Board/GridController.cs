using BaseCore.Collections;
using Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3Game
{
	/// <summary>
	/// Controller for the game grid, managing cells and chips on the board.
	/// </summary>
	public class GridController : MonoBehaviour
	{
		[SerializeField]
		Cell cellPrefab;

		[SerializeField]
		Transform cellsContainer;

		[SerializeField]
		EnumDictionary<ChipType, Chip> chipsDictionary;

		[field: SerializeField]
		public int Rows { get; private set; }

		[field: SerializeField]
		public int Columns { get; private set; }

		List<ICell> cells = new List<ICell>();
		IChip selectedChip;
		ChipType[] basePieceTypes = new ChipType[]
		{
			ChipType.Blue,
			ChipType.Red,
			ChipType.Green,
			ChipType.Purple,
			ChipType.Yellow,
			ChipType.Orange,
		};

		List<MatchInfo> currentMatches = new List<MatchInfo>();
		int chipMoveCompleteCounter = 0;
		int chipDestroyCompleteCounter = 0;
		ISkillPattern skillTargetingPattern;

		public bool CanInteract { get; private set; } = true;
		public ICell[,] Cells { get; private set; }

		public System.Action<Vector2Int> OnCellClicked;
		public System.Action OnMoveDone;
		public System.Action<MatchInfo> OnMatchProcess;
		public System.Action CascadeAnimated;

		/// <summary>
		/// Clears the grid.
		/// </summary>
		public void ClearGrid()
		{
			foreach (var cellController in cells)
			{
				DestroyImmediate((MonoBehaviour)cellController);
			}

			cells.Clear();
			Cells = new ICell[Rows, Columns];
		}

		/// <summary>
		/// Initializes the grid.
		/// </summary>
		public void InitializeGrid()
		{
			Cells = new ICell[Rows, Columns];

			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Columns; y++)
				{
					ICell cell = CreateCell(x, y);
					cells.Add(cell);
					CreateChip(cell);
					cell.OnMouseEnterCallback += OnMouseEnter;
					cell.OnMouseUpCallback += OnMouseUp;
				}
			}
		}

		/// <summary>
		/// Reinitializes the grid.
		/// </summary>
		public void ReinitializeGrid()
		{
			ClearGrid();
			InitializeGrid();
		}

		/// <summary>
		/// Creates a cell at the specified coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate of the cell.</param>
		/// <param name="y">The y-coordinate of the cell.</param>
		/// <returns>The created cell.</returns>
		ICell CreateCell(int x, int y)
		{
			Cell cellController = Instantiate(cellPrefab, cellsContainer, false);
			cellController.name = $"Cell {x} {y}";
			cellController.transform.localPosition = new Vector3(x, y);
			cellController.Initialize(x, y);
			Cells[x, y] = cellController;
			return cellController;
		}

		/// <summary>
		/// Creates a chip in the specified cell.
		/// </summary>
		/// <param name="cell">The cell to place the chip in.</param>
		/// <returns>The created chip.</returns>
		Chip CreateChip(ICell cell)
		{
			ChipType pieceType = GetValidPieceType(cell.X, cell.Y);
			var chipPrefab = chipsDictionary[pieceType];
			Chip chip = Instantiate(chipPrefab, cellsContainer, false);

			chip.Initialize(pieceType);
			chip.SetCallbacks(OnChipClick, OnChipSwipe);
			cell.SetChip(chip);
			chip.ImmediateMove();
			return chip;
		}

		void OnMouseEnter(Vector2Int pos)
		{
			if (skillTargetingPattern == null)
			{
				return;
			}

			var cellsPos = skillTargetingPattern.GetAffectedCells(pos, Rows, Columns);

			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Columns; y++)
				{
					bool highlight = cellsPos.Contains(new Vector2Int(x, y));
					Cells[x, y].SetHighlighted(highlight);
				}
			}
		}

		void OnMouseUp(Vector2Int pos)
		{
			OnCellClicked?.Invoke(pos);
		}

		/// <summary>
		/// Toggles the targeting skill.
		/// </summary>
		/// <param name="targetingActive">True to activate targeting, false to deactivate.</param>
		public void ToggleTargetingSkill(bool targetingActive)
		{
			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Columns; y++)
				{
					Cells[x, y].Chip?.ChangeInteractability(!targetingActive);
				}
			}

			if (!targetingActive)
			{
				for (int x = 0; x < Rows; x++)
				{
					for (int y = 0; y < Columns; y++)
					{
						Cells[x, y].SetHighlighted(false);
					}
				}
			}
		}

		/// <summary>
		/// Sets the targeting pattern for skills.
		/// </summary>
		/// <param name="pattern">The skill pattern to set.</param>
		public void SetTargetingPattern(ISkillPattern pattern)
		{
			skillTargetingPattern = pattern;
		}

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
				else
				{
					bool canSwap = CanSwap(selectedChip.Cell, chip.Cell);
					if (canSwap)
					{
						DoSwap(selectedChip, chip);
					}
					selectedChip.Highlight(false);
					selectedChip = null;
				}
			}
		}

		void OnChipSwipe(IChip chip, Vector2 direction)
		{
			if (!CanInteract) return;
			int x = chip.Cell.X + (int)direction.x;
			int y = chip.Cell.Y + (int)direction.y;
			if (x >= 0 && x < Rows && y >= 0 && y < Columns)
			{
				var targetCell = Cells[x, y];
				bool canSwap = CanSwap(chip.Cell, targetCell);
				if (canSwap)
				{
					DoSwap(chip, targetCell.Chip);
				}
				selectedChip?.Highlight(false);
				selectedChip = null;
			}
		}

		/// <summary>
		/// Checks if two cells can swap their chips.
		/// </summary>
		/// <param name="cell1">The first cell.</param>
		/// <param name="cell2">The second cell.</param>
		/// <returns>True if the swap is possible, otherwise false.</returns>
		bool CanSwap(ICell cell1, ICell cell2)
		{
			if (Mathf.Abs(cell1.X - cell2.X) + Mathf.Abs(cell1.Y - cell2.Y) != 1)
			{
				return false;
			}
			if (!cell1.Chip.CanSwapWith(cell2.Chip) || !cell2.Chip.CanSwapWith(cell1.Chip))
			{
				return false;
			}

			// Temporary swap to check for matches
			SwapChips(cell1, cell2);

			var match1 = CheckMatch(cell1);
			var match2 = CheckMatch(cell2);

			// Swap back
			SwapChips(cell1, cell2);

			return match1 != null || match2 != null;
		}

		void SwapChips(ICell cell1, ICell cell2)
		{
			var tempChip = cell1.Chip;
			cell1.SetChip(cell2.Chip);
			cell2.SetChip(tempChip);
		}

		void MoveChip(IChip chip, int xShift, int yShift, System.Action onComplete)
		{
			chipMoveCompleteCounter++;
			chip.SetPositions(new Vector3(xShift, yShift), Vector3.zero);
			chip.Move(() => { chipMoveCompleteCounter--; onComplete?.Invoke(); });
		}

		void DoSwap(IChip chip1, IChip chip2)
		{
			CanInteract = false;

			var cell1 = chip1.Cell;
			var cell2 = chip2.Cell;

			SwapChips(cell1, cell2);

			MoveChip(chip1, cell1.X - cell2.X, cell1.Y - cell2.Y, () => AfterSwap(chip1, chip2));
			MoveChip(chip2, cell2.X - cell1.X, cell2.Y - cell1.Y, () => AfterSwap(chip1, chip2));
		}

		void AfterSwap(IChip chip1, IChip chip2)
		{
			if (chipMoveCompleteCounter == 0)
			{
				CheckForMatches();
				ProcessMatches();
				OnMoveDone?.Invoke();
			}
		}

		/// <summary>
		/// Checks for matches on the grid.
		/// </summary>
		/// <returns>True if matches were found, otherwise false.</returns>
		bool CheckForMatches()
		{
			currentMatches.Clear();

			foreach (var cell in cells)
			{
				MatchInfo match = CheckMatch(cell);
				AddMatch(match);
			}

			return currentMatches.Count > 0;
		}

		/// <summary>
		/// Adds a new MatchInfo object to the list, merging intersecting and removing joined elements.
		/// </summary>
		/// <param name="newMatch">The new MatchInfo object to add.</param>
		public void AddMatch(MatchInfo newMatch)
		{
			if (newMatch == null)
			{
				return;
			}

			List<MatchInfo> intersectingMatches = new List<MatchInfo>();

			// Find intersecting MatchInfo
			foreach (var match in currentMatches)
			{
				if (match.FindIntersection(newMatch).Count > 0)
				{
					intersectingMatches.Add(match);
				}
			}

			// Merge intersecting MatchInfo
			foreach (var intersectingMatch in intersectingMatches)
			{
				newMatch.MergeWith(intersectingMatch);
				currentMatches.Remove(intersectingMatch);
			}

			// Add merged MatchInfo
			currentMatches.Add(newMatch);
		}

		void FinalizeSwapChips(IChip chip1, IChip chip2)
		{
			chip1.Move(() => CanInteract = true);
			chip2.Move(() => CanInteract = true);
		}

		/// <summary>
		/// Gets a valid chip type for the specified coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <returns>The valid chip type.</returns>
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

			return possibleTypes[Random.Range(0, possibleTypes.Count)];
		}

		/// <summary>
		/// Checks for matches for the specified cell.
		/// </summary>
		/// <param name="cell">The cell to check for matches.</param>
		/// <returns>Match information if a match is found, otherwise null.</returns>
		MatchInfo CheckMatch(ICell cell)
		{
			int x = cell.X;
			int y = cell.Y;
			ChipType type = cell.Chip.Type;

			MatchInfo horizontalMatch = new MatchInfo(type);
			MatchInfo verticalMatch = new MatchInfo(type);

			horizontalMatch.AddChip(cell.Chip);
			verticalMatch.AddChip(cell.Chip);

			// Check for horizontal match
			for (int i = x + 1; i < Rows; i++)
			{
				if (Cells[i, y].Chip?.Type == type)
				{
					horizontalMatch.AddChip(Cells[i, y].Chip);
				}
				else
				{
					break;
				}
			}
			for (int i = x - 1; i >= 0; i--)
			{
				if (Cells[i, y].Chip?.Type == type)
				{
					horizontalMatch.AddChip(Cells[i, y].Chip);
				}
				else
				{
					break;
				}
			}

			// Check for vertical match
			for (int i = y + 1; i < Columns; i++)
			{
				if (Cells[x, i].Chip?.Type == type)
				{
					verticalMatch.AddChip(Cells[x, i].Chip);
				}
				else
				{
					break;
				}
			}
			for (int i = y - 1; i >= 0; i--)
			{
				if (Cells[x, i].Chip?.Type == type)
				{
					verticalMatch.AddChip(Cells[x, i].Chip);
				}
				else
				{
					break;
				}
			}

			if (horizontalMatch.MatchCount >= 3)
			{
				return horizontalMatch;
			}
			if (verticalMatch.MatchCount >= 3)
			{
				return verticalMatch;
			}

			return null;
		}

		/// <summary>
		/// Processes matches on the grid.
		/// </summary>
		void ProcessMatches()
		{
			CanInteract = false;

			foreach (var match in currentMatches)
			{
				foreach (var chip in match.MatchedChips)
				{
					DestroyChip(chip);
				}
				OnMatchProcess?.Invoke(match);
			}
			currentMatches.Clear();
		}

		/// <summary>
		/// Performs cascade fill and generates new chips after match destruction.
		/// </summary>
		/// <returns>The coroutine for cascade fill.</returns>
		IEnumerator CascadeAndFill()
		{
			yield return new WaitForSeconds(0.05f);

			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Columns; y++)
				{
					int currentY = y;
					while (Cells[x, y].Chip == null && currentY < Columns - 1)
					{
						var movedChip = Cells[x, currentY + 1].Chip;

						if (movedChip != null)
						{
							Cells[x, y].SetChip(movedChip);
							Cells[x, currentY + 1].SetChip(null);
							MoveChip(movedChip, 0, currentY + 1 - y, null);
						}

						currentY++;
					}
				}
			}

			for (int x = 0; x < Rows; x++)
			{
				int lowest = Columns;
				for (int y = 0; y < Columns; y++)
				{
					if (Cells[x, y].Chip == null)
					{
						if (lowest > y)
						{
							lowest = Columns - y;
						}

						Chip chip = CreateChip(Cells[x, y]);
						MoveChip(chip, 0, lowest, null);
					}
				}
			}

			yield return new WaitWhile(() => chipMoveCompleteCounter > 0);
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
		/// Destroys a chip.
		/// </summary>
		/// <param name="chip">The chip to destroy.</param>
		void DestroyChip(IChip chip)
		{
			chipDestroyCompleteCounter++;
			var cell = Cells[chip.Cell.X, chip.Cell.Y];
			cell.SetChip(null);

			chip.DestroyChip(() =>
			{
				chipDestroyCompleteCounter--;
				if (chipDestroyCompleteCounter == 0)
				{
					StartCoroutine(CascadeAndFill());
				}
			});
		}

		public void DestroyChip(Cell cell)
		{
			DestroyChip(cell.Chip);
		}

		public void DestroyChip(Vector2Int position)
		{
			if (position.x < 0 || position.x >= Cells.GetLength(0))
			{
				Debug.LogError($"Position.x {position.x} is out of bounds.");
				return;
			}

			if (position.y < 0 || position.y >= Cells.GetLength(1))
			{
				Debug.LogError($"Position.y {position.y} is out of bounds.");
				return;
			}

			var cell = Cells[position.x, position.y];
			DestroyChip(cell.Chip);
		}
	}
}
