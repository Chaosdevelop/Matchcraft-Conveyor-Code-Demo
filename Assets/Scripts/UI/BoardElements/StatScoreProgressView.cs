using BaseCore;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3Game
{
	/// <summary>
	/// Event struct representing a change in stat scores.
	/// </summary>
	public struct StatScoresChanged
	{
		public ItemStat statType;
		public int Delta;
		public int NewValue;
	}

	/// <summary>
	/// View class for displaying stat score progress in the Match3 game.
	/// </summary>
	public class StatScoreProgressView : MonoBehaviour, IEventSubscriber<StatScoresChanged>, IStartUpMonoBehaviourSubscriber, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		Image progressLvlBar;

		[SerializeField]
		TextMeshProUGUI level;

		[SerializeField]
		TextMeshProUGUI scores;

		[SerializeField]
		ItemStat statType;

		[SerializeField]
		AudioSource loopSound;

		StatScoreProgress realscores;
		StatScoreProgress animatedscores;
		float animScorePerSecond = 25;

		Color mainBarColor;
		int prevLvl;

		Tweener progressTweener;

		/// <summary>
		/// Called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			mainBarColor = progressLvlBar.color;
		}

		/// <summary>
		/// Called when the object becomes enabled and active.
		/// </summary>
		void OnEnable()
		{
			Init();
		}

		/// <summary>
		/// Initializes the stat score progress view.
		/// </summary>
		public void Init()
		{
			realscores = new StatScoreProgress(statType);
			animatedscores = new StatScoreProgress(statType);
			prevLvl = animatedscores.Level;
			SetStates();
		}

		/// <summary>
		/// Adds points to the stat score.
		/// </summary>
		/// <param name="score">The score to add.</param>
		public void AddPoints(int score)
		{
			realscores.AddScore(score);
			loopSound.enabled = true;
			loopSound.Play();
		}

		/// <summary>
		/// Sets the state of the score progress view.
		/// </summary>
		void SetStates()
		{
			var scoresCurrent = (int)animatedscores.Scores;
			var scoresNeed = animatedscores.ScoreToNextLevel;
			int currentLvl = animatedscores.Level;
			if (prevLvl < currentLvl)
			{
				// Handle level up visual feedback if needed
			}
			prevLvl = currentLvl;
			progressLvlBar.fillAmount = Mathf.InverseLerp(0, scoresNeed, scoresCurrent);
			scores.text = string.Format("{0}/{1}", scoresCurrent, scoresNeed);
			level.text = currentLvl.ToString();
		}

		/// <summary>
		/// Called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		void Update()
		{
			if (animatedscores.TotalScore < realscores.TotalScore)
			{
				var delta = realscores.TotalScore - animatedscores.TotalScore;
				var deltaNow = Mathf.Min(delta, animScorePerSecond * Time.deltaTime);
				animatedscores.AddScore(deltaNow);
				SetStates();
				var animColor = Color.Lerp(mainBarColor, Color.white, delta / animScorePerSecond);
				progressLvlBar.color = animColor;
			}
			else
			{
				if (loopSound.enabled)
				{
					loopSound.Stop();
					loopSound.enabled = false;
				}
			}
		}

		/// <summary>
		/// Receives and handles the <see cref="StatScoresChanged"/> event.
		/// </summary>
		/// <param name="data">The stat score change data.</param>
		void IEventSubscriber<StatScoresChanged>.ReceiveEvent(StatScoresChanged data)
		{
			if (data.statType == statType)
			{
				AddPoints(data.Delta);
			}
		}

		/// <summary>
		/// Called when the object is being destroyed.
		/// </summary>
		void OnDestroy()
		{
			this.SelfUnsubscribe();
		}

		/// <summary>
		/// Called when the pointer enters the object.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			// Handle pointer enter event
		}

		/// <summary>
		/// Called when the pointer exits the object.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			// Handle pointer exit event
		}
	}
}
