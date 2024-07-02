using UnityEngine;

namespace Skills
{
	/// <summary>
	/// Interface for applying enhancements to a skill model.
	/// </summary>
	public interface ISkillEnhancement
	{
		/// <summary>
		/// Applies the enhancement to the specified skill model.
		/// </summary>
		/// <param name="skillModel">The skill model to enhance.</param>
		void Apply(SkillModel skillModel);
	}

	/// <summary>
	/// Enhancement that increases the maximum charges of a skill.
	/// </summary>
	[System.Serializable]
	public class MaxChargesEffect : ISkillEnhancement
	{
		[SerializeField]
		int additionalMaxCharges;

		/// <summary>
		/// Applies the enhancement to increase the maximum charges.
		/// </summary>
		/// <param name="skillModel">The skill model to enhance.</param>
		public void Apply(SkillModel skillModel)
		{
			skillModel.SkillCost.AddMaxCharges(additionalMaxCharges);
		}
	}

	/// <summary>
	/// Enhancement that increases the starting energy of a skill.
	/// </summary>
	[System.Serializable]
	public class StartEnergyEffect : ISkillEnhancement
	{
		[SerializeField]
		int additionalStartCharges;

		/// <summary>
		/// Applies the enhancement to increase the starting energy.
		/// </summary>
		/// <param name="skillModel">The skill model to enhance.</param>
		public void Apply(SkillModel skillModel)
		{
			skillModel.SkillCost.StartEnergy += additionalStartCharges;
		}
	}

	/// <summary>
	/// Enhancement that enables a skill.
	/// </summary>
	[System.Serializable]
	public class EnableSkillEffect : ISkillEnhancement
	{
		/// <summary>
		/// Applies the enhancement to enable the skill.
		/// </summary>
		/// <param name="skillModel">The skill model to enhance.</param>
		public void Apply(SkillModel skillModel)
		{
			skillModel.SkillAvailable = true;
		}
	}
}
