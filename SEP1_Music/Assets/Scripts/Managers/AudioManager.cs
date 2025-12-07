using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float crossfadeTime = 1f;

    public static AudioManager Instance;

    [Header("Audio Sources")]
    // Index 0 & 1 used for crossfading
    public AudioSource[] musicSources = new AudioSource[2];

    private int activeIndex = 0;
    private int inactiveIndex
    {
        get
        {
            if (activeIndex == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    private AudioSource Active => musicSources[activeIndex];
    private AudioSource Inactive => musicSources[inactiveIndex];

    [Header("Mixer Settings")]
    public AudioMixer audiomixer;
    public string musicVolumeParameter = "MusicVolume";

    private double songStartDspTime;
    private bool songIsPlaying = false;

    [Header("JSON Files")]
    private List<AudioClip> loadedSongs = new List<AudioClip>();
    private int currentSongIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Update()
    {
        if (loadedSongs.Count > 0 && IsSongFinished())
        {
            PlayNextSong();
        }
    }

    public void PlaySong(AudioClip clip, float startTime = 0f)
    {
        Active.clip = clip;

        songStartDspTime = AudioSettings.dspTime + 0.010f;
        Active.time = startTime;
        Active.PlayScheduled(songStartDspTime);

        songIsPlaying = true;
    }

    public void StartSongRotation()
    {
        currentSongIndex = 0;
        CrossfadeTo(loadedSongs[currentSongIndex]);
    }

    public void PlayNextSong()
    {
        currentSongIndex++;

        if (currentSongIndex >= loadedSongs.Count)
            currentSongIndex = 0;

        CrossfadeTo(loadedSongs[currentSongIndex]);
    }

    public void CrossfadeTo(AudioClip clip)
    {
        Inactive.clip = clip;
        Inactive.volume = 0f;

        double startTime = AudioSettings.dspTime + 0.010f;
        Inactive.PlayScheduled(startTime);

        StartCoroutine(CrossfadeRoutine(crossfadeTime));
    }

    public void AddSong(AudioClip clip)
    {
        loadedSongs.Add(clip);
    }

    public bool IsSongFinished()
    {
        return !Active.isPlaying;
    }

    public void ResumeSong()
    {
        Active.UnPause();
        songIsPlaying = true;
    }

    public void PauseSong()
    {
        Active.Pause();
        songIsPlaying = false;
    }

    public void StopSong()
    {
        Active.Stop();
        Inactive.Stop();
        songIsPlaying = false;
    }

    public float SongTime
    {
        get
        {
            if (Active == null || Active.clip == null)
                return 0f;

            return Active.time;
        }
    }

    private IEnumerator CrossfadeRoutine(float fadeDuration)
    {
        float currentTime = 0f;

        float activeStart = 1f;
        float activeEnd = 0f;

        float inactiveStart = 0f;
        float inactiveEnd = 1f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;

            float progress = currentTime / fadeDuration;

            Active.volume = activeStart + (activeEnd - activeStart) * progress;
            Inactive.volume = inactiveStart + (inactiveEnd - inactiveStart) * progress;

            yield return null;
        }
        Active.volume = 1f;
        Inactive.volume = 0f;
        musicSources[inactiveIndex].Stop();
    } 
}
