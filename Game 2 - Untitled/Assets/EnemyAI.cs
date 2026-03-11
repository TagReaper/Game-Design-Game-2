using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessScript _playerAwarenessController;
    private Vector2 _targetDirection;
    
    public float AggroDist;
    public float AggroKeepDist;
    public float rotationSpeed = 50f;

    private float distance;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessScript>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
        //distance = Vector2.Distance(transform.position, player.transform.position);
        //Vector2 direction = player.transform.position - transform.position;

        //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }
    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else
        {
            _targetDirection = Vector2.zero;
        }
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero)
        {
            return;
        }
        else if (player.transform.position.x - transform.position.x > 0)
        {
            //transform.Rotate(0, 180, 0, Space.Self);
            //transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //transform.localScale = new Vector2(-1, 1);
        }
        else if (player.transform.position.x - transform.position.x < 0)
        {
            //transform.Rotate(0, 0, 0, Space.Self);
            //transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //transform.localScale = new Vector2(1, 1);
        }

    }

    private void SetVelocity()
    {
        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
        else
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, _speed * Time.deltaTime);
        }
    }
}
