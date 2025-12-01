using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // EXAMPLE SETUP FOR CROSS SCENE MANAGER AND BEATMAP
        BeatNode[] nodes = new BeatNode[10];
        Beatmap beatmap;
        // Unity force check to avoid IndexOutOfRangeException
        if (nodes == null || nodes.Length < 10)
        {
            nodes = new BeatNode[10];
            Debug.Log("BeatmapPlayer: 'nodes' array initialized to length 10 to avoid IndexOutOfRangeException.");
        }

        // example beatmap initialization
        nodes[0] = new BeatNode(1.0f, Lanes.LeftLane);
        nodes[1] = new BeatNode(2.0f, Lanes.RightLane);
        nodes[2] = new BeatNode(3.0f, Lanes.LeftMidLane);
        nodes[3] = new BeatNode(4.0f, Lanes.RightMidLane);
        nodes[4] = new BeatNode(5.0f, Lanes.LeftLane);
        nodes[5] = new BeatNode(6.0f, Lanes.RightLane);
        nodes[6] = new BeatNode(7.0f, Lanes.LeftMidLane);
        nodes[7] = new BeatNode(8.0f, Lanes.RightMidLane);
        nodes[8] = new BeatNode(9.0f, Lanes.LeftLane);
        nodes[9] = new BeatNode(10.0f, Lanes.RightLane);

        beatmap = new Beatmap(nodes, "Example Beatmap", "Example Artist", 120f);
        CrossSceneManager.SelectedBeatmap = beatmap;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
