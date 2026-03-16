using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public SpriteRenderer sprite;
    public bool isRanged;
    public Timer cooldown;

    public void Attack(InputAction.CallbackContext context)
    {
        if(cooldown.isEnded){
            Hit();
            cooldown.Play();
        }

    }
    public void Hit()
    {
        Debug.Log("Hit");
    }
}