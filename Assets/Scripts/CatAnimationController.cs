using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    [SerializeField]
    private SpritesetAnimator animator;
    [SerializeField]
    private CatCharacter catCharacter;

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
            animator.AnimationSpeed = catCharacter.MoveSpeed;
            SetAnimation(CatAnimation.Jumping);
        }
        else 
        {
            animator.AnimationSpeed = 4;
            SetAnimation(CatAnimation.Standing);
        }
    }

    private void SetAnimation(CatAnimation animation)
    {
        animator.SetAnimation((int)animation);
    }
}



