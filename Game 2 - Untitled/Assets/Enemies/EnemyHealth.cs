using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Enemy is DEAD!");
        }
    }
}
