using System.Runtime.CompilerServices;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public float speed;
    public float despawnHeight = -4f;
    Ray2D ray;
    public float maxRayDistance = 15;
    LayerMask layerToHit;
    ContactFilter2D contactFilter2D = new ContactFilter2D();
    RaycastHit2D[] raycastHits;
    public float takesInputFromY = 0;
    public bool takesInput = false;
    public bool nodeHit = false;
    public bool debugRays;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        layerToHit = LayerMask.GetMask("Nodes");

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
        TestHit();

        if (nodeHit == true)
        {
            gameObject.GetComponentInParent<ScoreManager>().EvaluateHit(gameObject.transform.position.y);
            Destroy(gameObject);
        }
        else
        {
            if(takesInput == false && gameObject.transform.position.y < takesInputFromY)
            {
                CheckIfNext();
            }
            else if (debugRays == true && gameObject.transform.position.y < takesInputFromY)
            {
                Debug.DrawRay(gameObject.transform.position, transform.TransformDirection(Vector2.down) * maxRayDistance, Color.green);
            }

            gameObject.transform.position += Vector3.down * (speed / 10.0f) * Time.deltaTime;
            HeightCheck();
        }
    }

    void HeightCheck()
    {
        if (gameObject.transform.position.y <= despawnHeight)
        {
            gameObject.GetComponentInParent<ScoreManager>().MissedHit();
            Destroy(gameObject);
        }
    }

    void TestHit()
    {
        if (gameObject.transform.position.y <= -3 && takesInput == true)
        {
            nodeHit = true;
        }
    }

    void CheckIfNext()
    {
        

        if (debugRays == true)
        {
            Debug.DrawRay(gameObject.transform.position, transform.TransformDirection(Vector2.down) * maxRayDistance, Color.red);
        }

        
        raycastHits = new RaycastHit2D[2];
        
        contactFilter2D.layerMask = LayerMask.GetMask("Nodes");

        int nodesDetected = Physics2D.Raycast(gameObject.transform.position, transform.TransformDirection(Vector2.down), contactFilter2D, raycastHits, maxRayDistance);


        if (nodesDetected == 1 && gameObject.transform.position.y < takesInputFromY)
        {
            takesInput = true;
        }
        
    }
}
