using System.Collections.Generic;
using UnityEngine;

public class TimelineDisplay : MonoBehaviour
{
    public int maxBars = 4;
    public int beatsPerMeasure = 4;

    public GameObjectPool barLines;
    public GameObjectPool semiBarLines;

    void Awake()
    {
    }

    void Start()
    {
        barLines.Allocate(maxBars);
        semiBarLines.Allocate((beatsPerMeasure - 1) * maxBars);
    }

    void Update()
    {
    }
}
