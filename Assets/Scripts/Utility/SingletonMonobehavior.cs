using System.Linq;
using UnityEngine;

/// <summary>
/// Abstract base class for creating singleton MonoBehaviour instances.
/// </summary>
/// <typeparam name="T">The type of the singleton instance.</typeparam>
public abstract class SingletonMonobehavior<T> : MonoBehaviour where T : SingletonMonobehavior<T>
{
	static T instance;

	/// <summary>
	/// Singleton instance.
	/// </summary>
	public static T Instance {
		get {
			if (instance == null)
			{
				instance = FindObjectOfType<T>(true);

				if (instance == null)
				{
					instance = Resources.LoadAll<T>("").FirstOrDefault();
					instance = Instantiate(instance);

					if (instance == null)
					{
						var go = new GameObject(typeof(T).Name, typeof(T));
						instance = go.GetComponent<T>();
						Debug.LogError("New instance created: " + go);
					}
					else
					{
						Resources.UnloadUnusedAssets();
					}

					DontDestroyOnLoad(instance.gameObject);
				}

				if (!instance.gameObject.activeSelf && Application.isPlaying)
				{
					instance.gameObject.SetActive(true);
					instance.gameObject.SetActive(false);
				}
			}

			return instance;
		}
		protected set {
			instance = value;
		}
	}
}
