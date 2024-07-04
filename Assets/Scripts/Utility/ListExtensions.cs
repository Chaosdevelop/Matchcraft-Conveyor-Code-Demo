using System.Collections.Generic;

namespace BaseCore.Collections
{
	/// <summary>
	/// Extension methods for IList.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Removes all null values from the list.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the list.</typeparam>
		/// <param name="source">The list to remove null values from.</param>
		public static void RemoveNullValues<T>(this IList<T> source) where T : class
		{
			while (source.Remove(null)) { }
		}

		/// <summary>
		/// Picks a random element from the list.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the list.</typeparam>
		/// <param name="source">The list to pick a random element from.</param>
		/// <param name="rnd">An optional random number generator.</param>
		/// <returns>A randomly selected element from the list.</returns>
		public static T PickRandom<T>(this IList<T> source, System.Random rnd = null)
		{
			if (source.Count == 0)
				return default(T);

			int i = rnd?.Next(0, source.Count) ?? BaseCore.Randoms.Range(0, source.Count);

			return source[i];
		}

		/// <summary>
		/// Removes and returns a random element from the list.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the list.</typeparam>
		/// <param name="source">The list to remove a random element from.</param>
		/// <param name="rnd">An optional random number generator.</param>
		/// <returns>The randomly removed element from the list.</returns>
		public static T PopRandom<T>(this IList<T> source, System.Random rnd = null)
		{
			if (source.Count == 0)
				return default(T);

			int i = rnd?.Next(0, source.Count) ?? BaseCore.Randoms.Range(0, source.Count);
			T value = source[i];
			source.RemoveAt(i);

			return value;
		}

		/// <summary>
		/// Shuffles the elements in the list.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the list.</typeparam>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="rnd">An optional random number generator.</param>
		public static void Shuffle<T>(this IList<T> list, System.Random rnd = null)
		{
			int index = list.Count;
			while (index > 1)
			{
				index--;
				int newPos = rnd?.Next(0, index + 1) ?? BaseCore.Randoms.Range(0, index + 1);

				(list[newPos], list[index]) = (list[index], list[newPos]);
			}
		}
	}
}
