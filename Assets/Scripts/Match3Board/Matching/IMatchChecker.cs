namespace Match3Game
{

	/// <summary>
	/// Interface for chips matching.
	/// </summary>
	public interface IMatchChecker
	{
		/// <summary>
		/// Checks for a match starting from the specified cell.
		/// </summary>
		/// <param name="cells">The grid of cells.</param>
		/// <param name="cell">The cell to check from.</param>
		/// <returns>MatchInfo if a match is found, otherwise null.</returns>
		MatchInfo CheckMatch(ICell[,] cells, ICell cell);
	}
}