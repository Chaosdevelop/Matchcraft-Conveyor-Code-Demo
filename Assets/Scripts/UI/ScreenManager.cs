using BaseCore;
using BaseCore.Collections;
using UnityEngine;

/// <summary>
/// Enum representing different types of screens in the game.
/// </summary>
public enum ScreenType
{
	Welcome,
	Main,
	Match3Board,
}

/// <summary>
/// Manages the switching and activation of different screens in the game.
/// </summary>
public class ScreenManager : MonoBehaviour, IEventSubscriber<ScreenChangeEvent>, IStartUpMonoBehaviourSubscriber
{
	[SerializeField]
	EnumDictionary<ScreenType, GameObject> screens;

	/// <summary>
	/// Gets the current active screen type.
	/// </summary>
	public static ScreenType CurrentScreen { get; private set; }

	/// <summary>
	/// Initializes the screen manager by setting the active screen to the welcome screen.
	/// </summary>
	void Start()
	{
		SetActiveScreen(ScreenType.Welcome);
	}

	/// <summary>
	/// Sets the active screen based on the specified screen type.
	/// </summary>
	/// <param name="screenType">The screen type to activate.</param>
	void SetActiveScreen(ScreenType screenType)
	{
		foreach (var screen in screens)
		{
			screen.Value.SetActive(screen.Key == screenType);
		}
		CurrentScreen = screenType;
	}

	/// <summary>
	/// Handles the screen change event by setting the active screen.
	/// </summary>
	/// <param name="data">The screen change event data.</param>
	void IEventSubscriber<ScreenChangeEvent>.ReceiveEvent(ScreenChangeEvent data)
	{
		SetActiveScreen(data.ScreenType);
	}
}

/// <summary>
/// Interface for UI events.
/// </summary>
public interface IEventUI
{
	/// <summary>
	/// Sends the UI event.
	/// </summary>
	void Send();
}

/// <summary>
/// Event for opening a specific window.
/// </summary>
[System.Serializable]
public struct WindowOpenEvent : IEventUI
{
	public WindowType WindowType;

	/// <summary>
	/// Sends the window open event.
	/// </summary>
	public void Send()
	{
		EventSystem.SendEventToAll(this);
	}
}

/// <summary>
/// Event for changing the screen.
/// </summary>
[System.Serializable]
public struct ScreenChangeEvent : IEventUI
{
	public ScreenType ScreenType;

	/// <summary>
	/// Sends the screen change event.
	/// </summary>
	public void Send()
	{
		EventSystem.SendEventToAll(this);
	}
}
