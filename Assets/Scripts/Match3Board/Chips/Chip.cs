using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;

namespace Match3Game
{
	/// <summary>
	/// Represents the different types of chips in the Match3 game.
	/// </summary>
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

		/// <inheritdoc />
		public ChipType Type { get; private set; }

		/// <inheritdoc />
		public ICell Cell { get; private set; }

		Vector3 currentPos;
		Vector3 targetPos;
		Vector2 startTouchPosition;
		Vector2 currentTouchPosition;
		bool isSwipe;

		private readonly Subject<IChip> clickSubject = new Subject<IChip>();

		/// <inheritdoc />
		public Observable<IChip> Click => clickSubject;

		private readonly Subject<Vector2> swipeSubject = new Subject<Vector2>();

		/// <inheritdoc />
		public Observable<Vector2> Swipe => swipeSubject;

		const int MINIMUM_SWAP_DISTANCE = 50;
		const float HIGHLIGHT_SCALE_TIME = 0.2f;
		const float MOVE_SPEED_PER_UNIT = 0.1f;
		const float DESTROY_TIME = 0.3f;

		/// <inheritdoc />
		public void Initialize(ChipType type)
		{
			Type = type;
		}

		/// <inheritdoc />
		public void SetCell(ICell cell, Transform cellTransform)
		{
			Cell = cell;
			transform.SetParent(cellTransform, true);
			gameObject.name = $"Chip {cell.X} {cell.Y} {Type}";
		}

		/// <inheritdoc />
		public void SetPositions(Vector3 current, Vector3 target)
		{
			currentPos = current;
			targetPos = target;
			transform.localPosition = current;
		}

		/// <inheritdoc />
		public async UniTask Move()
		{
			float time = Vector3.Distance(targetPos, currentPos) * MOVE_SPEED_PER_UNIT;
			await transform.DOLocalMove(targetPos, time).AsyncWaitForCompletion();
		}

		/// <inheritdoc />
		public void ImmediateMove(Action onComplete = null)
		{
			transform.localPosition = targetPos;
		}

		/// <inheritdoc />
		public async UniTask DestroyChip()
		{
			await transform.DOScale(Vector3.zero, DESTROY_TIME).AsyncWaitForCompletion();
			Destroy(gameObject);
		}

		/// <inheritdoc />
		public void Highlight(bool highlight)
		{
			highlightObject.SetActive(highlight);
			transform.DOScale(highlight ? Vector3.one * 1.1f : Vector3.one, HIGHLIGHT_SCALE_TIME);
		}

		void OnMouseDown()
		{
			startTouchPosition = Input.mousePosition;
			clickSubject.OnNext(this);
			isSwipe = true;
		}

		void OnMouseDrag()
		{
			if (!isSwipe) return;

			currentTouchPosition = Input.mousePosition;
			Vector2 direction = currentTouchPosition - startTouchPosition;

			if (direction.magnitude < MINIMUM_SWAP_DISTANCE) return;

			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				isSwipe = false;
				swipeSubject.OnNext(direction.x > 0 ? Vector2.right : Vector2.left);
			}
			else
			{
				isSwipe = false;
				swipeSubject.OnNext(direction.y > 0 ? Vector2.up : Vector2.down);
			}
		}

		/// <inheritdoc />
		public bool CanSwapWith(IChip otherChip)
		{
			return otherChip != null && otherChip.Type != Type;
		}

		/// <inheritdoc />
		public void ChangeInteractability(bool interactable)
		{
			Collider2D collider = GetComponent<Collider2D>();
			if (collider != null)
			{
				collider.enabled = interactable;
			}
		}
	}
}