using UnityEngine;

namespace BaseCore
{
	/// <summary>
	/// Utility class for text-related extensions and transformations.
	/// </summary>
	public static class TextTools
	{
		/// <summary>
		/// Counts the number of lines in a string.
		/// </summary>
		/// <param name="s">The string to count lines in.</param>
		/// <returns>The number of lines in the string.</returns>
		public static int LinesCount(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return 0;
			}

			int count = 1;
			int position = 0;
			while ((position = s.IndexOf('\n', position)) != -1)
			{
				count++;
				position++;
			}
			return count;
		}

		/// <summary>
		/// Checks if a string is null or empty.
		/// </summary>
		/// <param name="s">The string to check.</param>
		/// <returns>True if the string is null or empty, otherwise false.</returns>
		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}

		/// <summary>
		/// Converts a string to sentence case.
		/// </summary>
		/// <param name="s">The string to convert.</param>
		/// <returns>The converted string in sentence case.</returns>
		public static string ToSentenceCase(this string s)
		{
			string str = System.Text.RegularExpressions.Regex.Replace(s, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());
			str = char.ToUpperInvariant(str[0]) + str.Substring(1);
			return str;
		}

		/// <summary>
		/// Converts a string to Pascal case.
		/// </summary>
		/// <param name="s">The string to convert.</param>
		/// <returns>The converted string in Pascal case.</returns>
		public static string ToPascalCase(this string s)
		{
			string str = System.Text.RegularExpressions.Regex.Replace(s, "(\\B[A-Z])", " $1");
			str = char.ToUpperInvariant(str[0]) + str.Substring(1);
			return str;
		}

		/// <summary>
		/// Applies rich text formatting to a string.
		/// </summary>
		/// <param name="s">The string to format.</param>
		/// <param name="color">The color to apply.</param>
		/// <param name="bold">Whether to make the text bold.</param>
		/// <param name="italics">Whether to make the text italic.</param>
		/// <param name="size">The size of the text.</param>
		/// <returns>The formatted string.</returns>
		public static string RichText(this string s, Color color, bool bold = false, bool italics = false, int size = 11)
		{
			string str = s;
			str = string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), str);

			if (bold)
			{
				str = string.Format("<b>{0}</b>", str);
			}
			if (italics)
			{
				str = string.Format("<i>{0}</i>", str);
			}
			if (size != 11)
			{
				str = string.Format("<size={0}>{1}</size>", size, str);
			}
			return str;
		}
	}
}
