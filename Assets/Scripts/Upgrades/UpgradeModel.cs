using UnityEngine.Localization;

namespace Upgrades
{
	/// <summary>
	/// Represents the model for an upgrade, including its properties and state.
	/// </summary>
	public class UpgradeModel
	{
		/// <summary>
		/// Gets the localized name of the upgrade.
		/// </summary>
		public LocalizedString UpgradeName { get; }

		/// <summary>
		/// Gets the localized description of the upgrade.
		/// </summary>
		public LocalizedString Description { get; }

		/// <summary>
		/// Gets the cost of the upgrade.
		/// </summary>
		public int Cost { get; }

		/// <summary>
		/// Gets the effect of the upgrade.
		/// </summary>
		public IUpgradeEffect Effect { get; }

		/// <summary>
		/// Gets a value indicating whether the upgrade is unlocked.
		/// </summary>
		public bool IsUnlocked { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="UpgradeModel"/> class using the specified upgrade data.
		/// </summary>
		/// <param name="data">The data for the upgrade.</param>
		public UpgradeModel(UpgradeData data)
		{
			UpgradeName = data.UpgradeName;
			Description = data.Description;
			Cost = data.Cost;
			Effect = data.GetEffect();
			IsUnlocked = false;
		}

		/// <summary>
		/// Unlocks the upgrade and applies its effect.
		/// </summary>
		public void Unlock()
		{
			IsUnlocked = true;
			// Effect.Apply();
		}
	}
}
