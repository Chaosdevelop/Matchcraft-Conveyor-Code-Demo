using UnityEngine;

namespace Upgrades
{
	/// <summary>
	/// Types of upgrade chains.
	/// </summary>
	public enum UpgradeChainType
	{
		Skill1,
		Skill2,
		Skill3,
		Turns,
		Scores,
	}

	/// <summary>
	/// Represents a chain of upgrades.
	/// </summary>
	[System.Serializable]
	public class UpgradesChain
	{

		/// <summary>
		/// Array of upgrades in the chain.
		/// </summary>
		[field: SerializeField]
		public UpgradeData[] Upgrades { get; private set; }

	}
}