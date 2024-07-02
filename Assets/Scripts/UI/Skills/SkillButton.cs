using Skills;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	/// <summary>
	/// Manages the functionality and display of a skill button in the UI.
	/// </summary>
	public class SkillButton : MonoBehaviour
	{
		[SerializeField]
		Button button;

		[SerializeField]
		Image buttonImage;

		[SerializeField]
		GameObject targetingStateView;

		SkillView skillView;
		bool isPressed;

		/// <summary>
		/// Initializes the skill button with the specified skill model and view.
		/// </summary>
		/// <param name="model">The skill model to use.</param>
		/// <param name="skillView">The skill view to use.</param>
		public void Initialize(SkillModel model, SkillView skillView)
		{
			this.skillView = skillView;
			buttonImage.sprite = model.Icon;
			button.onClick.AddListener(OnButtonClick);
			skillView.OnTargetingStateChanged += TargetingStateUpdate;
		}

		/// <summary>
		/// Updates the targeting state view based on the specified active state.
		/// </summary>
		/// <param name="active">If set to <c>true</c> activates the targeting state view.</param>
		void TargetingStateUpdate(bool active)
		{
			targetingStateView.SetActive(active);
		}

		/// <summary>
		/// Handles the button click event.
		/// </summary>
		void OnButtonClick()
		{
			isPressed = !isPressed;

			if (isPressed)
			{
				skillView.ActivateTargeting();
			}
			else
			{
				skillView.DeactivateTargeting();
			}
		}
	}
}
