using System;
using BaseCore.Collections;
using Skills;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SkillsInstaller", menuName = "ScriptableInstallers/SkillsInstaller", order = 1)]
public class SkillsInstaller : ScriptableObjectInstaller<SkillsInstaller>
{
	[SerializeField]
	GameSkills Skills;

	[Serializable]
	public class GameSkills
	{
		[field: SerializeField]
		public EnumDictionary<SkillSlot, SkillData> SkillsData { get; private set; }

	}

	public override void InstallBindings()
	{
		Container.BindInstance(Skills).IfNotBound();

	}
}
