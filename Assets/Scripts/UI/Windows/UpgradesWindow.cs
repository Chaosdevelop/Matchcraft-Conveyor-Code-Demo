using UnityEngine;
using Zenject;

/// <summary>
/// Manages the Upgrades window functionality and display.
/// </summary>
public class UpgradesWindow : BaseWindow
{
	[SerializeField]
	UpgradeButton upgradeButtonPrefab;
	[SerializeField]
	Transform buttonsArea;

	/*	[Inject]
		UpgradesArray upgrades;*/
	[Inject]
	PlayerProgress progress;
	[Inject]
	DiContainer dicontainer;

	public override WindowType WindowType => WindowType.Upgrades;


	void Start()
	{
		CreateContent();
	}

	/// <summary>
	/// Updates the content of the Upgrades window.
	/// </summary>
	void CreateContent()
	{

		foreach (var upgrade in progress.UpgradeStates)
		{
			var button = dicontainer.InstantiatePrefab(upgradeButtonPrefab).GetComponent<UpgradeButton>();
			//	var button = Instantiate(upgradeButtonPrefab);
			button.transform.SetParent(buttonsArea, false);
			button.SetUpgradeData(upgrade);
		}
	}
}
