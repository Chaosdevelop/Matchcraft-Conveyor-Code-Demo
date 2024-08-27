using System.Collections.Generic;
using System.Linq;
using Match3Game;
using UnityEngine;

namespace Skills
{
	/// <summary>
	/// A skill effect that changes the color of specified types of chips on the grid.
	/// </summary>
	[System.Serializable]
	public class ColorChangeEffect : ISkillEffect
	{
		[SerializeField]
		ChipType[] affectedTypes;

		[SerializeField]
		ChipType chipType;

		/// <summary>
		/// Applies the color change effect to the affected cells on the grid.
		/// </summary>
		/// <param name="affectedCells">List of affected cell positions.</param>
		/// <param name="gridController">The grid controller managing the game grid.</param>
		public void Apply(List<Vector2Int> affectedCells, GridController gridController)
		{
			foreach (var cellPos in affectedCells)
			{
				var cell = gridController.Cells[cellPos.x, cellPos.y];
				if (affectedTypes.Any(arg => arg == cell.Chip.Type))
				{
					// Apply color change logic here
				}
			}
		}
	}
}
