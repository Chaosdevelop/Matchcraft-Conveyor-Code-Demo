using System;

namespace Match3Game
{
	/// <summary>
	/// Represents the progress of a specific stat score in the Match3 game.
	/// </summary>
	public class StatScoreProgress
	{
		public ItemStat Stat { get; private set; }
		public float TotalScore { get; private set; }
		public float Scores { get; private set; }
		public int ScoreToNextLevel { get; private set; } = START_SCORES + SCORES_PROGRESSION;
		public int Level { get; private set; }

		public const int SCORES_PROGRESSION = 5;
		public const int START_SCORES = 45;

		/// <summary>
		/// Initializes a new instance of the <see cref="StatScoreProgress"/> class.
		/// </summary>
		/// <param name="stat">The stat to track.</param>
		public StatScoreProgress(ItemStat stat)
		{
			Stat = stat;
		}

		/// <summary>
		/// Adds a score value to the current score and handles leveling up.
		/// </summary>
		/// <param name="value">The score value to add.</param>
		public void AddScore(float value)
		{
			TotalScore += value;
			Scores += value;

			while (Scores >= ScoreToNextLevel)
			{
				Scores -= ScoreToNextLevel;
				Level++;
				ScoreToNextLevel += SCORES_PROGRESSION;

			}
		}

		/// <summary>
		/// Creates a copy of the current <see cref="StatScoreProgress"/> instance.
		/// </summary>
		/// <returns>A copy of the current instance.</returns>
		public StatScoreProgress Copy()
		{
			return this.MemberwiseClone() as StatScoreProgress;
		}
	}

	/// <summary>
	/// Represents the event data for when a stat level is reached.
	/// </summary>
	[Serializable]
	public struct StatLevelReached
	{
		public ItemStat Stat { get; set; }
		public int Level { get; set; }
	}

}
