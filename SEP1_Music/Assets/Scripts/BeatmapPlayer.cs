using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class BeatmapPlayer : MonoBehaviour
{
    public Beatmap beatmap;
    [SerializeField] private BeatNode[] nodes = new BeatNode[10];
    public GameObject nodePrefab;

    public int currentNodeIndex = 0;
    public float playtime = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (beatmap == null || beatmap.Nodes == null || beatmap.Nodes.Length == 0)
        {
            Debug.LogWarning("No beatmap or nodes to play.");
            return;
        }

        // Safety check for currentNodeIndex bounds
        if (currentNodeIndex < 0) currentNodeIndex = 0;


        playtime += Time.deltaTime;

        if (currentNodeIndex >= beatmap.Nodes.Length)
        {
            return; // All nodes spawned
        }

        if (playtime >= beatmap.Nodes[currentNodeIndex].time)
        {
            BeatNode node = beatmap.Nodes[currentNodeIndex];
            if (node != null)
            {
                SpawnNode(node);
            }
            else
            {
                Debug.LogWarning($"BeatNode at index {currentNodeIndex} is null.");
            }

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
