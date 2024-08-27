using System;

namespace Match3Game
{
	/// <summary>
	/// Represents the progress of a specific stat score in the Match3 game.
	/// </summary>
	public class StatScoreProgress
	{
		/// <summary>
		/// Gets the stat being tracked.
		/// </summary>
		public ItemStat Stat { get; }

		/// <summary>
		/// Gets the total score accumulated.
		/// </summary>
		public float TotalScore { get; private set; }

		/// <summary>
		/// Gets the current score within the current level.
		/// </summary>
		public float Scores { get; private set; }

		/// <summary>
		/// Gets the score required to reach the next level.
		/// </summary>
		public int ScoreToNextLevel { get; private set; } = START_SCORES + SCORES_PROGRESSION;

		/// <summary>
		/// Gets the current level.
		/// </summary>
		public int Level { get; private set; }

		/// <summary>
		/// The amount by which the score to the next level increases with each level.
		/// </summary>
		public const int SCORES_PROGRESSION = 5;

		/// <summary>
		/// The initial score required to reach the first level.
		/// </summary>
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
			return MemberwiseClone() as StatScoreProgress;
		}
	}

	/// <summary>
	/// Represents the event data for when a stat level is reached.
	/// </summary>
	[Serializable]
	public struct StatLevelReached
	{
		/// <summary>
		/// The stat that reached a new level.
		/// </summary>
		public ItemStat Stat { get; set; }

		/// <summary>
		/// The new level reached.
		/// </summary>
		public int Level { get; set; }
	}
}