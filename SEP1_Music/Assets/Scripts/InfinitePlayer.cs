using UnityEngine;
using UnityEngine.Rendering;

public class InfinitePlayer : MonoBehaviour
{
    public float playtime = 0f;
    public GameObject nodePrefab;
    public int health = 3;
    public NoteSpawner noteSpawner;
    private bool hasLost;

    [Header("Lane Settings")]
    [SerializeField] private LaneType lastLane;
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
        //if (health > 0)
        //{
            playtime += Time.deltaTime;
            if (playtime - lastNodeTime >= SpawnInterval)
            {
                // Randomly select a lane different from the last one
                LaneType newLane;
                do
                {
                    newLane = (LaneType)Random.Range(0, 4); // 4 lanes, 0 to 3 (can technically get stuck in a loop but psuedo-random should make it unlikely)
                } while (newLane == lastLane);

                noteSpawner.SpawnNote(newLane, playtime); // TODO: Make use of a timeline object.
                lastLane = newLane;
                lastNodeTime = playtime;
            }
            CalculateNewSpeed();
            CalculateNewSpawnInterval();
        //}
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
