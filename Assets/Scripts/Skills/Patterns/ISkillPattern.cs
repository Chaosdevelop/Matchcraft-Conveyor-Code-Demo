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
}
