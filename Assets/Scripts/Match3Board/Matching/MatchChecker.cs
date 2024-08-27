namespace Match3Game
{
	/// <summary>
	/// Checks for matches on the board.
	/// </summary>
	public class MatchChecker : IMatchChecker
	{

		readonly int rows;
		readonly int columns;

		/// <summary>
		/// Constructor for MatchChecker.
		/// </summary>
		/// <param name="cells">The 2D array of cells representing the board.</param>
		public MatchChecker(GridController gridController)
		{

			this.rows = gridController.Rows;
			this.columns = gridController.Columns;
		}

		/// <summary>
		/// Checks for a match starting from the specified cell.
		/// </summary>
		/// <param name="cell">The cell to check from.</param>
		/// <returns>MatchInfo if a match is found, otherwise null.</returns>
		public MatchInfo CheckMatch(ICell[,] cells, ICell cell)
		{
			if (cell.Chip == null)
			{
				return null;
			}

			int x = cell.X;
			int y = cell.Y;
			ChipType type = cell.Chip.Type;

			MatchInfo horizontalMatch = new MatchInfo(type);
			MatchInfo verticalMatch = new MatchInfo(type);

			horizontalMatch.AddChip(cell.Chip);
			verticalMatch.AddChip(cell.Chip);

			// Check for horizontal match
			for (int i = x + 1; i < rows; i++)
			{
				if (cells[i, y].Chip?.Type == type)
				{
					horizontalMatch.AddChip(cells[i, y].Chip);
				}
				else
				{
					break;
				}
			}
			for (int i = x - 1; i >= 0; i--)
			{
				if (cells[i, y].Chip?.Type == type)
				{
					horizontalMatch.AddChip(cells[i, y].Chip);
				}
				else
				{
					break;
				}
			}

			// Check for vertical match
			for (int i = y + 1; i < columns; i++)
			{
				if (cells[x, i].Chip?.Type == type)
				{
					verticalMatch.AddChip(cells[x, i].Chip);
				}
				else
				{
					break;
				}
			}
			for (int i = y - 1; i >= 0; i--)
			{
				if (cells[x, i].Chip?.Type == type)
				{
					verticalMatch.AddChip(cells[x, i].Chip);
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
	}
}
