using R3;
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

		/// <inheritdoc />
		public int X { get; private set; }

		/// <inheritdoc />
		public int Y { get; private set; }

		/// <inheritdoc />
		public IChip Chip { get; set; }

		private readonly Subject<Vector2Int> mouseEnterSubject = new();

		/// <inheritdoc />
		public Observable<Vector2Int> MouseEnter => mouseEnterSubject;

		private readonly Subject<Vector2Int> mouseUpSubject = new();

		/// <inheritdoc />
		public Observable<Vector2Int> MouseUp => mouseUpSubject;

		/// <inheritdoc />
		public void Initialize(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <inheritdoc />
		public void SetChip(IChip chip)
		{
			Chip = chip;
			chip?.SetCell(this, transform);
		}

		void OnMouseEnter()
		{
			mouseEnterSubject.OnNext(new Vector2Int(X, Y));
		}

		void OnMouseUp()
		{
			mouseUpSubject.OnNext(new Vector2Int(X, Y));
		}

		/// <inheritdoc />
		public void SetHighlighted(bool on)
		{
			highlighted.SetActive(on);
		}

		/// <inheritdoc />
		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}