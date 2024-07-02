using BaseCore;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Enum representing different types of resources in the game.
/// </summary>
public enum ResourceType
{
	Manacoins
}

/// <summary>
/// View class for displaying resource information.
/// </summary>
public class ResourceView : MonoBehaviour, IEventSubscriber<ResourceChanged>, IStartUpMonoBehaviourSubscriber, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	ResourceType resourceType;

	[SerializeField]
	Image iconImage;

	[SerializeField]
	TextMeshProUGUI valueText;

	/// <summary>
	/// Receives and handles the <see cref="ResourceChanged"/> event.
	/// </summary>
	/// <param name="data">The resource change data.</param>
	void IEventSubscriber<ResourceChanged>.ReceiveEvent(ResourceChanged data)
	{
		if (data.ResourceType == resourceType)
		{
			UpdateValue(data.Delta, data.NewValue);
		}
	}

	/// <summary>
	/// Updates the displayed resource value.
	/// </summary>
	/// <param name="delta">The resource delta.</param>
	/// <param name="value">The new resource value.</param>
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

/// <summary>
/// Event struct representing a change in resources.
/// </summary>
public struct ResourceChanged
{
	public ResourceType ResourceType;
	public int Delta;
	public int NewValue;
}
