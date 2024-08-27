using System;
using BaseCore.Collections;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ShipPartsInstaller", menuName = "ScriptableInstallers/ShipPartsInstaller", order = 1)]
public class ShipPartsInstaller : ScriptableObjectInstaller<ShipPartsInstaller>
{
	[SerializeField]
	PredefinedParts StartingParts;
	[SerializeField]
	ShipParts AllParts;

	[Serializable]
	public class PredefinedParts
	{
		[field: SerializeField]
		public EnumDictionary<ShipPartType, ShipPartInfo> Parts { get; private set; }

	}

	[Serializable]
	public class ShipParts
	{
		[field: SerializeField]
		public ShipPartInfo[] Parts { get; private set; }

	}

	public override void InstallBindings()
	{
		Container.BindInstance(StartingParts).IfNotBound();
		Container.BindInstance(AllParts).IfNotBound();
	}
}
