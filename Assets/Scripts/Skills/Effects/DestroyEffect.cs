using Match3Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Skills
{
	/// <summary>
	/// A skill effect that destroys chips of specified types on the grid.
	/// </summary>
	[System.Serializable]
	public class DestroyEffect : ISkillEffect
	{
		[SerializeField]
		ChipType[] affectedTypes;

		/// <summary>
		/// Applies the destroy effect to the affected cells on the grid.
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
					gridController.DestroyChip(cellPos);
				}
			}
		}
	}
}
