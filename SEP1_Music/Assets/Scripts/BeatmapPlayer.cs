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
        if (CrossSceneManager.SelectedBeatmap != null)
        {
            beatmap = CrossSceneManager.SelectedBeatmap;
            Debug.Log($"Loaded beatmap: {beatmap.MapName} with {beatmap.Nodes.Length} nodes.");
            CrossSceneManager.SelectedBeatmap = null; // Clear after loading
        }
        else
        {
            Debug.LogError("No beatmap selected in CrossSceneManager.");
        }
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
                spawnPosition = new Vector3((float)LaneLocations.LeftLane, 10f, 0f);
                break;
            case Lanes.LeftMidLane:
                spawnPosition = new Vector3((float)LaneLocations.LeftMidLane, 10f, 0f);
                break;
            case Lanes.RightMidLane:
                spawnPosition = new Vector3((float)LaneLocations.RightLane, 10f, 0f);
                break;
            case Lanes.RightLane:
                spawnPosition = new Vector3((float)LaneLocations.RightMidLane, 10f, 0f);
                break;
        }
        Instantiate(nodePrefab, spawnPosition, Quaternion.identity, transform);
    }
}
