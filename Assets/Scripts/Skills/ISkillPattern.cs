using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
	/// <summary>
	/// Interface for defining skill patterns.
	/// </summary>
	public interface ISkillPattern
	{
		/// <summary>
		/// Gets the cells affected by the skill pattern based on the origin.
		/// </summary>
		/// <param name="origin">The origin cell position.</param>
		/// <param name="rows">The number of rows in the grid.</param>
		/// <param name="columns">The number of columns in the grid.</param>
		/// <returns>A list of affected cell positions.</returns>
		List<Vector2Int> GetAffectedCells(Vector2Int origin, int rows, int columns);
	}

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
