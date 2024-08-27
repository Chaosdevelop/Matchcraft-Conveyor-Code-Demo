using System;
using BaseCore;
using UnityEngine;
using UnityEngine.Localization;

namespace Upgrades
{
	[Serializable]
	public class UpgradeData
	{
		/// <summary>
		/// Gets the localized name of the upgrade.
		/// </summary>
		[field: SerializeField]
		public LocalizedString Name { get; private set; }

		/// <summary>
		/// Gets the localized description of the upgrade.
		/// </summary>
		[field: SerializeField]
		public LocalizedString Description { get; private set; }

		[SerializeReference]
		[TypeSelector]
		IUpgradeEffect effect;
		[field: SerializeField]
		public int Cost { get; private set; }

		public void ApplyUpgrade(UpgradeContext upgradeContext)
		{
			effect?.Apply(upgradeContext);
		}

	}


}
