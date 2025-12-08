using UnityEngine;
using System.Collections.Generic;

public class Timeline : MonoBehaviour
{
    public double time = 0.0;
    public double beat = 0.0;
    public int beatsPerMeasure = 4;
    public double beatsPerMinute = 128;

    void Update()
    {
        time = Time.realtimeSinceStartupAsDouble;
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
}