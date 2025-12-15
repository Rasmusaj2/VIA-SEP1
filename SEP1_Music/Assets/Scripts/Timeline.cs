using UnityEngine;

public class Timeline : MonoBehaviour
{
    [Header("Parameters")]
    [Min(1)]
    public int beatsPerMeasure = 4;
    [Min(1)]
    public double beatsPerMinute = 128;
    public double anticipation = 5.0;

    [Header("Time")]
    public double realStartTime;
    public double audioStartTime;
    public double time = 0.0; // Time since the start of the timeline
    public double beat = 0.0; // Also provide the time in number of beats, for easier sync with music
    public bool isPlaying = false;

    void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
        if (isPlaying)
        {
            time = Time.realtimeSinceStartupAsDouble - realStartTime;
            beat = ToBeats(time);
        }
    }

    public void Play()
    {
        // The current time since the application start can be at any value when
        // the timeline starts. Hence we read the time once when the
        // timeline starts, and then measure all timeline-related timings against this
        // starting time
        realStartTime = Time.realtimeSinceStartupAsDouble + anticipation;
        audioStartTime = AudioSettings.dspTime + anticipation;
        isPlaying = true;
    }

    public void Stop()
    {
        isPlaying = false;
    }
    
    // Converts seconds into a fractional number of beats, based on bpm
    public double ToBeats(double seconds)
    {
        return seconds * beatsPerMinute / 60.0;
    }

    // Converts number of beats into seconds
    public double ToSeconds(double beats)
    {
        return 60.0 * beats / beatsPerMinute;
    }

    // Convert a time since the timeline started into the real time relative
    // to the start of the application. Use this to compare with input timings
    public double ToRealTime(double time)
    {
        return realStartTime + time;
    }

    // Convert a time since the timeline started into the real time relative
    // to the audio system. Use this to schedule sound effects to exact times
    public double ToAudioTime(double time)
    {
        return audioStartTime + time;
    }
}