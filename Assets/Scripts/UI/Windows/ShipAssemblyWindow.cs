using BaseCore;
using BaseCore.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Manages the Ship Assembly window functionality and display.
/// </summary>
public class ShipAssemblyWindow : BaseWindow
{
	[SerializeField]
	Button startCraftButton;

	[SerializeField]
	ShipPartPreview shipPartPreview;

	[SerializeField]
	EnumDictionary<ShipPartType, ShipPartCrafted> craftedViews = new EnumDictionary<ShipPartType, ShipPartCrafted>();

	[SerializeField]
	TextMeshProUGUI turns;

	[SerializeField]
	TextMeshProUGUI totalScores;

	[SerializeField]
	EnumDictionary<ItemStat, TextMeshProUGUI> totalStats = new EnumDictionary<ItemStat, TextMeshProUGUI>();

	[SerializeField]
	Button sellShipButton;

	[SerializeField]
	TextMeshProUGUI sellText;

	[Inject]
	GameManager gameManager;
	[Inject]
	PlayerProgress progress;

	public override WindowType WindowType => WindowType.ShipAssembly;

	void Awake()
	{
		startCraftButton.onClick.AddListener(StartCraft);
		sellShipButton.onClick.AddListener(SellShip);
	}

	void OnEnable()
	{
		UpdateContent();
	}

	/// <summary>
	/// Updates the content of the Ship Assembly window.
	/// </summary>
	void UpdateContent()
	{

		var parts = gameManager.GetParts();

		foreach (var item in parts)
		{
			craftedViews[item.Key].SetInfo(item.Value);
		}

		shipPartPreview.SetInfo(gameManager.GetCurrentCraftingPart().ShipPartInfo.Storable);
		turns.text = gameManager.TurnsPerCraft.ToString();
		totalScores.text = gameManager.GetTotalScores().ToString();

		var stats = gameManager.GetTotalStats();
		foreach (var statText in totalStats)
		{
			statText.Value.text = stats[statText.Key].ToString();
		}

		var shipCompleted = gameManager.IsShipCompleted();
		startCraftButton.gameObject.SetActive(!shipCompleted);
		sellShipButton.gameObject.SetActive(shipCompleted);

		if (shipCompleted)
		{
			var finalScores = gameManager.CalculateFinalScores();
			var coins = gameManager.ScoresToCoins(finalScores);
			sellText.text = coins.ToString();
		}
	}

	/// <summary>
	/// Starts the crafting process.
	/// </summary>
	void StartCraft()
	{
		EventSystem.SendEventToAll(new ScreenChangeEvent { ScreenType = ScreenType.Match3Board });
	}

	/// <summary>
	/// Sells the assembled ship and starts a new assembly process.
	/// </summary>
	void SellShip()
	{

		var finalScores = gameManager.CalculateFinalScores();
		var coins = gameManager.ScoresToCoins(finalScores);
		progress.ChangeCoins(coins);
		gameManager.StartNewShipAssembly();
		UpdateContent();
	}


}
