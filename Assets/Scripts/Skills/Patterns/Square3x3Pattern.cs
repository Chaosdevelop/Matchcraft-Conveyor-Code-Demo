using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
	/// <summary>
	/// A skill pattern that affects a 3x3 square area.
	/// </summary>
	[System.Serializable]
	public class Square3x3Pattern : ISkillPattern
	{
		/// <summary>
		/// Gets the cells affected by the 3x3 square pattern.
		/// </summary>
		/// <param name="origin">The origin cell position.</param>
		/// <param name="rows">The number of rows in the grid.</param>
		/// <param name="columns">The number of columns in the grid.</param>
		/// <returns>A list of affected cell positions within a 3x3 square area.</returns>
		public List<Vector2Int> GetAffectedCells(Vector2Int origin, int rows, int columns)
		{
			var cells = new List<Vector2Int>();
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					var cell = new Vector2Int(origin.x + x, origin.y + y);
					if (cell.x >= 0 && cell.x < rows && cell.y >= 0 && cell.y < columns)
					{
						cells.Add(cell);
					}
				}
			}
			return cells;
		}
	}
}
