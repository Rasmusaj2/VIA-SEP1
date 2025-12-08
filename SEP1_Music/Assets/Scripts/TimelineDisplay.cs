using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timeline))]
public class TimelineDisplay : MonoBehaviour
{
    public int maxBars = 4;
    public double distancePerMeasure = 4.0f;

    public GameObjectPool barLines;
    public GameObjectPool semiBarLines;

    private Timeline timeline;
    private List<Transform> barLineTransforms;
    private List<Transform> semiBarLineTransforms;

    private double ToBeats(float displacement)
    {
        return 4.0 * (double)displacement / distancePerMeasure;
    }

    private float ToDisplacement(double beats)
    {
        return (float)(0.25 * distancePerMeasure * beats);
    }

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
        float yMin = -5.0f;
        float yMax = 5.0f;

        int startingBeat = (int)(timeline.beat + ToBeats(yMin));
        int endingBeat = (int)(timeline.beat + ToBeats(yMax));
        int beatsAmount = endingBeat - startingBeat + 1;

        int bi = 0;
        int sbi = 0;
        for (int i = 0; i < beatsAmount; i++)
        {
            int beat = startingBeat + i;
            Transform b;

            if (beat % timeline.beatsPerMeasure == 0)
            {
                if (bi == barLineTransforms.Count)
                {
                    GameObject barLine;
                    barLine = barLines.Acquire();
                    barLineTransforms.Add(barLine.transform);
                    b = barLine.transform;
                }
                else
                {
                    b = barLineTransforms[bi];
                }

                bi++;
            }
            else
            {
                if (sbi == semiBarLineTransforms.Count)
                {
                    GameObject barLine;
                    barLine = semiBarLines.Acquire();
                    semiBarLineTransforms.Add(barLine.transform);
                    b = barLine.transform;
                }
                else
                {
                    b = semiBarLineTransforms[sbi];
                }

                sbi++;
            }

            b.position = new Vector3(b.position.x, ToDisplacement(beat - timeline.beat), b.position.z);
        }

        for (int i = bi; i < barLineTransforms.Count; i++)
        {
            barLines.Release(barLineTransforms[i].gameObject);
            barLineTransforms.Remove(barLineTransforms[i]);
        }

        for (int i = sbi; i < semiBarLineTransforms.Count; i++)
        {
            semiBarLines.Release(semiBarLineTransforms[i].gameObject);
            semiBarLineTransforms.Remove(semiBarLineTransforms[i]);
        }
    }
}
