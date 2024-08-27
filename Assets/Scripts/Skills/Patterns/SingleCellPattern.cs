using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
	/// <summary>
	/// A skill pattern that affects a single cell.
	/// </summary>
	[System.Serializable]
	public class SingleCellPattern : ISkillPattern
	{
		/// <summary>
		/// Gets the cells affected by the single cell pattern.
		/// </summary>
		/// <param name="origin">The origin cell position.</param>
		/// <param name="rows">The number of rows in the grid.</param>
		/// <param name="columns">The number of columns in the grid.</param>
		/// <returns>A list containing only the origin cell position.</returns>
		public List<Vector2Int> GetAffectedCells(Vector2Int origin, int rows, int columns)
		{
			return new List<Vector2Int> { origin };
		}
	}
}
