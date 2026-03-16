using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public SpriteRenderer sprite;
    private bool dead = false;
    public ParticleSystem deathFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !dead)
        {
            dead = true;
            Debug.Log("Enemy is DEAD!");
            deathFX.Play();
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
