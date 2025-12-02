using UnityEngine;
using System.Collections.Generic;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // EXAMPLE SETUP FOR CROSS SCENE MANAGER AND BEATMAP
        List<BeatNode> nodes = new List<BeatNode>();
        Beatmap beatmap;
        // Unity force check to avoid IndexOutOfRangeException

        // example beatmap initialization
        nodes.Add(new BeatNode(1.0f, Lanes.LeftLane));
        nodes.Add(new BeatNode(2.0f, Lanes.RightLane));
        nodes.Add(new BeatNode(3.0f, Lanes.LeftMidLane));
        nodes.Add(new BeatNode(4.0f, Lanes.RightMidLane));
        nodes.Add(new BeatNode(5.0f, Lanes.LeftLane));
        nodes.Add(new BeatNode(6.0f, Lanes.RightLane));
        nodes.Add(new BeatNode(7.0f, Lanes.LeftMidLane));
        nodes.Add(new BeatNode(8.0f, Lanes.RightMidLane));
        nodes.Add(new BeatNode(9.0f, Lanes.LeftLane));
        nodes.Add(new BeatNode(10.0f, Lanes.RightLane));

        beatmap = new Beatmap(nodes, "Example Beatmap", "Example Artist", 120f);
        JSONPersistence.SaveToJSON<Beatmap>(beatmap, "maps/test_map/beatmap.json");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
