using UnityEngine;
using UnityEngine.Localization;

namespace Upgrades
{
	/// <summary>
	/// Abstract base class for upgrade data. Stores common properties for all upgrades.
	/// </summary>
	public class UpgradeGroupData : StorableScriptableObject
	{
		/// <summary>
		/// Gets the localized name of the upgrade.
		/// </summary>
		[field: SerializeField]
		public LocalizedString UpgradeGroupName { get; private set; }

		/// <summary>
		/// Gets the icon representing the upgrade.
		/// </summary>
		[field: SerializeField]
		public Sprite Icon { get; private set; }

		/// <summary>
		/// Upgrades array.
		/// </summary>

		[field: SerializeField]
		public UpgradeData[] Upgrades { get; private set; }

	}


}
