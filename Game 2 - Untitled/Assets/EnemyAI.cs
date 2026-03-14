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
    public Transform target;
    public Transform leftLimit;
    public Transform rightLimit;

    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private float attackDist;
    [SerializeField] private float attackCD;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessScript _playerAwarenessController;
    private Vector2 _targetDirection;
    private float initialCD;
    private bool cooling;
    private bool attackMode;
    #endregion

    #region Main Functions
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessScript>();
        initialCD = attackCD;
        SelectTarget();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!attackMode)
        {
            UpdateTargetDirection();
            RotateTowardsTarget();
            SetVelocity();
        }

        if(!InsideofLimits() && !_playerAwarenessController.AwareOfPlayer && !_animator.GetCurrentAnimatorStateInfo(0).IsName("enemyBODAttack"))
        {
            SelectTarget();
        }

        if (cooling)
        {
            Cooldown();
            _animator.SetBool("isAttacking", false);
            _animator.SetBool("canMove", true);
        }
        else
        {
            if (target != leftLimit && target !=rightLimit)
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

        if (attackCD <= 0 && cooling && attackMode)
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

        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }
    #endregion
}
