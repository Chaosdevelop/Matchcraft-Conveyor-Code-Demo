using UnityEngine;

namespace Skills
{
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
}
