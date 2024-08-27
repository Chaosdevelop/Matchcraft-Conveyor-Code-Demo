using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
	/// <summary>
	/// A skill pattern that affects a cross-shaped area.
	/// </summary>
	[System.Serializable]
	public class CrossPattern : ISkillPattern
	{
		/// <summary>
		/// Gets the cells affected by the cross pattern.
		/// </summary>
		/// <param name="origin">The origin cell position.</param>
		/// <param name="rows">The number of rows in the grid.</param>
		/// <param name="columns">The number of columns in the grid.</param>
		/// <returns>A list of affected cell positions within a cross-shaped area.</returns>
		public List<Vector2Int> GetAffectedCells(Vector2Int origin, int rows, int columns)
		{
			var cells = new List<Vector2Int>
			{
				origin,
				new Vector2Int(origin.x - 1, origin.y),
				new Vector2Int(origin.x + 1, origin.y),
				new Vector2Int(origin.x, origin.y - 1),
				new Vector2Int(origin.x, origin.y + 1)
			};

			cells.RemoveAll(cell => cell.x < 0 || cell.x >= rows || cell.y < 0 || cell.y >= columns);
			return cells;
		}
	}
}
