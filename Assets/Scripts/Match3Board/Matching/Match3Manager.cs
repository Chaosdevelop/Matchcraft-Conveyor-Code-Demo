using BaseCore;
using BaseCore.Collections;
using Skills;
using UI;
using UnityEngine;
using Zenject;

namespace Match3Game
{
	/// <summary>
	/// Manages the Match3 game, including grid and skills.
	/// </summary>
	public class Match3Manager : MonoBehaviour
	{
		[Inject]
		ISkillViewFactory skillViewFactory;

		[Inject]
		GridController gridController;

		[Inject]
		GameManager gameManager;

		readonly EnumDictionary<SkillSlot, SkillView> skillsViews = new();
		readonly EnumDictionary<ItemStat, StatScoreProgress> statScores = new();

		int totalScore;
		int turnsMade;
		int turnsLeft;

		ShipPartAssemblyResult results;


		void Start()
		{
			Initialize();
		}
		/// <summary>
		/// Initializes the Match3Manager.
		/// </summary>
		void Initialize()
		{
			gridController.OnMoveDone += OnMoveDone;
			gridController.OnMatchProcess += ProcessMatch;
			gridController.CascadeAnimated += WaitCascadeAnimation;
		}

		void CreateSkills()
		{
			foreach (var (key, skillModel) in gameManager.Skills)
			{
				skillModel.ResetEnergy();

				var skillView = skillViewFactory.CreateSkillView(key, skillModel);
				skillView.SetAvailability(skillModel.SkillAvailable);
				skillsViews[key] = skillView;
			}
		}

		void DestroySkills()
		{
			foreach (var (key, skillView) in skillsViews)
			{
				Destroy(skillView.gameObject);
			}

		}

		void OnEnable()
		{
			gridController.ReinitializeGrid();
			PrepareGame();
		}

		/// <summary>
		/// Handles the event when a move is done.
		/// </summary>
		void OnMoveDone()
		{
			turnsMade++;
			turnsLeft--;
			EventSystem.SendEventToAll(new TurnsChanged { NewValue = turnsLeft, Delta = -1 });
		}

		/// <summary>
		/// Waits for the cascade animation to complete.
		/// </summary>
		void WaitCascadeAnimation()
		{
			if (turnsLeft == 0)
			{
				EndCraftGame();
			}
		}

		/// <summary>
		/// Prepares the game for a new craft.
		/// </summary>
		void PrepareGame()
		{
			totalScore = 0;
			turnsMade = 0;
			turnsLeft = gameManager.TurnsPerCraft;

			statScores[ItemStat.Durability] = new StatScoreProgress(ItemStat.Durability);
			statScores[ItemStat.Quality] = new StatScoreProgress(ItemStat.Quality);
			statScores[ItemStat.Design] = new StatScoreProgress(ItemStat.Design);

			CreateSkills();

			EventSystem.SendEventToAll(new TurnsChanged { NewValue = turnsLeft });
			results = gameManager.GetCurrentCraftingPart();
		}

		/// <summary>
		/// Ends the current craft.
		/// </summary>
		void EndCraftGame()
		{
			foreach (var (key, value) in statScores)
			{
				results.Stats[key] = value.Level;
			}

			results.Scores = totalScore;
			results.Done = true;
			gameManager.NextShipType();
			EventSystem.SendEventToAll(new ScreenChangeEvent { ScreenType = ScreenType.Main });
			DestroySkills();
		}

		/// <summary>
		/// Processes a match and updates scores and stats.
		/// </summary>
		/// <param name="matchInfo">The match information.</param>
		public void ProcessMatch(MatchInfo matchInfo)
		{
			if (matchInfo == null)
			{
				return;
			}

			int count = matchInfo.MatchCount;
			ChipType type = matchInfo.MatchType;
			int scores = gameManager.ScoresPerMatch * count;
			totalScore += scores;

			EventSystem.SendEventToAll(new ScoresChanged { NewValue = totalScore, Delta = scores });

			switch (type)
			{
				case ChipType.Blue:
					statScores[ItemStat.Durability].AddScore(scores);
					EventSystem.SendEventToAll(new StatScoresChanged { statType = ItemStat.Durability, Delta = scores });
					break;

				case ChipType.Red:
					statScores[ItemStat.Quality].AddScore(scores);
					EventSystem.SendEventToAll(new StatScoresChanged { statType = ItemStat.Quality, Delta = scores });
					break;

				case ChipType.Green:
					statScores[ItemStat.Design].AddScore(scores);
					EventSystem.SendEventToAll(new StatScoresChanged { statType = ItemStat.Design, Delta = scores });
					break;

				case ChipType.Purple:
					gameManager.Skills[SkillSlot.Purple].AddEnergy(scores);
					break;

				case ChipType.Yellow:
					gameManager.Skills[SkillSlot.Yellow].AddEnergy(scores);
					break;

				case ChipType.Orange:
					gameManager.Skills[SkillSlot.Orange].AddEnergy(scores);
					break;
			}
		}
	}
}