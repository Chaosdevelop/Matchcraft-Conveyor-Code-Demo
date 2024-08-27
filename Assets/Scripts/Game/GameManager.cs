using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using BaseCore.Collections;
using Skills;
using UnityEngine;
using static GameSettingsInstaller;
using static ShipPartsInstaller;
using static SkillsInstaller;
using static UpgradesInstaller;


/// <summary>
/// Manages the overall game state and player progress.
/// </summary>
public class GameManager
{
	ShipParts shipParts;
	PlayerProgress progress;

	int initialTurns;
	int initialScoresPerMatch;

	int additionalTurnsPerCraft;
	int additionalScoresPerMatch;

	/// <summary>
	/// Gets the number of turns per craft.
	/// </summary>
	public int TurnsPerCraft => initialTurns + additionalTurnsPerCraft;

	/// <summary>
	/// Gets the number of scores awarded per match.
	/// </summary>
	public int ScoresPerMatch => initialScoresPerMatch + additionalScoresPerMatch;

	/// <summary>
	/// Gets the dictionary of skill models, indexed by skill slot.
	/// </summary>
	public EnumDictionary<SkillSlot, SkillModel> Skills { get; } = new();

	List<ShipPartInfo> shipPartsAll;

	/// <summary>
	/// Initializes a new instance of the <see cref="GameManager"/> class.
	/// </summary>
	/// <param name="progress">The player progress data.</param>
	/// <param name="defaultParts">The default ship parts.</param>
	/// <param name="shipParts">The collection of ship parts.</param>
	/// <param name="skills">The game skills data.</param>
	/// <param name="upgrades">The array of upgrades.</param>
	/// <param name="playerValues">The player values.</param>
	public GameManager(PlayerProgress progress, PredefinedParts defaultParts, ShipParts shipParts, GameSkills skills,
		UpgradesArray upgrades, PlayerValues playerValues)
	{
		this.progress = progress;
		this.shipParts = shipParts;

		SetSettings(playerValues);

		SaveManager.Load(progress);
		progress.Initialize(upgrades, defaultParts, this);

		EventSystem.SendEventToAll(new ResourceChanged { NewValue = progress.CurrentCoins });

		shipPartsAll = StorableStorage.GetStorablesOfType<ShipPartInfo>();

		foreach (var (key, value) in skills.SkillsData)
		{
			Skills[key] = value.CreateSkillModel();
		}
	}

	void SetSettings(PlayerValues playerValues)
	{
		initialTurns = playerValues.TurnsForCraft;
		initialScoresPerMatch = playerValues.ScoresPerMatch;
	}

	/// <summary>
	/// Adds the specified number of turns to the turns per craft.
	/// </summary>
	/// <param name="value">The number of turns to add.</param>
	public void AddTurnsForCraft(int value)
	{
		additionalTurnsPerCraft += value;
	}

	/// <summary>
	/// Adds the specified number of scores to the scores per match.
	/// </summary>
	/// <param name="value">The number of scores to add.</param>
	public void AddScoresPerMatch(int value)
	{
		additionalScoresPerMatch += value;
	}

	/// <summary>
	/// Starts a new ship assembly.
	/// </summary>
	public void StartNewShipAssembly()
	{
		var partTypes = Enum.GetValues(typeof(ShipPartType));
		foreach (ShipPartType partType in partTypes)
		{
			var randomPart = GenerateItemForCraft(partType);
			progress.ShipParts[partType] = new ShipPartAssemblyResult(randomPart);
		}
	}

	/// <summary>
	/// Advances to the next ship part type.
	/// </summary>
	public void NextShipType()
	{
		progress.CurrentShipPartType = progress.CurrentShipPartType switch
		{
			ShipPartType.Hull => ShipPartType.Engine,
			ShipPartType.Engine => ShipPartType.Weapon,
			ShipPartType.Weapon => ShipPartType.Utility,
			ShipPartType.Utility => ShipPartType.Hull,
			_ => throw new NotSupportedException()
		};
	}

	/// <summary>
	/// Checks if the ship is fully assembled.
	/// </summary>
	/// <returns>True if the ship is completed, otherwise false.</returns>
	public bool IsShipCompleted()
	{
		return progress.IsShipCompleted();
	}

	/// <summary>
	/// Generates a random item for crafting based on the ship part type.
	/// </summary>
	/// <param name="shipPartType">The type of ship part to generate.</param>
	/// <returns>The generated ship part info.</returns>
	public ShipPartInfo GenerateItemForCraft(ShipPartType shipPartType)
	{
		return shipPartsAll
			.Where(arg => arg.PartType == shipPartType && arg.UseCommonGeneration)
			.ToList()
			.PickRandom();
	}

	/// <summary>
	/// Gets the current crafting part.
	/// </summary>
	/// <returns>The current crafting part.</returns>
	public ShipPartAssemblyResult GetCurrentCraftingPart()
	{
		return progress.CurrentCraftingPart;
	}

	/// <summary>
	/// Gets all ship parts.
	/// </summary>
	/// <returns>An enum dictionary of all ship parts.</returns>
	public EnumDictionary<ShipPartType, ShipPartAssemblyResult> GetParts()
	{
		return progress.ShipParts;
	}

	/// <summary>
	/// Gets the total stats from all ship parts.
	/// </summary>
	/// <returns>An enum dictionary of total stats.</returns>
	public EnumDictionary<ItemStat, int> GetTotalStats()
	{
		return progress.GetTotalStats();
	}

	/// <summary>
	/// Gets the total scores from all ship parts.
	/// </summary>
	/// <returns>The total scores.</returns>
	public int GetTotalScores()
	{
		return progress.GetTotalScores();
	}

	/// <summary>
	/// Calculates the final scores based on the total stats and scores.
	/// </summary>
	/// <returns>The calculated final scores.</returns>
	public int CalculateFinalScores()
	{
		var statSum = GetTotalStats().Sum(arg => arg.Value);
		var scores = GetTotalScores();
		return (int)(scores * (1 + 0.1f * statSum));
	}

	/// <summary>
	/// Converts scores to coins.
	/// </summary>
	/// <param name="scores">The scores to convert.</param>
	/// <returns>The converted coin value.</returns>
	public int ScoresToCoins(int scores)
	{
		var coins = scores / 100;
		return Mathf.Max(coins, 1);
	}
}
