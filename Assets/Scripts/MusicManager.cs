using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource audioSource;  // Main AudioSource for background music
    public AudioSource crossfadeSource; // Secondary AudioSource for crossfade

    public AudioClip currentMusicClip;
    private bool isCrossfading = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep MusicManager alive across scenes
            Debug.Log("MusicManager Initialized and set to persist.");
        }
        else
        {
            Debug.LogWarning("A duplicate MusicManager was detected and destroyed.");
            Destroy(gameObject);
        }

        // Ensure AudioSources exist
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        if (crossfadeSource == null) crossfadeSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        crossfadeSource.loop = true;
    }

    public void PlayMusic(AudioClip newClip, float fadeDuration = 1.0f, bool forceRestart = false)
    {
        if (newClip == null)
        {
            Debug.LogError("MusicManager: Received null AudioClip!");
            return;
        }

        Debug.Log("Trying to play music: " + newClip.name);

        // Prevent restarting the same track unless forced
        if (!forceRestart && currentMusicClip == newClip && audioSource.isPlaying)
        {
            Debug.Log("MusicManager: Same music is already playing, skipping.");
            return;
        }

        if (isCrossfading)
        {
            Debug.Log("MusicManager: Crossfade in progress, queuing track.");
            return;
        }

        Debug.Log("MusicManager: Starting new music: " + newClip.name);
        currentMusicClip = newClip;
        StartCoroutine(CrossfadeMusic(newClip, fadeDuration));
    }


    private IEnumerator CrossfadeMusic(AudioClip newClip, float fadeDuration)
    {
        isCrossfading = true;
        Debug.Log("Crossfading from " + (currentMusicClip != null ? currentMusicClip.name : "None") + " to " + newClip.name);

        // Assign the new clip to the inactive AudioSource
        crossfadeSource.clip = newClip;
        crossfadeSource.volume = 0f;
        crossfadeSource.loop = true;
        crossfadeSource.Play();

        float timer = 0f;
        float startVolume = audioSource.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            audioSource.volume = Mathf.Lerp(startVolume, 0.0f, progress);
            crossfadeSource.volume = Mathf.Lerp(0.0f, 1.0f, progress);

            yield return null;
        }

        Debug.Log("Crossfade complete. New track: " + newClip.name);

        // Stop and reset the old AudioSource
        audioSource.Stop();
        audioSource.clip = null; // Reset to avoid accidental playback
        audioSource.volume = 0f;  // Ensure it's completely silent

        // Swap the audio sources
        AudioSource temp = audioSource;
        audioSource = crossfadeSource;
        crossfadeSource = temp;

        isCrossfading = false;
        currentMusicClip = newClip;

        Debug.Log("Final confirmation: Only one track playing - " + currentMusicClip.name);
    }

}
