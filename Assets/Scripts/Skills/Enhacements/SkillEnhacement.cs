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
}
