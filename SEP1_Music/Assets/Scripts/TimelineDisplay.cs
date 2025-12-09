using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timeline))]
public class TimelineDisplay : MonoBehaviour
{
    [Header("Parameters")]
    public int maxBars = 8; // Number of bar lines to allocate
    public double distancePerMeasure = 4.0f;
    [Header("Object Pools")]
    public GameObjectPool barLines;
    public GameObjectPool semiBarLines;
    [Header("Display Limits")]
    public float lowerLimit = -8.0f;
    public float upperLimit = 8.0f;

    private Timeline timeline;
    private List<Transform> barLineTransforms;
    private List<Transform> semiBarLineTransforms;

    void Awake()
    {
        timeline = GetComponent<Timeline>();
        barLineTransforms = new List<Transform>();
        semiBarLineTransforms = new List<Transform>();
    }

    void Start()
    {
        barLines.Allocate(maxBars);
        semiBarLines.Allocate((timeline.beatsPerMeasure - 1) * maxBars);
    }

    void Update()
    {
        int startingBeat = (int)(timeline.beat + ToBeats(lowerLimit));
        int endingBeat = (int)(timeline.beat + ToBeats(upperLimit));
        int beatsAmount = endingBeat - startingBeat + 1;

        int bi = 0;
        int sbi = 0;
        for (int i = 0; i < beatsAmount; i++)
        {
            int beat = startingBeat + i;
            Transform b;

            if (beat % timeline.beatsPerMeasure == 0)
            {
                b = GetBarLine(barLines, barLineTransforms, bi++);
            }
            else
            {
                b = GetBarLine(semiBarLines, semiBarLineTransforms, sbi++);
            }

            b.position = new Vector3(b.position.x, ToDisplacement(beat - timeline.beat), b.position.z);
        }

        ReleaseBarLines(barLines, barLineTransforms, bi);
        ReleaseBarLines(semiBarLines, semiBarLineTransforms, sbi);
    }

    private Transform GetBarLine(GameObjectPool pool, List<Transform> transforms, int index)
    {
        if (index == transforms.Count)
        {
            GameObject barLine;
            barLine = pool.Acquire();
            transforms.Add(barLine.transform);
            return barLine.transform;
        }
        else
        {
            return transforms[index];
        }
    }

    private void ReleaseBarLines(GameObjectPool pool, List<Transform> transforms, int startIndex)
    {
        for (int i = startIndex; i < transforms.Count; i++)
        {
            pool.Release(transforms[i].gameObject);
            transforms.Remove(transforms[i]);
        }
    }

    public double ToBeats(float displacement)
    {
        return 4.0 * (double)displacement / distancePerMeasure;
    }

    public float ToDisplacement(double beats)
    {
        return (float)(0.25 * distancePerMeasure * beats);
    }

    public void PositionToTimeline(Transform transform, double beat)
    {
        transform.position = new Vector3(
            transform.position.x,
            ToDisplacement(beat - timeline.beat),
            transform.position.z);
    }
}
