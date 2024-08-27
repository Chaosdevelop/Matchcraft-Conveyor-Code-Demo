using UnityEngine;
using Zenject;

namespace Match3Game
{
	/// <summary>
	/// Factory for creating cells.
	/// </summary>
	public class CellFactory : ICellFactory
	{
		readonly DiContainer container;
		readonly Cell cellPrefab;
		readonly Transform cellsContainer;

		/// <summary>
		/// Constructor for CellFactory.
		/// </summary>
		/// <param name="container">The DI container.</param>
		/// <param name="cellPrefab">The cell prefab.</param>
		/// <param name="cellsContainer">The container for cells.</param>
		public CellFactory(DiContainer container, Cell cellPrefab, Transform cellsContainer)
		{
			this.container = container;
			this.cellPrefab = cellPrefab;
			this.cellsContainer = cellsContainer;
		}

		/// <summary>
		/// Creates a new cell at the specified coordinates.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <returns>The created cell.</returns>
		public ICell CreateCell(int x, int y)
		{
			Cell cellController = container.InstantiatePrefabForComponent<Cell>(cellPrefab, cellsContainer);
			cellController.name = $"Cell {x} {y}";
			cellController.transform.localPosition = new Vector3(x, y);
			cellController.Initialize(x, y);
			return cellController;
		}
	}
}