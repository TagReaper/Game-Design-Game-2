using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;
    public bool enemy;
    public bool active;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (active){
            if (other.gameObject.CompareTag("Player") && enemy)
            {
                other.gameObject.GetComponent<PlayerHealth>().Hurt(damage);
            } else if (other.gameObject.CompareTag("Enemy") && !enemy)
            {
                other.gameObject.GetComponent<EnemyHealth>().Hurt(damage);
            }
        }
    }
}