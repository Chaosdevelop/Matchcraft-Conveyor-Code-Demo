using UnityEngine;

/// <summary>
/// Manages the Upgrades window functionality and display.
/// </summary>
public class UpgradesWindow : BaseWindow
{
	[SerializeField]
	UpgradeButton[] upgradeButtons;

	public override WindowType WindowType => WindowType.Upgrades;

	void OnEnable()
	{
		UpdateContent();
	}

	/// <summary>
	/// Updates the content of the Upgrades window.
	/// </summary>
	void UpdateContent()
	{
		var upgradeManager = GameManager.Instance.UpgradeManager;

		foreach (var button in upgradeButtons)
		{
			var nextUpgrade = upgradeManager.GetNextUpgradeInChain(button.UpgradesChainType);
			button.SetNextUpgrade(nextUpgrade);
			button.OnUpgrade = upgrade =>
			{
				upgradeManager.UpgradeDone(upgrade);
				var nextUp = upgradeManager.GetNextUpgradeInChain(button.UpgradesChainType);
				button.SetNextUpgrade(nextUp);
			};
		}
	}
}
