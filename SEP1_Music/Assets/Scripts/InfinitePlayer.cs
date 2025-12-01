using UnityEngine;
using UnityEngine.Rendering;

public class InfinitePlayer : MonoBehaviour
{
    public float playtime = 0f;
    public GameObject nodePrefab;

    [Header("Lane Settings")]
    [SerializeField] private Lanes lastLane;
    [SerializeField] private float lastNodeTime;

    [Header("Speed Settings")]
    public float StartSpeed = 25f;
    public float MaxSpeed = 100f;
    public float speedIncreaseRate = 100.0f;
    public float currentSpeed; // Has to be public for NodeScript access

    [Header("Spawn Interval Settings")]
    public float BaseSpawnInterval = 2.0f; 
    public float MinSpawnInterval = 0.5f;
    public float spawnIntervalDecreaseRate = 100.0f;
    [SerializeField] private float SpawnInterval = 1.5f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = StartSpeed;
        SpawnInterval = BaseSpawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        playtime += Time.deltaTime;
        if (playtime - lastNodeTime >= SpawnInterval)
        {
            
            // Randomly select a lane different from the last one
            Lanes newLane;
            do
            {
                newLane = (Lanes)Random.Range(0, 4); // 4 lanes, 0 to 3 (can technically get stuck in a loop but psuedo-random should make it unlikely)
            } while (newLane == lastLane);

            BeatNode newNode = new BeatNode(playtime, newLane);

            SpawnNode(newNode);
            lastLane = newLane;
            lastNodeTime = playtime;
        }
        CalculateNewSpeed();
        CalculateNewSpawnInterval();
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

    void CalculateNewSpawnInterval()
    {
        float t = 1f - Mathf.Exp(-playtime * spawnIntervalDecreaseRate / 1000);
        SpawnInterval = Mathf.Lerp(BaseSpawnInterval, MinSpawnInterval, t);
    }

    void CalculateNewSpeed() {
        // Update this math function
        float new_speed = StartSpeed + (speedIncreaseRate * (playtime / speedIncreaseRate));
        currentSpeed = Mathf.Clamp(new_speed, StartSpeed, MaxSpeed);
    }
}
