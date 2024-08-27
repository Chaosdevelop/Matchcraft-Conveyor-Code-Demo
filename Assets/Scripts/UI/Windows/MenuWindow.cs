using BaseCore;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Manages the menu window, including various settings and social media links.
/// </summary>
public class MenuWindow : BaseWindow
{
	public override WindowType WindowType => WindowType.Menu;

	[SerializeField]
	Button exitGame;

	[SerializeField]
	Button finishCraft;

	[SerializeField]
	Button resetData;

	[SerializeField]
	Button socialDiscord;

	[SerializeField]
	Button socialSteam;

	[SerializeField]
	TMP_Dropdown localeDropdown;

	[SerializeField]
	Slider soundsVolume;

	[SerializeField]
	Slider musicVolume;

	[Inject]
	GameManager gameManager;
	[Inject]
	AudioManager audioManager;
	[Inject]
	PlayerProgress progress;

	void Awake()
	{
		exitGame.onClick.AddListener(ExitGame);
		finishCraft.onClick.AddListener(OnFinishCraft);
		resetData.onClick.AddListener(ResetData);
		localeDropdown.onValueChanged.AddListener(ChangeLocale);
		socialDiscord.onClick.AddListener(() => Application.OpenURL("https://discord.gg/4HdZH9jP8h"));
		socialSteam.onClick.AddListener(() => Application.OpenURL("https://store.steampowered.com/app/1816920/Matchcraft_Efir_Adventure/"));
		soundsVolume.onValueChanged.AddListener(val => audioManager.SoundsVolume = val);
		musicVolume.onValueChanged.AddListener(val => audioManager.MusicVolume = val);
	}


	void OnEnable()
	{
		finishCraft.gameObject.SetActive(ScreenManager.CurrentScreen == ScreenType.Match3Board);
	}


	void Start()
	{
		soundsVolume.value = audioManager.SoundsVolume;
		musicVolume.value = audioManager.MusicVolume;
	}

	/// <summary>
	/// Handles the finish craft button click event.
	/// </summary>
	void OnFinishCraft()
	{
		EventSystem.SendEventToAll(new ScreenChangeEvent { ScreenType = ScreenType.Main });
		Close();
	}

	/// <summary>
	/// Resets the game data.
	/// </summary>
	void ResetData()
	{
		SaveManager.ResetData();
		SaveManager.Load(progress);
		Close();
		EventSystem.SendEventToAll(new ScreenChangeEvent { ScreenType = ScreenType.Main });
	}

	/// <summary>
	/// Exits the game application.
	/// </summary>
	void ExitGame()
	{
		Application.Quit();
	}

	/// <summary>
	/// Changes the locale based on the selected index.
	/// </summary>
	/// <param name="index">The selected locale index.</param>
	void ChangeLocale(int index)
	{
		string identifier = index == 0 ? "en" : "ru";
		LoadLocale(identifier);
	}

	/// <summary>
	/// Loads the specified locale.
	/// </summary>
	/// <param name="languageId">The language identifier.</param>
	public static void LoadLocale(string languageId)
	{
		LocalizationSettings settings = LocalizationSettings.Instance;
		LocaleIdentifier localeCode = new LocaleIdentifier(languageId);

		foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
		{
			if (locale.Identifier == localeCode)
			{
				LocalizationSettings.SelectedLocale = locale;
				PlayerPrefs.SetString("selected-locale", languageId);
				break;
			}
		}
	}
}
