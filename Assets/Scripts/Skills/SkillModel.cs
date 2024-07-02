using Match3Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Skills
{
	/// <summary>
	/// Enum representing different skill slots.
	/// </summary>
	public enum SkillSlot
	{
		Yellow,
		Orange,
		Purple
	}

	/// <summary>
	/// Represents a skill model containing all relevant data and behavior.
	/// </summary>
	public class SkillModel
	{
		public LocalizedString SkillName { get; }
		public LocalizedString Description { get; }
		public ISkillPattern Pattern { get; }
		public ISkillEffect Effect { get; }
		public SkillCost SkillCost { get; }
		public SkillSlot SkillSlot { get; }
		public Sprite Icon { get; }

		public bool IsActiveTargeting { get; private set; }
		public bool SkillAvailable { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SkillModel"/> class.
		/// </summary>
		/// <param name="data">The data for the skill.</param>
		public SkillModel(SkillData data)
		{
			SkillName = data.SkillName;
			Description = data.Description;
			SkillCost = new SkillCost(data.BaseEnergyCost, data.MaxCharges);
			Pattern = data.SkillPattern;
			Effect = data.SkillEffect;
			SkillSlot = data.SkillSlot;
			Icon = data.Icon;
		}

		/// <summary>
		/// Determines if the skill can be used based on current charges.
		/// </summary>
		/// <returns>True if the skill can be used, otherwise false.</returns>
		public bool CanUseSkill() => SkillCost.CurrentCharges > 0;

		/// <summary>
		/// Activates the skill targeting.
		/// </summary>
		public void ActivateTargeting()
		{
			if (CanUseSkill())
			{
				IsActiveTargeting = true;
			}
		}

		/// <summary>
		/// Deactivates the skill targeting.
		/// </summary>
		public void DeactivateTargeting()
		{
			IsActiveTargeting = false;
		}

		/// <summary>
		/// Spends a charge of the skill if available.
		/// </summary>
		public void SpendCharge()
		{
			if (CanUseSkill())
			{
				SkillCost.SpendCharge();
			}
		}

		/// <summary>
		/// Adds energy to the skill.
		/// </summary>
		/// <param name="energy">The amount of energy to add.</param>
		public void AddEnergy(int energy)
		{
			SkillCost.AddEnergy(energy);
		}

		/// <summary>
		/// Resets the skill's energy.
		/// </summary>
		public void ResetEnergy()
		{
			SkillCost.ResetEnergy();
		}

		/// <summary>
		/// Applies the skill to the grid at the specified origin.
		/// </summary>
		/// <param name="gridController">The grid controller managing the game grid.</param>
		/// <param name="origin">The origin position of the skill application.</param>
		/// <param name="rows">The number of rows in the grid.</param>
		/// <param name="columns">The number of columns in the grid.</param>
		public void ApplySkill(GridController gridController, Vector2Int origin, int rows, int columns)
		{
			var affectedCells = Pattern.GetAffectedCells(origin, rows, columns);
			Effect.Apply(affectedCells, gridController);
		}

		/// <summary>
		/// Applies a collection of enhancements to the skill.
		/// </summary>
		/// <param name="enhancements">The enhancements to apply.</param>
		public void ApplyEnhancements(IEnumerable<ISkillEnhancement> enhancements)
		{
			foreach (var enhancement in enhancements)
			{
				enhancement.Apply(this);
			}
		}
	}
}
