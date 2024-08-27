using System.IO;
using UnityEngine;

/// <summary>
/// Manages saving and loading game data.
/// </summary>
public class SaveManager
{
	public static string PersonalPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "GameData";

	/// <summary>
	/// Saves the player's progress to a file.
	/// </summary>
	/// <param name="playerProgress">The player progress data to save.</param>
	public static void Save(PlayerProgress playerProgress)
	{
		try
		{
			string json = JsonUtility.ToJson(playerProgress, true);
			SaveData("Game.save", json);
		}
		catch (System.Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	/// <summary>
	/// Loads the player's progress from a file.
	/// </summary>
	/// <param name="playerProgress">The player progress data to load into.</param>
	/// <returns>True if loading was successful, false otherwise.</returns>
	public static bool Load(PlayerProgress playerProgress)
	{
		string json = LoadData("Game.save");
		if (string.IsNullOrEmpty(json))
		{
			return false;
		}

		try
		{
			JsonUtility.FromJsonOverwrite(json, playerProgress);
			return true;
		}
		catch (System.Exception ex)
		{
			Debug.LogError(ex);
			return false;
		}
	}

	/// <summary>
	/// Loads data from the specified path.
	/// </summary>
	/// <param name="path">The relative path to load data from.</param>
	/// <returns>The loaded data as a string.</returns>
	static string LoadData(string path)
	{
		string finalPath = Path.Combine(PersonalPath, path);
		if (File.Exists(finalPath))
		{
			return File.ReadAllText(finalPath);
		}
		return string.Empty;
	}

	/// <summary>
	/// Resets the saved game data by deleting all files in the save directory.
	/// </summary>
	public static void ResetData()
	{
		if (Directory.Exists(PersonalPath))
		{
			var files = Directory.GetFiles(PersonalPath);
			foreach (string filepath in files)
			{
				File.Delete(filepath);
			}
		}
	}

	/// <summary>
	/// Saves data to the specified path.
	/// </summary>
	/// <param name="path">The relative path to save data to.</param>
	/// <param name="data">The data to save.</param>
	/// <returns>True if saving was successful, false otherwise.</returns>
	static bool SaveData(string path, string data)
	{
		string finalPath = Path.Combine(PersonalPath, path);
		if (!Directory.Exists(PersonalPath))
		{
			Directory.CreateDirectory(PersonalPath);
		}
		File.WriteAllText(finalPath, data);
		return true;
	}
}
