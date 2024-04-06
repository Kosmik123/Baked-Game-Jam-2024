using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class SpritesetAnimator : MonoBehaviour
{
    public const int idleAnimationSpeed = 4;

    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private int rowsCount = 33;
    [SerializeField]
    private int columnsCount = 4;

    [SerializeField]
    private float animationSpeed;
    public float AnimationSpeed
    {
        get => animationSpeed;
        set => animationSpeed = value;
    }

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    [SerializeField, ReadOnly]
    private List<Sprite> currentSequence;

    private int currentAnimationIndex = -1;
    private bool isReversed;

    private int currentIndex;
    private float animationTimer;

    private void Awake()
    {
        currentSequence = new List<Sprite>();
    }

    private void Start()
    {
        SetAnimation(0);
        animationTimer = 0;
    }

    public void SetAnimation(int animationIndex, bool reversed = false)
    {
        SetAnimation(animationIndex, columnsCount, reversed);
    }

    public void SetAnimation(int animationIndex, int overrideSequenceLenght, bool reversed = false)
    {
        if (currentAnimationIndex == animationIndex && overrideSequenceLenght == currentSequence.Count && isReversed == reversed)
            return;

        isReversed = reversed;
        currentAnimationIndex = animationIndex;
        currentSequence.Clear();
        int firstFrameIndex = animationIndex * columnsCount;
        for (int i = 0; i < overrideSequenceLenght; i++)
        {
            var relativeIndex = reversed ? (overrideSequenceLenght - 1 - i) : i;
            currentSequence.Add(sprites[firstFrameIndex + relativeIndex]);
        }
        currentIndex = 0;
    }

    private void Update()
    {
        animationTimer += Time.deltaTime * animationSpeed;
        if (animationTimer > 1)
        {
            animationTimer -= 1;
            currentIndex = (currentIndex + 1) % currentSequence.Count;
            spriteRenderer.sprite = currentSequence[currentIndex];
        }
    }
}
