using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Match3Game
{
	/// <summary>
	/// Interface for chips.
	/// </summary>
	public interface IChip
	{
		/// <summary>
		/// Gets the cell that contains the chip.
		/// </summary>
		ICell Cell { get; }

		/// <summary>
		/// Gets the type of the chip.
		/// </summary>
		ChipType Type { get; }

		/// <summary>
		/// Initializes the chip with the given type.
		/// </summary>
		/// <param name="type">The type of the chip.</param>
		void Initialize(ChipType type);

		/// <summary>
		/// Sets the cell and parent transform for the chip.
		/// </summary>
		/// <param name="cell">The cell that contains the chip.</param>
		/// <param name="cellTransform">The parent transform of the cell.</param>
		void SetCell(ICell cell, Transform cellTransform);

		/// <summary>
		/// Asynchronously moves the chip to the target position.
		/// </summary>
		/// <returns>A UniTask that represents the asynchronous move operation.</returns>
		UniTask Move();

		/// <summary>
		/// Immediately moves the chip to the target position.
		/// </summary>
		/// <param name="onComplete">An optional action to execute when the move is complete.</param>
		void ImmediateMove(Action onComplete = null);

		/// <summary>
		/// Asynchronously destroys the chip with an animation.
		/// </summary>
		/// <returns>A UniTask that represents the asynchronous destroy operation.</returns>
		UniTask DestroyChip();

		/// <summary>
		/// Sets the current and target positions for the chip.
		/// </summary>
		/// <param name="current">The current position.</param>
		/// <param name="target">The target position.</param>
		void SetPositions(Vector3 current, Vector3 target);

		/// <summary>
		/// Highlights or unhighlights the chip.
		/// </summary>
		/// <param name="state">True to highlight the chip, false otherwise.</param>
		void Highlight(bool state);

		/// <summary>
		/// Checks if the chip can be swapped with another chip.
		/// </summary>
		/// <param name="otherChip">The other chip to check.</param>
		/// <returns>True if the chip can be swapped with the other chip, false otherwise.</returns>
		bool CanSwapWith(IChip otherChip);

		/// <summary>
		/// Changes the interactability of the chip.
		/// </summary>
		/// <param name="interactable">True to make the chip interactable, false otherwise.</param>
		void ChangeInteractability(bool interactable);

		/// <summary>
		/// An observable stream that emits the chip itself when it is clicked.
		/// </summary>
		Observable<IChip> Click { get; }

		/// <summary>
		/// An observable stream that emits the direction of the swipe when the chip is swiped.
		/// </summary>
		Observable<Vector2> Swipe { get; }
	}
}