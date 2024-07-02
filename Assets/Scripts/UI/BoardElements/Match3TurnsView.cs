using BaseCore;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3Game
{
	/// <summary>
	/// Event struct representing a change in turns.
	/// </summary>
	public struct TurnsChanged
	{
		public int Delta;
		public int NewValue;
	}

	/// <summary>
	/// View class for displaying Match3 game turns.
	/// </summary>
	public class Match3TurnsView : MonoBehaviour, IEventSubscriber<TurnsChanged>, IStartUpMonoBehaviourSubscriber, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		TextMeshProUGUI valueText;


		/// <summary>
		/// Receives and handles the <see cref="TurnsChanged"/> event.
		/// </summary>
		/// <param name="data">The turns change data.</param>
		void IEventSubscriber<TurnsChanged>.ReceiveEvent(TurnsChanged data)
		{
			UpdateValue(data.Delta, data.NewValue);
		}

		/// <summary>
		/// Updates the displayed turn value.
		/// </summary>
		/// <param name="delta">The turn delta.</param>
		/// <param name="value">The new turn value.</param>
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
		}

		/// <summary>
		/// Called when the pointer exits the object.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
		}
	}
}
