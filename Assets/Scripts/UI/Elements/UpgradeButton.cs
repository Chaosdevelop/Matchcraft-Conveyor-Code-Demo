using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;
using Zenject;

/// <summary>
/// Manages the upgrade button functionality and display.
/// </summary>
public class UpgradeButton : MonoBehaviour
{
	[SerializeField]
	Button buyUpgradeButton;

	[SerializeField]
	TextMeshProUGUI costText;

	[SerializeField]
	TextMeshProUGUI upgradeNameText;

	[SerializeField]
	Image upgradeIcon;


	UpgradeState upgradeState;

	[Inject]
	PlayerProgress progress;
	[Inject]
	GameManager gameManager;
	/// <summary>
	/// Called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		buyUpgradeButton.onClick.AddListener(OnClick);
	}



	/// <summary>
	/// Updates the upgrade button with the next available upgrade data.
	/// </summary>
	void UpdateButton()
	{
		buyUpgradeButton.interactable = false;
		upgradeNameText.text = upgradeState.UpgradeGroupData.UpgradeGroupName.GetLocalizedString();
		upgradeIcon.sprite = upgradeState.UpgradeGroupData.Icon;
		var nextUpgrade = upgradeState.GetNextUpgrade();
		Debug.Log($"nextUpgrade {nextUpgrade}");
		if (nextUpgrade != null)
		{
			costText.text = nextUpgrade.Cost.ToString();
			buyUpgradeButton.interactable = progress.CanSpendCoins(nextUpgrade.Cost);
		}
		else
		{
			costText.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Sets the next available upgrade data for the button.
	/// </summary>
	/// <param name="upgradeData">The upgrade data to set.</param>
	public void SetUpgradeData(UpgradeState upgradeState)
	{
		this.upgradeState = upgradeState;
		UpdateButton();
	}

	/// <summary>
	/// Handles the button click event for purchasing the upgrade.
	/// </summary>
	void OnClick()
	{
		var nextUpgrade = upgradeState.GetNextUpgrade();
		if (nextUpgrade != null)
		{
			progress.ChangeCoins(-nextUpgrade.Cost);
			upgradeState.DoUpgrade(new UpgradeContext { GameManager = gameManager });
			UpdateButton();
		}
	}
}
