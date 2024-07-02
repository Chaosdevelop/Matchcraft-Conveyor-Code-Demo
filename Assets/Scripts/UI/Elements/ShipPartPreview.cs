using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the preview display for a ship part in the UI.
/// </summary>
public class ShipPartPreview : MonoBehaviour
{
	[SerializeField]
	Image image;

	[SerializeField]
	TextMeshProUGUI partLabelText;

	ShipPartInfo shipPartInfo;


	/// <summary>
	/// Sets the information for the ship part preview.
	/// </summary>
	/// <param name="shipPartInfo">The ship part information to display.</param>
	public void SetInfo(ShipPartInfo shipPartInfo)
	{
		this.shipPartInfo = shipPartInfo;
		image.sprite = shipPartInfo.Icon;
		partLabelText.text = shipPartInfo.PartName.GetLocalizedString();
	}
}
