using Match3Game;
using Skills;
using UnityEngine;
using Zenject;

namespace UI
{

	/// <summary>
	/// Interface for creating SkillView instances.
	/// </summary>
	public interface ISkillViewFactory
	{
		/// <summary>
		/// Creates a new SkillView for the specified skill slot.
		/// </summary>
		/// <param name="slot">The skill slot.</param>
		/// <param name="skillModel">The skill model.</param>
		/// <returns>The created SkillView.</returns>
		SkillView CreateSkillView(SkillSlot slot, SkillModel skillModel);
	}

	/// <summary>
	/// Factory for creating SkillView instances.
	/// </summary>
	public class SkillViewFactory : ISkillViewFactory
	{
		readonly DiContainer container;
		readonly SkillView skillViewPrefab;
		readonly Transform skillsTransform;
		readonly GridController gridController;

		/// <summary>
		/// Constructor for SkillViewFactory.
		/// </summary>
		/// <param name="container">The DI container.</param>
		/// <param name="skillViewPrefab">The SkillView prefab.</param>
		/// <param name="skillsTransform">The parent transform for skill views.</param>
		public SkillViewFactory(DiContainer container, SkillView skillViewPrefab, Transform skillsTransform, GridController gridController)
		{
			this.container = container;
			this.skillViewPrefab = skillViewPrefab;
			this.skillsTransform = skillsTransform;
			this.gridController = gridController;
		}

		/// <summary>
		/// Creates a new SkillView for the specified skill slot.
		/// </summary>
		/// <param name="slot">The skill slot.</param>
		/// <param name="skillModel">The skill model.</param>
		/// <param name="gridController">The grid controller.</param>
		/// <returns>The created SkillView.</returns>
		public SkillView CreateSkillView(SkillSlot slot, SkillModel skillModel)
		{
			SkillView skillView = container.InstantiatePrefabForComponent<SkillView>(skillViewPrefab, skillsTransform);
			skillView.name = $"SkillView {slot}";
			skillView.Initialize(skillModel, gridController);
			return skillView;
		}
	}
}
