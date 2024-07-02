using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BaseCore
{
	/// <summary>
	/// Extension methods for various utility functions.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Converts an object to a detailed string representation, including its fields and properties.
		/// </summary>
		/// <param name="obj">The object to convert to a string.</param>
		/// <returns>A string representation of the object's fields and properties.</returns>
		public static string ToDetailedString(this object obj)
		{
			if (obj == null)
			{
				return "null";
			}

			Type type = obj.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			StringBuilder sb = new StringBuilder();
			sb.Append($"{type.Name} (");

			bool first = true;

			foreach (var field in fields)
			{
				if (!first)
				{
					sb.Append(", ");
				}
				sb.Append($"{field.Name} = {field.GetValue(obj) ?? "null"}");
				first = false;
			}

			foreach (var property in properties.Where(p => p.CanRead))
			{
				if (!first)
				{
					sb.Append(", ");
				}
				sb.Append($"{property.Name} = {property.GetValue(obj, null) ?? "null"}");
				first = false;
			}

			sb.Append(")");
			return sb.ToString();
		}
	}
}
