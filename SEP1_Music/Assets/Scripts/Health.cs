using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public int health;

    void Awake()
    {
        Reset();
    }
    
    public void Reset()
    {
        health = maxHealth;
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            health--;
        }
    }
}