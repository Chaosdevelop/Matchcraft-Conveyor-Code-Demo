using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	/// <summary>
	/// Manages the display and updates for skill cost indicators in the UI.
	/// </summary>
	public class SkillCostIndicator : MonoBehaviour
	{
		[SerializeField]
		Image progressBar;

		[SerializeField]
		TextMeshProUGUI chargesText;

		Skills.SkillCost skillCost;

		/// <summary>
		/// Initializes the skill cost indicator with the specified skill cost.
		/// </summary>
		/// <param name="cost">The skill cost to use.</param>
		public void Initialize(Skills.SkillCost cost)
		{
			skillCost = cost;
			skillCost.OnChargesChanged += UpdateCharges;
			skillCost.OnEnergyChanged += UpdateEnergy;
			UpdateCharges();
			UpdateEnergy();
		}

		/// <summary>
		/// Updates the displayed charge count.
		/// </summary>
		void UpdateCharges()
		{
			chargesText.text = $"{skillCost.CurrentCharges}";
		}

		/// <summary>
		/// Updates the displayed energy progress.
		/// </summary>
		void UpdateEnergy()
		{
			float progress = (float)skillCost.CurrentEnergy / skillCost.MaxEnergy;
			progressBar.fillAmount = progress;
			progressBar.color = skillCost.CurrentCharges > 0 ? Color.green : Color.red;
		}

		/// <summary>
		/// Called when the object is being destroyed.
		/// </summary>
		void OnDestroy()
		{
			if (skillCost != null)
			{
				skillCost.OnChargesChanged -= UpdateCharges;
				skillCost.OnEnergyChanged -= UpdateEnergy;
			}
		}
	}
}
