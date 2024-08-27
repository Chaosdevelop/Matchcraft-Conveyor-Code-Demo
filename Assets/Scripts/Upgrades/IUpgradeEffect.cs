using BaseCore;
using Skills;
using UnityEngine;

namespace Upgrades
{

	public struct UpgradeContext
	{
		public GameManager GameManager;
	}
	/// <summary>
	/// Defines a base interface for upgrade effects.
	/// </summary>
	public interface IUpgradeEffect
	{
		/// <summary>
		/// Applies the upgrade effect.
		/// </summary>
		void Apply(UpgradeContext upgradeContext);
	}

	/// <summary>
	/// Defines an interface for skill upgrade effects.
	/// </summary>
	public interface ISkillUpgradeEffect : IUpgradeEffect { }

	/// <summary>
	/// Defines an interface for macro upgrade effects.
	/// </summary>
	public interface IMacroUpgradeEffect : IUpgradeEffect { }

	/// <summary>
	/// Represents a skill upgrade effect.
	/// </summary>
	[System.Serializable]
	public class SkillUpgradeEffect : ISkillUpgradeEffect
	{
		[SerializeField]
		SkillSlot targetSkillSlot;

		[SerializeReference]
		[TypeSelector]
		ISkillEnhancement[] skillEnhancements;


		/// <summary>
		/// Applies the skill upgrade effect.
		/// </summary>
		public void Apply(UpgradeContext upgradeContext)
		{

			var skill = upgradeContext.GameManager.Skills[targetSkillSlot];
			foreach (var item in skillEnhancements)
			{
				item.Apply(skill);
			}
		}
	}

	/// <summary>
	/// Represents an effect that grants extra turns.
	/// </summary>
	public class ExtraTurnsEffect : IMacroUpgradeEffect
	{
		[SerializeField]
		int turns;

		/// <summary>
		/// Applies the extra turns effect.
		/// </summary>
		public void Apply(UpgradeContext upgradeContext)
		{
			upgradeContext.GameManager.AddTurnsForCraft(turns);
		}
	}

	/// <summary>
	/// Represents an effect that grants extra scores.
	/// </summary>
	public class ExtraScoresEffect : IMacroUpgradeEffect
	{
		[SerializeField]
		int scores;

		/// <summary>
		/// Applies the extra scores effect.
		/// </summary>
		public void Apply(UpgradeContext upgradeContext)
		{

			upgradeContext.GameManager.AddScoresPerMatch(scores);
		}
	}
}
