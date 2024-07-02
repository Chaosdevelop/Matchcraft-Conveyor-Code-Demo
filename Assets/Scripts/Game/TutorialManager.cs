using BaseCore.Collections;
using UnityEngine;

/// <summary>
/// Dummy for tutorial manager.
/// </summary>
public class TutorialManager : MonoBehaviour
{
	[field: SerializeField]
	public EnumDictionary<ShipPartType, ShipPartInfo> StartingParts { get; private set; }

}
