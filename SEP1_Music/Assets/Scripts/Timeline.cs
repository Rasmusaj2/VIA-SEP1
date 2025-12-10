using UnityEngine;

public class Timeline : MonoBehaviour
{
    [Header("Parameters")]
    [Min(1)]
    public int beatsPerMeasure = 4;
    [Min(1)]
    public double beatsPerMinute = 128;

    [Header("Time")]
    public double startTime;
    public double time = 0.0; // Time since the start of the timeline
    public double beat = 0.0; // Also provide the time in number of beats, for easier sync with music

    void Awake()
    {
        // The current time of the audio system can be at any value when the timeline
        // starts (it'll typically be the time since the start of the application, and
        // thus the start of the audio system). Hence we read the time once when the
        // timeline starts, and then measure all timeline-related timings against this
        // starting time
        startTime = Time.realtimeSinceStartupAsDouble;
    }

    void Update()
    {
        time = Time.realtimeSinceStartupAsDouble - startTime;
        beat = ToBeats(time);
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
    // to the audio system. Use this to schedule sound effects to exact times
    public double ToRealTime(double time)
    {
        return startTime + time;
    }
}