using UnityEngine;

public class BeatmapController : MonoBehaviour
{
    public double TimeStart;

    public void Hit(Lanes lane, double time) { }

    public void Release(Lanes lane, double time) { }

    void Awake()
    {
        TimeStart = Time.realtimeSinceStartupAsDouble;
    }
}