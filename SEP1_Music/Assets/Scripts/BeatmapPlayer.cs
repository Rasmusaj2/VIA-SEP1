using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class BeatmapPlayer : MonoBehaviour
{
    public Beatmap beatmap;
    public BeatNode[] nodes = new BeatNode[10];
    public GameObject nodePrefab;

    public int currentNodeIndex = 0;
    public float playtime = 0f;
    
    public List<BeatNode> ActiveNodes = new List<BeatNode>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        playtime += Time.deltaTime;
        if (playtime >= nodes[currentNodeIndex].time && currentNodeIndex <= nodes.Length)
        {
            SpawnNode(nodes[currentNodeIndex]);
            currentNodeIndex++;
        }
    }

    void SpawnNode(BeatNode node)
    // Spawns node at appropriate lane position
    // TODO: Adjust spawn position based on lane and game design
    {
        Vector3 spawnPosition = Vector3.zero;
        switch (node.lane)
        {
            case Lanes.LeftLane:
                spawnPosition = new Vector3(-2f, 10f, 0f);
                break;
            case Lanes.LeftMidLane:
                spawnPosition = new Vector3(-1f, 10f, 0f);
                break;
            case Lanes.RightMidLane:
                spawnPosition = new Vector3(1f, 10f, 0f);
                break;
            case Lanes.RightLane:
                spawnPosition = new Vector3(2f, 10f, 0f);
                break;
        }
        Instantiate(nodePrefab, spawnPosition, Quaternion.identity, transform);
    }
}
