using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using BaseCore.Collections;
using UnityEngine;
using Upgrades;
using static GameSettingsInstaller;
using static ShipPartsInstaller;
using static UpgradesInstaller;


/// <summary>
/// Represents the player's progress for JSON serialization.
/// </summary>
[Serializable]
public class PlayerProgress
{
	[SerializeField]
	int currentCoins;

	[SerializeField]
	EnumDictionary<ShipPartType, ShipPartAssemblyResult> shipParts = new();

	[SerializeField]
	bool progressInitialized;

	[SerializeField]
	List<UpgradeState> upgradeStates = new();

	/// <summary>
	/// Gets or sets the current ship part type being assembled.
	/// </summary>
	[field: SerializeField]
	public ShipPartType CurrentShipPartType { get; set; }

	const ShipPartType DEFAULT_PART_TYPE = ShipPartType.Hull;

	/// <summary>
	/// Gets the current crafting part based on the current ship part type.
	/// </summary>
	public ShipPartAssemblyResult CurrentCraftingPart => shipParts[CurrentShipPartType];

	/// <summary>
	/// Gets or sets the current number of coins.
	/// </summary>
	public int CurrentCoins {
		get => currentCoins;
		set => currentCoins = value;
	}

	/// <summary>
	/// Gets or sets the ship parts dictionary.
	/// </summary>
	public EnumDictionary<ShipPartType, ShipPartAssemblyResult> ShipParts {
		get => shipParts;
		set => shipParts = value;
	}

	/// <summary>
	/// Gets the list of upgrade states.
	/// </summary>
	public List<UpgradeState> UpgradeStates => upgradeStates;

	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerProgress"/> class with the specified starting coins.
	/// </summary>
	/// <param name="playerValues">The player values containing the starting coin amount.</param>
	public PlayerProgress(PlayerValues playerValues)
	{
		currentCoins = playerValues.StartCoins;
	}

	/// <summary>
	/// Initializes the player progress, applying upgrades and setting up initial ship parts.
	/// </summary>
	/// <param name="upgrades">The array of upgrades.</param>
	/// <param name="predefinedParts">The predefined ship parts.</param>
	/// <param name="gameManager">The game manager.</param>
	public void Initialize(UpgradesArray upgrades, PredefinedParts predefinedParts, GameManager gameManager)
	{
		if (upgradeStates.Count > 0)
		{
			foreach (var upgradeState in upgradeStates)
			{
				upgradeState.ApplyUpToLastLevel(new UpgradeContext { GameManager = gameManager });
			}
		}

		if (progressInitialized)
		{
			return;
		}

		progressInitialized = true;
		CurrentShipPartType = DEFAULT_PART_TYPE;
		upgradeStates = new List<UpgradeState>();
		foreach (var upgrade in upgrades.AllUpgrades)
		{
			var state = new UpgradeState(upgrade);
			upgradeStates.Add(state);
		}

		foreach (var (key, value) in predefinedParts.Parts)
		{
			ShipParts[key] = new ShipPartAssemblyResult(value);
		}
	}

	/// <summary>
	/// Checks if the player has enough coins to spend.
	/// </summary>
	/// <param name="amount">The amount of coins to check.</param>
	/// <returns>True if the player has enough coins, otherwise false.</returns>
	public bool CanSpendCoins(int amount)
	{
		return CurrentCoins >= amount;
	}

	/// <summary>
	/// Changes the player's coin count and sends a resource changed event.
	/// </summary>
	/// <param name="amount">The amount to change the coin count by.</param>
	public void ChangeCoins(int amount)
	{
		CurrentCoins += amount;
		EventSystem.SendEventToAll(new ResourceChanged
		{ ResourceType = ResourceType.Manacoins, NewValue = CurrentCoins, Delta = amount });
	}

	/// <summary>
	/// Gets the total stats from all ship parts.
	/// </summary>
	/// <returns>An EnumDictionary containing the total stats.</returns>
	public EnumDictionary<ItemStat, int> GetTotalStats()
	{
		var statsDictionary = new EnumDictionary<ItemStat, int>();
		foreach (var part in shipParts.AsDictionary().Values)
		{
			foreach (var (key, value) in part.Stats)
			{
				statsDictionary[key] += value;
			}
		}

		return statsDictionary;
	}

	/// <summary>
	/// Gets the total scores from all ship parts.
	/// </summary>
	/// <returns>The total scores.</returns>
	public int GetTotalScores()
	{
		return shipParts.AsDictionary().Values.Sum(part => part.Scores);
	}

	/// <summary>
	/// Checks if the ship is completed.
	/// </summary>
	/// <returns>True if the ship is completed, otherwise false.</returns>
	public bool IsShipCompleted()
	{
		return shipParts.AsDictionary().Values.All(part => part.Done);
	}
}
