using BaseCore.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Upgrades
{
	/// <summary>
	/// Manages the upgrade system, tracking states and chains of upgrades.
	/// </summary>
	public class UpgradeManager : MonoBehaviour
	{
		[SerializeField]
		EnumDictionary<UpgradeChainType, UpgradesChain> upgradeChains = new EnumDictionary<UpgradeChainType, UpgradesChain>();

		public List<UpgradeState> upgradeStates;

		public System.Action<List<UpgradeState>> OnUpgraded;

		/// <summary>
		/// Sets the current upgrade states.
		/// </summary>
		/// <param name="upgradeStates">The list of upgrade states to set.</param>
		public void SetStates(List<UpgradeState> upgradeStates)
		{
			this.upgradeStates = upgradeStates;
		}

		/// <summary>
		/// Marks an upgrade as done, updating the state and triggering the OnUpgraded event.
		/// </summary>
		/// <param name="upgradeData">The upgrade data to mark as done.</param>
		public void UpgradeDone(UpgradeData upgradeData)
		{
			var state = upgradeStates.FirstOrDefault(arg => arg.UpgradeData.Storable == upgradeData);
			if (state != null)
			{
				state.Unlocked = true;
			}
			else
			{
				state = new UpgradeState
				{
					UpgradeData = new BaseCore.StorableReference<UpgradeData>(upgradeData),
					Unlocked = true
				};
				upgradeStates.Add(state);
			}
			OnUpgraded?.Invoke(upgradeStates);
		}

		/// <summary>
		/// Checks if a given upgrade is already unlocked.
		/// </summary>
		/// <param name="upgradeData">The upgrade data to check.</param>
		/// <returns>True if the upgrade is unlocked, otherwise false.</returns>
		bool IsUpgraded(UpgradeData upgradeData)
		{
			var upgradeState = upgradeStates.FirstOrDefault(arg => arg.UpgradeData.Storable == upgradeData);
			return upgradeState?.Unlocked ?? false;
		}

		/// <summary>
		/// Retrieves the upgrade chain for a given chain type.
		/// </summary>
		/// <param name="chainType">The type of the upgrade chain.</param>
		/// <returns>The upgrade chain corresponding to the specified type.</returns>
		public UpgradesChain GetUpgradesChain(UpgradeChainType chainType)
		{
			return upgradeChains[chainType];
		}

		/// <summary>
		/// Gets the next upgrade in a specified chain that has not been unlocked yet.
		/// </summary>
		/// <param name="chainType">The type of the upgrade chain.</param>
		/// <returns>The next upgrade data in the chain, or null if all are unlocked.</returns>
		public UpgradeData GetNextUpgradeInChain(UpgradeChainType chainType)
		{
			var chain = GetUpgradesChain(chainType);
			foreach (var item in chain.Upgrades)
			{
				if (!IsUpgraded(item))
				{
					return item;
				}
			}
			return null;
		}
	}
}
