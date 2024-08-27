using BaseCore.Collections;
using UnityEngine;
using Zenject;

namespace Match3Game
{
	/// <summary>
	/// Factory for creating chips.
	/// </summary>
	public class ChipFactory : IChipFactory
	{
		readonly DiContainer container;
		readonly EnumDictionary<ChipType, Chip> chipsDictionary;
		readonly Transform chipsContainer;

		/// <summary>
		/// Constructor for ChipFactory.
		/// </summary>
		/// <param name="container">The DI container.</param>
		/// <param name="chipsDictionary">Dictionary of chip prefabs by type.</param>
		/// <param name="chipsContainer">The container for chips.</param>
		public ChipFactory(DiContainer container, EnumDictionary<ChipType, Chip> chipsDictionary, Transform chipsContainer)
		{
			this.container = container;
			this.chipsDictionary = chipsDictionary;
			this.chipsContainer = chipsContainer;
		}

		/// <summary>
		/// Creates a new chip of the specified type.
		/// </summary>
		/// <param name="type">The type of chip to create.</param>
		/// <returns>The created chip.</returns>
		public IChip CreateChip(ChipType type)
		{
			Chip chipPrefab = chipsDictionary[type];
			Chip chip = container.InstantiatePrefabForComponent<Chip>(chipPrefab, chipsContainer);
			chip.Initialize(type);
			return chip;
		}
	}
}