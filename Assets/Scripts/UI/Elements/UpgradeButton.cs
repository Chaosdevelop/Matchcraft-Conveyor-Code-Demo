using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

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

	[field: SerializeField]
	public UpgradeChainType UpgradesChainType { get; private set; }

	public System.Action<UpgradeData> OnUpgrade;

	UpgradeData nextUpgrade;

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
		if (nextUpgrade != null)
		{
			costText.text = nextUpgrade.Cost.ToString();
			buyUpgradeButton.interactable = GameManager.Instance.CanSpendCoins(nextUpgrade.Cost);
			upgradeNameText.text = nextUpgrade.UpgradeName.GetLocalizedString();
			upgradeIcon.sprite = nextUpgrade.Icon;
		}
	}

	/// <summary>
	/// Sets the next available upgrade data for the button.
	/// </summary>
	/// <param name="upgradeData">The upgrade data to set.</param>
	public void SetNextUpgrade(UpgradeData upgradeData)
	{
		nextUpgrade = upgradeData;
		UpdateButton();
	}

	/// <summary>
	/// Handles the button click event for purchasing the upgrade.
	/// </summary>
	void OnClick()
	{
		if (nextUpgrade != null)
		{
			GameManager.Instance.ChangeCoins(-nextUpgrade.Cost);
			nextUpgrade.GetEffect().Apply();
			OnUpgrade?.Invoke(nextUpgrade);
			UpdateButton();
		}
	}
}
