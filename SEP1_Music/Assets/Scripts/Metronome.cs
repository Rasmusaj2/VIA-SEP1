using System;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public Timeline timeline;
    public AudioClip beatSoundClip;
    public AudioClip semiBeatSoundClip;
    [Range(0.0f, 1000.0f)]
    public double audioDelayCompensationMilliseconds = 40.0;
    [Min(0)]
    public double scheduleAheadSeconds = 0.2;

    private double nextEventTime;
    // Several sources to play sounds at every beat.
    // More sources will prevent scheduled sounds from interfering
    private AudioSource[] audioSources = new AudioSource[8];
    private int source = 0;

    void Start()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            GameObject child = new GameObject("AudioPlayer");
            child.transform.parent = transform;
            audioSources[i] = child.AddComponent<AudioSource>();
        }

        nextEventTime = timeline.ToSeconds(0.0);
    }

    void Update()
    {
        double audioDelayCompensationSeconds = 0.001 * audioDelayCompensationMilliseconds;

        if (timeline.time + scheduleAheadSeconds > nextEventTime)
        {
            // We are now approx. at a time shortly before the time at which the sound should
            // play, based on the value given by scheduleAheadSeconds, so we will schedule it now
            // in order for the system to have enough to prepare the playback at the specified time.
            // This may involve opening buffering a streamed file and should therefore take any
            // worst-case delay into account.
            int beat = (int)Math.Round(timeline.ToBeats(nextEventTime)); // Round to the nearest beat
            int subdivision = beat % timeline.beatsPerMeasure;
            // Either load a sound for the first beat or a sound for the other beats,
            // depending on the current subdivision within the measure.
            AudioClip clip = subdivision == 0 ? beatSoundClip : semiBeatSoundClip;
            // Calculate the time to schedule the clip, and apply any audio latency compensation
            double scheduledTime = timeline.ToAudioTime(nextEventTime) - audioDelayCompensationSeconds;

            audioSources[source].clip = clip;
            audioSources[source].PlayScheduled(scheduledTime);
            // Cycle between audio sources so that the loading process of one does not interfere with the others
            source = (source + 1) % audioSources.Length;

            Debug.Log($"Scheduled source {source} to start at time {scheduledTime} (beat {beat})");

            // Place the next event on the next whole beat from here
            nextEventTime = timeline.ToSeconds(beat + 1.0);
        }
    }
}
