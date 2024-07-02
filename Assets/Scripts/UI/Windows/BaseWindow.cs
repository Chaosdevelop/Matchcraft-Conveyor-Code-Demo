using BaseCore;
using UnityEngine;

/// <summary>
/// Enum representing different types of windows in the game.
/// </summary>
public enum WindowType
{
	ShipAssembly,
	Upgrades,
	Stats,
	Menu,
}

/// <summary>
/// Abstract base class for managing windows in the UI.
/// Implements event subscription for opening windows.
/// </summary>
public abstract class BaseWindow : MonoBehaviour, IEventSubscriber<WindowOpenEvent>, IStartUpMonoBehaviourSubscriber
{
	/// <summary>
	/// Gets the type of the window.
	/// </summary>
	public abstract WindowType WindowType { get; }

	/// <summary>
	/// Called when the object is being destroyed.
	/// Unsubscribes from events.
	/// </summary>
	void OnDestroy()
	{
		this.SelfUnsubscribe();
	}

	/// <summary>
	/// Opens the window.
	/// </summary>
	public virtual void Open()
	{
		gameObject.SetActive(true);
		OnOpen();
	}

	/// <summary>
	/// Closes the window.
	/// </summary>
	public virtual void Close()
	{
		OnClose();
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Method called when the window is opened.
	/// Can be overridden in derived classes for specific behavior.
	/// </summary>
	protected virtual void OnOpen() { }

	/// <summary>
	/// Method called when the window is closed.
	/// Can be overridden in derived classes for specific behavior.
	/// </summary>
	protected virtual void OnClose() { }

	/// <summary>
	/// Receives the window open event and opens the window if the type matches.
	/// </summary>
	/// <param name="data">The window open event data.</param>
	public void ReceiveEvent(WindowOpenEvent data)
	{
		if (data.WindowType == WindowType)
		{
			Open();
		}
	}
}
