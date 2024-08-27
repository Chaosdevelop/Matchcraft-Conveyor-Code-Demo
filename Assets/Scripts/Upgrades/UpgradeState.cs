using BaseCore;
using UnityEngine;

namespace Upgrades
{
	/// <summary>
	/// Serializable state of an upgrade.
	/// </summary>
	[System.Serializable]
	public class UpgradeState
	{
		[SerializeField]
		StorableReference<UpgradeGroupData> upgradeDataReference;

		[SerializeField]
		int upgradeLevel;

		public UpgradeGroupData UpgradeGroupData => upgradeDataReference.Storable;

		public UpgradeState(UpgradeGroupData upgradeGroupData)
		{
			this.upgradeDataReference = new StorableReference<UpgradeGroupData>(upgradeGroupData);
		}


		/// <summary>
		/// Gets or sets a unlock state.
		/// </summary>
		public int UpgradeLevel {
			get => upgradeLevel;
			private set => upgradeLevel = value;
		}

		public void DoUpgrade(UpgradeContext upgradeContext)
		{
			GetNextUpgrade().ApplyUpgrade(upgradeContext);
			UpgradeLevel++;
		}

		public UpgradeData GetNextUpgrade()
		{
			var upgrades = upgradeDataReference.Storable.Upgrades;
			if (upgrades.Length > upgradeLevel)
			{
				return upgrades[upgradeLevel];
			}

			return null;
		}

		public void ApplyUpToLastLevel(UpgradeContext upgradeContext)
		{
			var upgrades = upgradeDataReference.Storable.Upgrades;
			for (int i = 0; i < UpgradeLevel; i++)
			{
				upgrades[upgradeLevel].ApplyUpgrade(upgradeContext);
			}
		}
	}
}
