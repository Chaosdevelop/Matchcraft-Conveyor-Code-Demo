using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3Game
{
	/// <summary>
	/// Represents match information in the Match3 game.
	/// </summary>
	public class MatchInfo
	{
		public int MatchCount => MatchedChips.Count;

		public List<IChip> MatchedChips { get; private set; }
		public ChipType MatchType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MatchInfo"/> class.
		/// </summary>
		/// <param name="matchType">The type of the matched chips.</param>
		public MatchInfo(ChipType matchType)
		{
			MatchedChips = new List<IChip>();
			MatchType = matchType;
		}

		/// <summary>
		/// Adds a chip to the match.
		/// </summary>
		/// <param name="chip">The chip to add.</param>
		/// <exception cref="ArgumentNullException">Thrown when the chip is null.</exception>
		public void AddChip(IChip chip)
		{
			if (chip == null)
			{
				throw new ArgumentNullException(nameof(chip));
			}
			MatchedChips.Add(chip);
		}

		/// <summary>
		/// Finds the intersection of matched chips with another match.
		/// </summary>
		/// <param name="other">The other match to find the intersection with.</param>
		/// <returns>A list of chips that are in both matches.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the other match is null.</exception>
		public List<IChip> FindIntersection(MatchInfo other)
		{
			if (other == null)
			{
				throw new ArgumentNullException(nameof(other));
			}
			return MatchedChips.Intersect(other.MatchedChips).ToList();
		}

		/// <summary>
		/// Merges this match with another match.
		/// </summary>
		/// <param name="other">The other match to merge with.</param>
		/// <exception cref="ArgumentNullException">Thrown when the other match is null.</exception>
		/// <exception cref="ArgumentException">Thrown when the match types are different.</exception>
		public void MergeWith(MatchInfo other)
		{
			if (other == null)
			{
				throw new ArgumentNullException(nameof(other));
			}
			if (MatchType != other.MatchType)
			{
				throw new ArgumentException("Cannot merge MatchInfo objects with different MatchTypes.");
			}
			foreach (var chip in other.MatchedChips)
			{
				if (!MatchedChips.Contains(chip))
				{
					MatchedChips.Add(chip);
				}
			}
		}
	}
}
