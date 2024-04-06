using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
    }

    private static Transform player;
    
    [SerializeField]
    private Rigidbody2D rigidbody2d;
    [SerializeField]
    private SpritesetAnimator animator;
    [ShowNonSerializedField]
    private Vector2 currentTarget;

    [Header("Patrolling")]
    [SerializeField]
    private float randomMovementRange;
    private Vector2 randomMovementCenter;
    [SerializeField]
    private float randomMovementChangeDelay;
    [SerializeField]
    private float patrollingSpeed;

    [Header("Detection")]
    [SerializeField]
    private float playerDetectionRange;
    [SerializeField]
    private Animation exclamationAnimation;

    [Header("Attacking")]
    [SerializeField]
    private float chasingSpeed;
    [SerializeField]
    private float escapeDistance;
    [ShowNonSerializedField]
    private bool isChasing; 

    private void Start()
    {
        if (player == null) 
            player = FindObjectOfType<FollowLeader>().transform;

        randomMovementCenter = transform.position;
        InvokeRepeating(nameof(ChangePatrolTarget), 0, randomMovementChangeDelay);
    }

    private void ChangePatrolTarget()
    {
        currentTarget = randomMovementCenter + Random.insideUnitCircle * randomMovementRange;
    }

    private void Update()
    {
        float squareDistanceToPlayer = (player.position - transform.position).sqrMagnitude;
        if (isChasing == false && squareDistanceToPlayer < playerDetectionRange * playerDetectionRange)
        {
            isChasing = true;
            //exclamationAnimation?.Play();
            CancelInvoke(nameof(ChangePatrolTarget));
        }
        else if (isChasing && squareDistanceToPlayer > escapeDistance * escapeDistance)
        {
            isChasing = false;
            InvokeRepeating(nameof(ChangePatrolTarget), 0, randomMovementChangeDelay);
        }

        float speed = isChasing ? chasingSpeed : patrollingSpeed;
        if (isChasing)
        {
            currentTarget = player.position;
        }
        
        var direction =  currentTarget - (Vector2)transform.position;
        if (direction.sqrMagnitude > 0.1f) 
        {
            if (direction.sqrMagnitude > 1)
                direction.Normalize();

            var animation = isChasing ? OfficerAnimation.Running : OfficerAnimation.Walking;
            SetAnimation(animation);
            animator.AnimationSpeed = Mathf.Max(SpritesetAnimator.idleAnimationSpeed, speed);
            float xDirection = direction.x;
            if (xDirection < 0)
                animator.SpriteRenderer.flipX = true;
            else if (xDirection > 0)
                animator.SpriteRenderer.flipX = false;
            
            rigidbody2d.velocity = direction * speed;
        }
        else
        {
            SetAnimation(OfficerAnimation.Standing);
            animator.AnimationSpeed = SpritesetAnimator.idleAnimationSpeed;
            rigidbody2d.velocity = Vector2.zero;
        }
    }

    private void SetAnimation(OfficerAnimation animation)
    {
        animator.SetAnimation((int)animation);
    }
}

