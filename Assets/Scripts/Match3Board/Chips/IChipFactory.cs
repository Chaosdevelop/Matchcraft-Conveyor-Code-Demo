namespace Match3Game
{
	/// <summary>
	/// Interface for chips factory.
	/// </summary>
	public interface IChipFactory
	{
		/// <summary>
		/// Creates a new chip of the specified type.
		/// </summary>
		/// <param name="type">The type of chip to create.</param>
		/// <returns>The created chip.</returns>
		IChip CreateChip(ChipType type);
	}
}