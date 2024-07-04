using BaseCore;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component for simple sending event on button click.
/// </summary>
[RequireComponent(typeof(Button))]
public class EventButton : MonoBehaviour
{
	[SerializeReference]
	[TypeSelector]
	IEventUI eventToSend;

	Button button;

	/// <summary>
	/// Initializes the button and assigns the click listener on awake.
	/// </summary>
	void Awake()
	{
		button = GetComponent<Button>();
		button?.onClick.AddListener(OnButtonClick);
	}

	/// <summary>
	/// Removes the click listener when the object is destroyed.
	/// </summary>
	void OnDestroy()
	{
		button?.onClick.RemoveListener(OnButtonClick);
	}

	/// <summary>
	/// Handles the button click event, sending the specified event.
	/// </summary>
	void OnButtonClick()
	{
		eventToSend?.Send();
		Debug.Log($"Click on {gameObject.name.RichText(Color.cyan)} fired event {eventToSend.ToDetailedString().RichText(Color.yellow)}", this.gameObject);
	}
}
