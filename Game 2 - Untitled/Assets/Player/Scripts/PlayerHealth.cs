using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Slider healthBar;
    public PlayerMovement movement;
    public SpriteRenderer sprite;
    private bool dead = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
        if (health <= 0 && !dead)
        {
            dead = true;
            movement.isDead = true;
            movement.animator.SetTrigger("Dead");
            movement.weapon.canAttack = false;
        }
    }
    public void Hurt(float damage)
    {
        if (!dead)
        {
            sprite.color = new Color(1f, 0f, 0f, 1f);
            health -= damage;
            Invoke(nameof(Flash), 0.1f);
        }
    }

    private void Flash()
    {
        sprite.color = new Color(1f, 1f, 1f, 1f);
    }
}
