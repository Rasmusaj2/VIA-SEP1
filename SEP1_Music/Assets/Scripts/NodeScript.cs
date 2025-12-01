using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public float speed;
    public float despawnHeight = -10f;

    public bool nodeHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.GetComponentInParent<BeatmapPlayer>() != null)
        {
            speed = gameObject.GetComponentInParent<BeatmapPlayer>().beatmap.NodeSpeed;
            Debug.Log("Loaded speed from BeatmapPlayer: " + speed);
        } else
        {
            speed =  gameObject.GetComponentInParent<InfinitePlayer>().currentSpeed;
            Debug.Log("Loaded speed from InfinitePlayer: " + speed);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.down * (speed / 10.0f) * Time.deltaTime;
        HeightCheck();
    }

    void HeightCheck()
    {
        if (gameObject.transform.position.y <= despawnHeight)
        {
            // add missed node logic
            Destroy(gameObject);
        }
    }
}
