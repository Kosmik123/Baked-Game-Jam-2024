using NaughtyAttributes;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
    }

    public enum AttackType
    {
        Meelee,
        Nightstick,
        Ranged,
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
    [SerializeField]
    private float randomMovementChangeDelay;
    [SerializeField]
    private float patrollingSpeed;
    [SerializeField]
    private float patrollingAnimationSpeedModifier = 1;
    [ShowNonSerializedField]
    private Vector2 randomMovementCenter;

    [Header("Detection")]
    [SerializeField]
    private float playerDetectionRange;
    [SerializeField]
    private Animation exclamationAnimation;

    [Header("Chasing")]
    [SerializeField]
    private float chasingSpeed;
    [SerializeField]
    private float chasingAnimationSpeedModifier = 1;
    [SerializeField]
    private float escapeDistance;
    [ShowNonSerializedField]
    private bool isChasing;

    [Header("Attacking")]
    [SerializeField]
    private AttackType attackType;
    [SerializeField]
    private float attackRange = 2;
    [SerializeField]
    private float attackCooldown = 5;
    [ShowNonSerializedField]
    private float attackCooldownTimer;
    [SerializeField]
    private Transform rangedAttackTarget; // cat member
    [SerializeField]
    private LineRenderer shootingLine;

    [ShowNonSerializedField]
    private bool isAttacking;

    private Collider2D[] detectedMeeleeColliders = new Collider2D[4];

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
        if (isAttacking)
            return;

        float squareDistanceToPlayer = (player.position - transform.position).sqrMagnitude;
        if (isChasing == false && squareDistanceToPlayer < playerDetectionRange * playerDetectionRange)
        {
            isChasing = true;
            if (attackType == AttackType.Ranged)
                rangedAttackTarget = player;  // potem to ma byæ cat member

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
        
        var direction = currentTarget - (Vector2)transform.position;
        if (direction.sqrMagnitude > 0.1f) 
        {
            if (direction.sqrMagnitude > 1)
                direction.Normalize();

            float animationSpeedModifier = isChasing ? chasingAnimationSpeedModifier : patrollingAnimationSpeedModifier;
            var animation = OfficerAnimation.Walking;
            if (isChasing)
            {
                switch (attackType)
                {
                    case AttackType.Meelee:
                        animation = OfficerAnimation.Running;
                        break;
                    case AttackType.Nightstick:
                        animation = OfficerAnimation.RunningWithNightstick;
                        break;
                    case AttackType.Ranged:
                        var relativePosition = rangedAttackTarget.position - transform.position;
                        float shootingLineY = 1.225f;
                        if (Mathf.Abs(relativePosition.y) < Mathf.Abs(relativePosition.x))
                        {
                            animation = OfficerAnimation.RunningWithGunFront;
                        }    
                        else if (relativePosition.y > 0)
                        {
                            shootingLineY = 1.68f;
                            animation = OfficerAnimation.RunningWithGunUp;
                        }
                        else
                        {
                            shootingLineY = 0.62f;
                            animation = OfficerAnimation.RunningWithGunDown;
                        }

                        float shootingLineX = 0.625f;
                        if (animator.SpriteRenderer.flipX)
                            shootingLineX *= -1;
                        if (shootingLine)   
                            shootingLine.transform.localPosition = new Vector3(shootingLineX, shootingLineY);
    
                        break;
                }
            }
            else
            {
                switch (attackType)
                {
                    case AttackType.Nightstick:
                        animation = OfficerAnimation.WalkingWithNightstick;
                        break;
                    case AttackType.Ranged:
                        animation = OfficerAnimation.WalkingWithGunFront;
                        break;
                }
            }

            SetAnimation(animation);
            animator.AnimationSpeed = animationSpeedModifier * Mathf.Max(SpritesetAnimator.idleAnimationSpeed, speed);
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

        if (isChasing)
        {
            attackCooldownTimer += Time.deltaTime;
            if (CanAttack())
            {
                attackCooldownTimer = 0;
                Attack();
            }
        }
    }

    private void SetAnimation(OfficerAnimation animation)
    {
        animator.SetAnimation((int)animation);
    }

    public bool CanAttack()
    {
        if (attackCooldownTimer < attackCooldown)
            return false;

        if (rangedAttackTarget && Vector2.Distance(rangedAttackTarget.position, transform.position) > attackRange)
            return false;

        return true;
    }

    private void Attack()
    {
        rigidbody2d.velocity = Vector2.zero;

        isAttacking = true;
        if (attackType == AttackType.Ranged)
        {
            if (shootingLine)
            {
                shootingLine.SetPosition(0, shootingLine.transform.position);
                shootingLine.SetPosition(1, rangedAttackTarget.position);
                shootingLine.enabled = true;
            }
        }
        else
        {
            var animation = OfficerAnimation.AttackingWithNightstick;
            if (attackType == AttackType.Meelee)
            {
                float randomValue = Random.value;
                animation = randomValue > 0.75f ? OfficerAnimation.KarateKick : randomValue > 0.5f ? OfficerAnimation.Kicking : OfficerAnimation.Punching;
            }

            int count = Physics2D.OverlapCircleNonAlloc(transform.position, attackRange, detectedMeeleeColliders);
            for (int i = 0; i < count; i++)
            {

                // detectedMeeleeColliders[i].TryGetComponent<CatMember>().Damage();
            }
            SetAnimation(animation);
        }
        Invoke(nameof(EndAttacking), 0.3f);
    }

    private void EndAttacking()
    {
        if (shootingLine)
            shootingLine.enabled = false;

        isAttacking = false;
    }

}
