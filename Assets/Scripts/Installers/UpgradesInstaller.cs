using System;
using UnityEngine;
using Upgrades;
using Zenject;

[CreateAssetMenu(fileName = "UpgradesInstaller", menuName = "ScriptableInstallers/UpgradesInstaller", order = 1)]
public class UpgradesInstaller : ScriptableObjectInstaller<UpgradesInstaller>
{
	[SerializeField]
	UpgradesArray upgrades;

	[Serializable]
	public class UpgradesArray
	{
		[field: SerializeField]
		public UpgradeGroupData[] AllUpgrades { get; private set; }

	}
	public override void InstallBindings()
	{
		Container.BindInstance(upgrades).IfNotBound();
	}
}
