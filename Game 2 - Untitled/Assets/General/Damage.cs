using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage;
    public bool enemy;
    public bool active;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (active){
            Debug.Log(other.gameObject.name);//Enemies can only harm Players and vise-versa if hitbox is active
            if (other.gameObject.CompareTag("Player") && enemy)
            {
                other.gameObject.GetComponent<PlayerHealth>().health -=damage;
            } else if (other.gameObject.CompareTag("Enemy") && !enemy)
            {
                Debug.Log("HIT");
                other.gameObject.GetComponent<EnemyHealth>().health -=damage;
            }
        }
    }
}