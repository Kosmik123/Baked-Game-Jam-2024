using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    [SerializeField]
    private SpritesetAnimator animator;
    [SerializeField]
    private CatCharacter catCharacter;

    private const float idleAnimationSpeed = 4;
    
    private void Update()
    {
        float xSpeed = catCharacter.Velocity.x;
        if (xSpeed > 0) 
        {
            animator.SpriteRenderer.flipX = false;
        }
        else if (xSpeed < 0)
        {
            animator.SpriteRenderer.flipX = true;
        }

        float speed = catCharacter.Velocity.magnitude;
        if (speed > 0.01f)
        {
            animator.AnimationSpeed = Mathf.Max(idleAnimationSpeed, catCharacter.MoveSpeed);
            SetAnimation(CatAnimation.Jumping);
        }
        else 
        {
            animator.AnimationSpeed = idleAnimationSpeed;
            SetAnimation(CatAnimation.Standing);
        }
    }

    private void SetAnimation(CatAnimation animation)
    {
        animator.SetAnimation((int)animation);
    }
}



