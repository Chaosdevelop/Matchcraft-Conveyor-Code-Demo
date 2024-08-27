using BaseCore.Collections;
using Match3Game;
using UI;
using UnityEngine;
using Zenject;


public class MatchGameInstaller : MonoInstaller
{
	[SerializeField]
	GridController gridController;
	[SerializeField]
	Match3Manager match3Manager;
	[SerializeField]
	Cell cellPrefab;
	[SerializeField]
	Transform cellsContainer;
	[SerializeField]
	EnumDictionary<ChipType, Chip> chipsDictionary;
	[SerializeField]
	Transform chipsContainer;
	[SerializeField]
	SkillView skillViewPrefab;
	[SerializeField]
	Transform skillsContainer;

	public override void InstallBindings()
	{
		Container.Bind<ICellFactory>().To<CellFactory>().AsSingle().WithArguments(cellPrefab, cellsContainer);
		Container.Bind<IChipFactory>().To<ChipFactory>().AsSingle().WithArguments(chipsDictionary, chipsContainer);
		Container.Bind<ISkillViewFactory>().To<SkillViewFactory>().AsSingle().WithArguments(skillViewPrefab, skillsContainer);

		Container.Bind<GridController>().FromComponentsOn(gridController.gameObject).AsSingle();
		Container.Bind<Match3Manager>().FromComponentsOn(match3Manager.gameObject).AsSingle();

		Container.Bind<IMatchChecker>().To<MatchChecker>().AsSingle();

	}
}
