using UnityEngine;
using System.Collections.Generic;

public class Timeline : MonoBehaviour
{
    public double startTime;
    public double time = 0.0;
    public double beat = 0.0;
    [Min(1)]
    public int beatsPerMeasure = 4;
    [Min(1)]
    public double beatsPerMinute = 128;

    void Start()
    {
        startTime = AudioSettings.dspTime;
    }

    void Update()
    {
        time = AudioSettings.dspTime - startTime;
        beat = ToBeats(time);
    }

    public double ToBeats(double seconds)
    {
        return seconds * beatsPerMinute / 60.0;
    }

    public double ToSeconds(double beats)
    {
        return 60.0 * beats / beatsPerMinute;
    }

    public double ToRealTime(double time)
    {
        return startTime + time;
    }
}