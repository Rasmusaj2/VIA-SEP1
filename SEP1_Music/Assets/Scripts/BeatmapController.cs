using UnityEngine;

public enum HitPhase
{
    Attack,
    Release,
}

public class BeatmapController : MonoBehaviour
{
    public double timeStart;

    public void Hit(Lanes lane, HitPhase phase, double time) { }

    void Awake()
    {
        timeStart = Time.realtimeSinceStartupAsDouble;
    }
}