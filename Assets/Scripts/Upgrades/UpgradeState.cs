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
		StorableReference<UpgradeData> upgradeData;

		[SerializeField]
		bool unlocked;

		/// <summary>
		/// Gets or sets the upgrade data.
		/// </summary>
		public StorableReference<UpgradeData> UpgradeData {
			get => upgradeData;
			set => upgradeData = value;
		}

		/// <summary>
		/// Gets or sets a unlock state.
		/// </summary>
		public bool Unlocked {
			get => unlocked;
			set => unlocked = value;
		}
	}
}
