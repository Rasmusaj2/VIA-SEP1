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
        //Rays registrerer kun nodes
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
            //Når nodehit = true evalueres scoren baseret på afstand fra scoreline og objektet fjernes
            gameObject.GetComponentInParent<ScoreManager>().EvaluateHit(gameObject.transform.position.y, 0.0);
            Destroy(gameObject);
        }
        else
        {
            if(takesInput == false && gameObject.transform.position.y < takesInputFromY)
            {
                CheckIfNext(); //Hvis noden ikke allerede tager imod input, vil den tjekke om den burde gøre det, hvis den er tæt nok på scorelinen til det.
            }
            else if (debugRays == true && gameObject.transform.position.y < takesInputFromY)
            {
                //Visualiserer i viewporten at noden er ændret til at tage imod input.
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
            //gameObject.GetComponentInParent<ScoreManager>().MissedHit();
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
            //Visualiserer i editor at noden tjekker, men ikke tager imod input.
            Debug.DrawRay(gameObject.transform.position, transform.TransformDirection(Vector2.down) * maxRayDistance, Color.red);
        }
        
        raycastHits = new RaycastHit2D[2]; //Array af resultater fra raycast fra noden, som maks indeholder 2.
        
        contactFilter2D.layerMask = LayerMask.GetMask("Nodes"); //Raycast sættes til kun at registrerer colliders fra nodes.

        //Noden raycaster nedad ud fra sin position. Derefter gemmer den resultaterne til raycastHits og returnerer antallet af hits som int nodesDetected.
        int nodesDetected = Physics2D.Raycast(gameObject.transform.position, transform.TransformDirection(Vector2.down), contactFilter2D, raycastHits, maxRayDistance);


        if (nodesDetected == 1 && gameObject.transform.position.y < takesInputFromY)
        {
            takesInput = true; //Nodesdetected vil altid være mindst en, da 2D raycasts registrerer colliders, som de starter fra. 
            //Hvis der ikke er over 1, vil noden være den næste i rækken til at tage imod input.
        }
        
    }
}
