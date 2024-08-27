using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
	/// <summary>
	/// A skill pattern that affects the entire field.
	/// </summary>
	[System.Serializable]
	public class WholeFieldPattern : ISkillPattern
	{
		/// <summary>
		/// Gets the cells affected by the whole field pattern.
		/// </summary>
		/// <param name="origin">The origin cell position.</param>
		/// <param name="rows">The number of rows in the grid.</param>
		/// <param name="columns">The number of columns in the grid.</param>
		/// <returns>A list of all cell positions in the grid.</returns>
		public List<Vector2Int> GetAffectedCells(Vector2Int origin, int rows, int columns)
		{
			var cells = new List<Vector2Int>();
			for (int x = 0; x < rows; x++)
			{
				for (int y = 0; y < columns; y++)
				{
					cells.Add(new Vector2Int(x, y));
				}
			}
			return cells;
		}
	}
}
