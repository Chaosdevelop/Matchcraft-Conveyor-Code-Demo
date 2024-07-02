using BaseCore.Collections;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manages audio playback for the game.
/// </summary>
public class AudioManager : SingletonMonobehavior<AudioManager>
{
	[SerializeField]
	AudioSource audioSource;

	[SerializeField]
	AudioSource musicSource;

	[SerializeField]
	AudioClip[] musicTracks;

	[SerializeField]
	AudioMixer audioMixer;

	[SerializeField]
	float delayBetweenTracks;

	const float FADE_TIME = 2;
	float timerBetweenTracks;
	bool paused = false;
	AudioClip[] currentTracks;

	/// <summary>
	/// Gets the paused state.
	/// </summary>
	public bool Paused => paused;

	/// <summary>
	/// Gets or sets the music volume.
	/// </summary>
	public float MusicVolume {
		get => PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		set => SetVolume("MusicVolume", value);
	}

	/// <summary>
	/// Gets or sets the sounds volume.
	/// </summary>
	public float SoundsVolume {
		get => PlayerPrefs.GetFloat("SoundsVolume", 0.5f);
		set => SetVolume("SoundsVolume", value);
	}

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		SetupAudio();
		PlayCurrentTracks();
	}

	/// <summary>
	/// Pauses all audio playback.
	/// </summary>
	public void PauseAll()
	{
		musicSource.Pause();
		paused = true;
	}

	/// <summary>
	/// Resumes all audio playback.
	/// </summary>
	public void UnPauseAll()
	{
		musicSource.UnPause();
		paused = false;
	}

	void SetupAudio()
	{
		SetVolume("GeneralVolume", 1);
		SetVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0.5f));
		SetVolume("SoundsVolume", PlayerPrefs.GetFloat("SoundsVolume", 0.5f));
	}

	void SetVolume(string group, float value)
	{
		value = Mathf.Clamp(value, 0.001f, 1);
		var logValue = Mathf.Clamp(Mathf.Log10(value) * 26, -80, 0);
		audioMixer.SetFloat(group, logValue);
		PlayerPrefs.SetFloat(group, value);
	}

	/// <summary>
	/// Plays the specified audio clip.
	/// </summary>
	/// <param name="clip">The audio clip to play.</param>
	public void PlaySound(AudioClip clip)
	{
		if (paused) return;
		audioSource.PlayOneShot(clip);
	}

	/// <summary>
	/// Gets the master volume.
	/// </summary>
	/// <returns>The master volume.</returns>
	public float GetMasterVolume()
	{
		return PlayerPrefs.GetFloat("GeneralVolume", 0.5f);
	}

	/// <summary>
	/// Plays the current set of music tracks.
	/// </summary>
	void PlayCurrentTracks()
	{
		var prevTracks = currentTracks;
		bool tracksChanged = currentTracks != prevTracks && prevTracks != null;

		if (tracksChanged)
		{
			StartCoroutine(FadeAndChangeTracks());
		}
		else
		{
			if (currentTracks != null && currentTracks.Length > 0 && (musicSource.clip == null || musicSource.time < musicSource.clip.length))
			{
				SetMusicClip(currentTracks.PickRandom());
			}
		}
	}

	void SetMusicClip(AudioClip newClip)
	{
		musicSource.Stop();
		musicSource.clip = newClip;
		musicSource.Play();
		if (paused) musicSource.Pause();
		timerBetweenTracks = 0;
	}

	void Update()
	{
		if (!musicSource.isPlaying && !paused && Application.isFocused)
		{
			if (timerBetweenTracks < delayBetweenTracks)
			{
				timerBetweenTracks += Time.deltaTime;
			}
			else
			{
				PlayCurrentTracks();
			}
		}
	}

	IEnumerator FadeAndChangeTracks()
	{
		float timer = FADE_TIME;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			musicSource.volume = timer / FADE_TIME;
			yield return new WaitForEndOfFrame();
		}

		PlayCurrentTracks();
		timer = FADE_TIME;
		while (timer > 0)
		{
			timer -= Time.deltaTime;
			musicSource.volume = 1 - timer / FADE_TIME;
			yield return new WaitForEndOfFrame();
		}

		musicSource.volume = 1;
	}
}
