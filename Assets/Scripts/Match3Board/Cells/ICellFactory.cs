namespace Match3Game
{
	/// <summary>
	/// Interface for cells factory.
	/// </summary>
	public interface ICellFactory
	{
		/// <summary>
		/// Creates a new cell at the specified coordinates.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <returns>The created cell.</returns>
		ICell CreateCell(int x, int y);
	}
}