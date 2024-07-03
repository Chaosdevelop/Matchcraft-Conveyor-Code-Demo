using BaseCore;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3Game
{
	/// <summary>
	/// Event struct representing a scores change.
	/// </summary>
	public struct ScoresChanged
	{
		public int Delta;
		public int NewValue;
	}

	/// <summary>
	/// View class for displaying Match3 game scores.
	/// </summary>
	public class Match3ScoresView : MonoBehaviour, IEventSubscriber<ScoresChanged>, IStartUpMonoBehaviourSubscriber, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		TextMeshProUGUI valueText;

		/// <summary>
		/// Called when the object becomes enabled and active.
		/// </summary>
		void OnEnable()
		{
			UpdateValue(0, 0);
		}

		/// <summary>
		/// Receives and handles the <see cref="ScoresChanged"/> event.
		/// </summary>
		/// <param name="data">The score change data.</param>
		void IEventSubscriber<ScoresChanged>.ReceiveEvent(ScoresChanged data)
		{
			UpdateValue(data.Delta, data.NewValue);
		}

		/// <summary>
		/// Updates the displayed score value.
		/// </summary>
		/// <param name="delta">The score delta.</param>
		/// <param name="value">The new score value.</param>
		void UpdateValue(int delta, int value)
		{
			valueText.text = value.ToString();
		}

		/// <summary>
		/// Called when the object is being destroyed.
		/// </summary>
		void OnDestroy()
		{
			this.SelfUnsubscribe();
		}

		/// <summary>
		/// Called when the pointer enters the object.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			// Handle pointer enter event
		}

		/// <summary>
		/// Called when the pointer exits the object.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			// Handle pointer exit event
		}
	}
}
