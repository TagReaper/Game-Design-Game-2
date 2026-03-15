using System;
using System.Threading;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAI : MonoBehaviour
{
    #region Variables
    public Transform target;     // This is the current entity that the enemy is locked onto
    public Transform leftLimit;  // Left patroll boundary
    public Transform rightLimit; // Right patroll boundary

    [SerializeField] private float _speed;       // Assigns the enemy movement speed
    [SerializeField] private Animator _animator; // Assigns the enemy animation controller
    [SerializeField] private float attackDist;   // Assigns the enemy attack distance
    [SerializeField] private float attackCD;     // Assigns attack cooldown time

    private Rigidbody2D _rigidbody;                           // Assigns the enemy 2D body
    private PlayerAwarenessScript _playerAwarenessController; // References the PlayerAwarenessScript so the variables can be used
    private Vector2 _targetDirection;                         // Assigns a vector from the enemy to its target
    private float initialCD;                                  // Internal Variable to record the intial cooldown time
    private bool cooling;                                    // Internal bool to check if the enemy attack is currently cooling down
    private bool attackMode;                                 // Internal bool to check if the enemy can attack
    #endregion

    #region Main Functions
    // This function activates once the game starts up
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); // Gets access to the enemy rigid body
        _playerAwarenessController = GetComponent<PlayerAwarenessScript>(); // Gets access to the PlayerAwarenessScript
        initialCD = attackCD; // Records the intial cooldown time
        SelectTarget();       // Selects an intial target for patrolling to begin
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!attackMode) // Check if the enemy is attacking. Otherwise it just keeps moving towards its target
        {
            UpdateTargetDirection(); // Updates whether the enemy is chasing the player or is patrolling
            RotateTowardsTarget();   // Orientates the enemy to face its target
            SetVelocity();           // Sets the velocity that the enemy moves towards its target.
        }

        // This if statement makes sure that if the enemy is outside of its patrol boundary and is not aware of player that they return
        if(!InsideofLimits() && !_playerAwarenessController.AwareOfPlayer && !_animator.GetCurrentAnimatorStateInfo(0).IsName("enemyBODAttack"))
        {
            SelectTarget(); // Selects a new target once the enemy reaches a boundary
        }

        // This if statement makes sure that if the enemy attack is on cooldown that they don't attack and that the cooldown resets
        if (cooling)
        {
            Cooldown();
            _animator.SetBool("isAttacking", false); // Deactivates the attack animation
            _animator.SetBool("canMove", true);      // Activates the enemy ability to move
            attackMode = false;                      // deactivates internal bool for keeping track of whether enemy can attack
        }
        else
        {
            // this if statement makes sure the enemy doesn't attack the boundaries
            if (target != leftLimit && target != rightLimit)
            {
                Attack();
            }
        }
    }
    #endregion

    #region Enemy Movement
    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
            target = FindAnyObjectByType<PlayerMovement>().transform;
        }
        else
        {
            _targetDirection = target.position - this.transform.position;
        }
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero)
        {
            return;
        }
        else if (target.transform.position.x - transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (target.transform.position.x - transform.position.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

    private void SetVelocity()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("enemyBODAttack"))
        {
            if (_targetDirection == Vector2.zero)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                _animator.SetBool("isWalking", false);
            }
            else
            {
                Vector2 targetPositionXOnly = new Vector2(target.position.x, this.transform.position.y);
                transform.position = Vector2.MoveTowards(this.transform.position, targetPositionXOnly, _speed * Time.deltaTime);
                _animator.SetBool("isWalking", true);
            }
        }
    }
    #endregion

    #region Enemy Attack
    private void Attack()
    {
        if (Math.Abs(target.transform.position.x - transform.position.x) > attackDist)
        {
            cooling = false;
            attackMode = false;
            _animator.SetBool("isAttacking", false);
            _animator.SetBool("canMove", true);
        }
        else if (Math.Abs(target.transform.position.x - transform.position.x) <= attackDist)
        {
            attackMode = true;
            _rigidbody.linearVelocity = Vector2.zero;
            _animator.SetBool("canMove", false);
            _animator.SetBool("isAttacking", true);
            
        }
    }

    private void Cooldown()
    {
        attackCD -= Time.deltaTime;

        if (attackCD <= 0 && cooling)
        {
            cooling = false;
            attackCD = initialCD;
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }
    #endregion

    #region Patrolling
    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        //UpdateTargetDirection();
        //RotateTowardsTarget();
        //SetVelocity();
    }
    #endregion
}
