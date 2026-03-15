using UnityEngine;

public class Sword : MonoBehaviour
{
    public Damage damage;
    public SpriteRenderer sprite;
    public void Hit()
    {
        Debug.Log("Hit");
    }
}