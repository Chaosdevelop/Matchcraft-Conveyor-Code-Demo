using UnityEngine;
using Zenject;
public class GameInstaller : MonoInstaller
{
	[SerializeField]
	AudioManager audioManager;
	[SerializeField]
	UpgradesWindow upgradesWindow;
	[SerializeField]
	ShipAssemblyWindow shipAssemblyWindow;


	public override void InstallBindings()
	{
		Container.Bind<PlayerProgress>().AsSingle();
		Container.Bind<GameManager>().AsSingle();
		Container.Bind<AudioManager>().FromComponentInNewPrefab(audioManager).AsSingle();
		Container.Bind<UpgradesWindow>().FromComponentsOn(upgradesWindow.gameObject).AsSingle();
		Container.Bind<ShipAssemblyWindow>().FromComponentsOn(shipAssemblyWindow.gameObject).AsSingle();
	}
}
