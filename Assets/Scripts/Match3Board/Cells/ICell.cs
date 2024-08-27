using R3;
using UnityEngine;

namespace Match3Game
{
	/// <summary>
	/// Interface for cells.
	/// </summary>
	public interface ICell
	{
		/// <summary>
		/// Gets the X coordinate of the cell.
		/// </summary>
		int X { get; }

		/// <summary>
		/// Gets the Y coordinate of the cell.
		/// </summary>
		int Y { get; }

		/// <summary>
		/// Gets or sets the chip contained in the cell.
		/// </summary>
		IChip Chip { get; set; }

		/// <summary>
		/// Initializes the cell with the given coordinates.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		void Initialize(int x, int y);

		/// <summary>
		/// Sets the chip in the cell.
		/// </summary>
		/// <param name="chip">The chip to set.</param>
		void SetChip(IChip chip);

		/// <summary>
		/// Sets the highlighted state of the cell.
		/// </summary>
		/// <param name="on">True to highlight the cell, false otherwise.</param>
		void SetHighlighted(bool on);

		/// <summary>
		/// An observable stream that emits the cell's coordinates when the mouse enters the cell.
		/// </summary>
		Observable<Vector2Int> MouseEnter { get; }

		/// <summary>
		/// An observable stream that emits the cell's coordinates when the mouse button is released over the cell.
		/// </summary>
		Observable<Vector2Int> MouseUp { get; }

		/// <summary>
		/// Destroys the cell's game object.
		/// </summary>
		void Destroy();
	}
}