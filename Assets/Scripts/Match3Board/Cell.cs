using UnityEngine;

namespace Match3Game
{
	/// <summary>
	/// Class for a cell.
	/// </summary>
	public class Cell : MonoBehaviour, ICell
	{
		[SerializeField]
		GameObject highlighted;

		public int X { get; private set; }
		public int Y { get; private set; }
		public IChip Chip { get; set; }

		public System.Action<Vector2Int> OnMouseEnterCallback { get; set; }
		public System.Action<Vector2Int> OnMouseUpCallback { get; set; }

		/// <summary>
		/// Initializes the cell with coordinates.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		public void Initialize(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Sets the chip in the cell.
		/// </summary>
		/// <param name="chip">The chip to set.</param>
		public void SetChip(IChip chip)
		{
			Chip = chip;
			if (chip != null)
			{
				chip.SetCell(this, transform);
			}
		}

		void OnMouseEnter()
		{
			OnMouseEnterCallback?.Invoke(new Vector2Int(X, Y));
		}

		void OnMouseUp()
		{
			OnMouseUpCallback?.Invoke(new Vector2Int(X, Y));
		}

		/// <summary>
		/// Highlights or unhighlights the cell.
		/// </summary>
		/// <param name="on">True to highlight, false to unhighlight.</param>
		public void SetHighlighted(bool on)
		{
			highlighted.SetActive(on);
		}
	}
}
