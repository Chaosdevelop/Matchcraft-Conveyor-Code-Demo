namespace Skills
{
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
