using BaseCore.Collections;
using Match3Game;
using Skills;
using UnityEngine;

namespace UI
{


	/// <summary>
	/// Manages the view and interactions for a skill in the game.
	/// </summary>
	public class SkillView : MonoBehaviour
	{
		[SerializeField]
		SkillButton skillButton;

		[SerializeField]
		SkillCostIndicator skillCostIndicator;

		[SerializeField]
		EnumDictionary<SkillSlot, GameObject> chipTypeIcons = new EnumDictionary<SkillSlot, GameObject>();

		SkillModel skillModel;
		GridController gridController;

		/// <summary>
		/// Event invoked when the targeting state changes.
		/// </summary>
		public System.Action<bool> OnTargetingStateChanged { get; set; }


		/// <summary>
		/// Initializes the skill view with the specified model.
		/// </summary>
		/// <param name="model">The skill model to initialize.</param>
		public void Initialize(SkillModel model, GridController gridController)
		{
			this.gridController = gridController;
			skillModel = model;
			skillButton.Initialize(model, this);
			skillCostIndicator.Initialize(model.SkillCost);
			foreach (var item in chipTypeIcons)
			{
				item.Value.SetActive(model.SkillSlot == item.Key);
			}
		}

		/// <summary>
		/// Activates the targeting mode for the skill.
		/// </summary>
		public void ActivateTargeting()
		{
			if (skillModel.IsActiveTargeting)
			{
				DeactivateTargeting();
				return;
			}

			OnTargetingStateChanged?.Invoke(true);
			skillModel.ActivateTargeting();
			gridController.OnCellClicked += HandleCellClicked;
			gridController.ToggleTargetingSkill(true);
			gridController.SetTargetingPattern(skillModel.Pattern);
		}

		/// <summary>
		/// Deactivates the targeting mode for the skill.
		/// </summary>
		public void DeactivateTargeting()
		{
			skillModel.DeactivateTargeting();
			gridController.OnCellClicked -= HandleCellClicked;
			OnTargetingStateChanged?.Invoke(false);
			gridController.ToggleTargetingSkill(false);
			gridController.SetTargetingPattern(null);
		}

		/// <summary>
		/// Handles the cell click event when the skill is targeting.
		/// </summary>
		/// <param name="cellPosition">The position of the clicked cell.</param>
		void HandleCellClicked(Vector2Int cellPosition)
		{
			if (!skillModel.IsActiveTargeting || !skillModel.CanUseSkill()) return;

			skillModel.ApplySkill(gridController, cellPosition, gridController.Rows, gridController.Columns);
			skillModel.SpendCharge();
			DeactivateTargeting();
		}

		/// <summary>
		/// Sets the availability of the skill view.
		/// </summary>
		/// <param name="available">True if the skill view should be available, otherwise false.</param>
		public void SetAvailability(bool available)
		{
			gameObject.SetActive(available);
		}
	}
}
