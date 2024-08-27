using System.Collections.Generic;
using Match3Game;
using UnityEngine;

namespace Skills
{
	/// <summary>
	/// Interface for applying skill effects on the grid.
	/// </summary>
	public interface ISkillEffect
	{
		/// <summary>
		/// Applies the skill effect to the affected cells on the grid.
		/// </summary>
		/// <param name="affectedCells">List of affected cell positions.</param>
		/// <param name="gridController">The grid controller managing the game grid.</param>
		void Apply(List<Vector2Int> affectedCells, GridController gridController);
	}
}
