using DG.Tweening;
using System;
using UnityEngine;

namespace Match3Game
{
	public enum ChipType
	{
		Blue,
		Red,
		Green,
		Purple,
		Yellow,
		Orange,
	}

	/// <summary>
	/// Chip in the Match3 game.
	/// </summary>
	public class Chip : MonoBehaviour, IChip
	{
		[SerializeField]
		GameObject highlightObject;

		public ChipType Type { get; private set; }
		public ICell Cell { get; private set; }

		Vector3 currentPos;
		Vector3 targetPos;
		Vector2 startTouchPosition;
		Vector2 currentTouchPosition;
		bool isSwipe;

		BoxCollider2D boxCollider;
		Action<IChip> onClickAction;
		Action<IChip, Vector2> onSwipeAction;

		const int MINIMUM_SWAP_DISTANCE = 50;
		const float HIGHLIGHT_SCALE_TIME = 0.2f;
		const float MOVE_SPEED_PER_UNIT = 0.1f;
		const float DESTROY_TIME = 0.3f;

		void Awake()
		{
			boxCollider = GetComponent<BoxCollider2D>();
			boxCollider.enabled = false;
		}

		/// <summary>
		/// Strange collider behaviour fix;
		/// </summary>
		private void OnEnable()
		{
			boxCollider.enabled = true;
		}


		/// <summary>
		/// Initializes the chip with the specified type.
		/// </summary>
		/// <param name="type">The type of the chip.</param>
		public void Initialize(ChipType type)
		{
			Type = type;
		}

		/// <summary>
		/// Sets the callback actions for click and swipe events.
		/// </summary>
		/// <param name="onClickAction">The click callback action.</param>
		/// <param name="onSwipeAction">The swipe callback action.</param>
		public void SetCallbacks(Action<IChip> onClickAction, Action<IChip, Vector2> onSwipeAction)
		{
			this.onClickAction = onClickAction;
			this.onSwipeAction = onSwipeAction;
		}

		/// <summary>
		/// Sets the cell and parent transform for the chip.
		/// </summary>
		/// <param name="cell">The cell containing the chip.</param>
		/// <param name="cellTransform">The parent transform of the cell.</param>
		public void SetCell(ICell cell, Transform cellTransform)
		{
			Cell = cell;
			transform.SetParent(cellTransform, true);
			gameObject.name = $"Chip {cell.X} {cell.Y} {Type}";
		}

		/// <summary>
		/// Sets the current and target positions for the chip.
		/// </summary>
		/// <param name="current">The current position.</param>
		/// <param name="target">The target position.</param>
		public void SetPositions(Vector3 current, Vector3 target)
		{
			currentPos = current;
			targetPos = target;
			transform.localPosition = current;

		}

		/// <summary>
		/// Moves the chip to the target position.
		/// </summary>
		/// <param name="onComplete">Action to execute on completion.</param>
		public void Move(Action onComplete = null)
		{
			float time = Vector3.Distance(targetPos, currentPos) * MOVE_SPEED_PER_UNIT;
			transform.DOLocalMove(targetPos, time).OnComplete(() => onComplete?.Invoke());
		}

		/// <summary>
		/// Immediately moves the chip to the target position.
		/// </summary>
		/// <param name="onComplete">Action to execute on completion.</param>
		public void ImmediateMove(Action onComplete = null)
		{
			transform.localPosition = targetPos;
		}

		/// <summary>
		/// Destroys the chip with an animation.
		/// </summary>
		/// <param name="onComplete">Action to execute on completion.</param>
		public void DestroyChip(Action onComplete)
		{
			transform.DOScale(Vector3.zero, DESTROY_TIME).OnComplete(() =>
			{
				onComplete?.Invoke();
				Destroy(gameObject);
			});
		}

		/// <summary>
		/// Highlights selection of the chip.
		/// </summary>
		/// <param name="highlight">True to highlight, false to unhighlight.</param>
		public void Highlight(bool highlight)
		{
			highlightObject.SetActive(highlight);
			transform.DOScale(highlight ? Vector3.one * 1.1f : Vector3.one, HIGHLIGHT_SCALE_TIME);
		}

		void OnMouseDown()
		{
			startTouchPosition = Input.mousePosition;
			isSwipe = true;
			onClickAction?.Invoke(this);
		}

		void OnMouseUp()
		{
			isSwipe = false;
		}

		void OnMouseDrag()
		{
			if (!isSwipe)
			{
				return;
			}

			currentTouchPosition = Input.mousePosition;
			Vector2 direction = currentTouchPosition - startTouchPosition;

			if (direction.magnitude < MINIMUM_SWAP_DISTANCE)
			{
				return;
			}

			direction.Normalize();

			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				OnSwipe(direction.x > 0 ? Vector2.right : Vector2.left);
			}
			else
			{
				OnSwipe(direction.y > 0 ? Vector2.up : Vector2.down);
			}

		}

		void OnSwipe(Vector2 direction)
		{
			isSwipe = false;
			onSwipeAction?.Invoke(this, direction);
		}

		/// <summary>
		/// Determines if the chip can be swapped with another chip.
		/// </summary>
		/// <param name="otherChip">The other chip to check against.</param>
		/// <returns>True if the chips can be swapped, otherwise false.</returns>
		public bool CanSwapWith(IChip otherChip)
		{
			return otherChip != null && otherChip.Type != Type;
		}

		/// <summary>
		/// Changes the interactability of the chip.
		/// </summary>
		/// <param name="interactable">True to enable interaction, false to disable.</param>
		public void ChangeInteractability(bool interactable)
		{
			boxCollider.enabled = interactable;
		}
	}
}
