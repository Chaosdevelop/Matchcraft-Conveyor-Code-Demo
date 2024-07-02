using UnityEngine;

namespace Upgrades
{
	/// <summary>
	/// Skill upgrade effects data.
	/// </summary>
	[CreateAssetMenu(fileName = "SkillUpgradeData", menuName = "Upgrades/SkillUpgradeData", order = 1)]
	public class SkillUpgradeData : GenericUpgradeData<ISkillUpgradeEffect>
	{

	}
}