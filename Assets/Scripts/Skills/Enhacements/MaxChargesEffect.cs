using UnityEngine;

namespace Skills
{
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
}
