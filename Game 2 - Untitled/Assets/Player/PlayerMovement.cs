using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private Animator _animator;

    float jump;
    float horizontalMovement;
    bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);

    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

        if (context.ReadValue<Vector2>().x == 1)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (context.ReadValue<Vector2>().x == -1)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (context.ReadValue<Vector2>().x != 0)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jumping {context.performed}");
        {
            if (context.performed && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isGrounded = false;
                _animator.SetBool("isJumping", true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        _animator.SetBool("isJumping", false);
    }

}
