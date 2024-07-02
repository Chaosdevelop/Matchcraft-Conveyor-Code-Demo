using System;
using UnityEngine;

namespace Match3Game
{
	/// <summary>
	/// Interface for cells.
	/// </summary>
	public interface ICell
	{
		int X { get; }
		int Y { get; }
		IChip Chip { get; }

		void Initialize(int x, int y);
		void SetChip(IChip chip);
		void SetHighlighted(bool on);

		Action<Vector2Int> OnMouseEnterCallback { get; set; }
		Action<Vector2Int> OnMouseUpCallback { get; set; }
	}

	/// <summary>
	/// Interface for chips.
	/// </summary>
	public interface IChip
	{
		ICell Cell { get; }
		ChipType Type { get; }

		void Initialize(ChipType type);
		void SetCell(ICell cell, Transform cellTransform);
		void Move(Action onComplete = null);
		void ImmediateMove(Action onComplete = null);
		void DestroyChip(Action onComplete = null);

		void SetPositions(Vector3 current, Vector3 target);
		void SetCallbacks(Action<IChip> onClickAction, Action<IChip, Vector2> onSwipeAction);
		void Highlight(bool state);
		bool CanSwapWith(IChip otherChip);
		void ChangeInteractability(bool interactable);
	}
}
