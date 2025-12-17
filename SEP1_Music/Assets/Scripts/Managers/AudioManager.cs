using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public enum MusicNode
    {
        None,
        Menu,
        Game,
        Infinity
    }

    public MusicNode currentMode = MusicNode.None;

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

    [Header("Music Playlist")]
    public List<AudioClip> playlist = new List<AudioClip>();
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
            return;
        }

        if (musicSources == null || musicSources.Length < 2)
            musicSources = new AudioSource[2];

        for (int i = 0; i < 2; i++)
        {
            if (musicSources[i] == null)
            {
                AudioSource newSource = gameObject.AddComponent<AudioSource>();
                newSource.playOnAwake = false;
                newSource.loop = false;
                musicSources[i] = newSource;
            }
        }
    }
    
    private void Update()
    {
        if (currentMode == MusicNode.Infinity && playlist.Count > 0 
            && songIsPlaying && IsSongFinished())
        {
            PlayRandomSong();
        }
    }

    public void PlaySong(AudioClip clip, double startTime = 0.0, double scheduleTime = 0.01)
    {
        Active.clip = clip;

        songStartDspTime = scheduleTime;
        Active.time = (float)startTime;
        Active.PlayScheduled(songStartDspTime);

        songIsPlaying = true;
    }

    public void StartSongRotation()
    {
        currentSongIndex = 0;
        CrossfadeTo(playlist[currentSongIndex]);
    }

    public void PlayRandomSong()
    {
        // Debug.Log("PlayRandomSong() Called");

        if (playlist.Count == 0)
        {
            // Debug.Log("Playlist is empty");
            return;
        }

        int newIndex = Random.Range(0, playlist.Count);

        currentSongIndex = newIndex;

        CrossfadeTo(playlist[currentSongIndex]);
    }

    public void PlayLoopSong(AudioClip clip)
    {
        currentMode = MusicNode.Menu;

        Active.loop = true;
        Active.clip = clip;
        Active.volume = 1f;
        Active.Play();

        songIsPlaying = true;
    }

    public void CrossfadeTo(AudioClip clip)
    {
        // Debug.Log("CrossfadeTo called");

        AudioSource newSource = Inactive;
        newSource.clip = clip;
        newSource.volume = 0f;

        double startTime = AudioSettings.dspTime + 0.010f;
        newSource.PlayScheduled(startTime);

        songStartDspTime = startTime;

        StartCoroutine(CrossfadeRoutine(crossfadeTime));
    }

    public void AddSong(AudioClip clip)
    {
        playlist.Add(clip);
    }

    public bool IsSongFinished()
    {
        if (Active.clip == null)
            return false;

        double timeSinceStart = AudioSettings.dspTime - songStartDspTime;

        return timeSinceStart >= Active.clip.length;
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

    private IEnumerator CrossfadeRoutine(float duration)
    {
        float t = 0f;

        AudioSource oldSource = Active;
        AudioSource newSource = Inactive;

        while (t < duration)
        {
            t += Time.deltaTime;

            float progress = t / duration;

            oldSource.volume = Mathf.Lerp(1f, 0f, progress);
            newSource.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        oldSource.volume = 0f;
        newSource.volume = 1f;

        oldSource.Stop();

        activeIndex = inactiveIndex;

        songStartDspTime = AudioSettings.dspTime;

        songIsPlaying = true;
    } 
}
