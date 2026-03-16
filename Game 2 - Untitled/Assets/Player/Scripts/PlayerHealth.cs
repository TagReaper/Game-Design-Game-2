using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Slider healthBar;
    public SpriteRenderer sprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
        if (health <= 0)
        {
            Debug.Log("Player is DEAD!");
        }
    }
    public void Hurt(float damage)
    {
        sprite.color = new Color(1f, 0f, 0f, 1f);
        health -= damage;
        Invoke(nameof(Flash), 0.1f);
    }

    private void Flash()
    {
        sprite.color = new Color(1f, 1f, 1f, 1f);
    }
}
