using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public float speed = 5f;
    public float despawnHeight = -10f;

    public bool nodeHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.down * speed * Time.deltaTime;
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
