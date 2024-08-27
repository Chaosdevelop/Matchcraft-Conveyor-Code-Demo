using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "ScriptableInstallers/GameSettingsInstaller", order = 1)]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
	[SerializeField]
	PlayerValues Values;

	[Serializable]
	public class PlayerValues
	{
		[field: SerializeField]
		public int TurnsForCraft { get; private set; }
		[field: SerializeField]
		public int ScoresPerMatch { get; private set; }
		[field: SerializeField]
		public int StartCoins { get; private set; }
	}


	public override void InstallBindings()
	{
		Container.BindInstance(Values).IfNotBound();
	}
}
