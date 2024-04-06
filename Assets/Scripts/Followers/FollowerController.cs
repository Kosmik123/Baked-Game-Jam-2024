using NaughtyAttributes;
using UnityEngine;

public class FollowerController : CatCharacter
{
    private Vector3 target;
    public Vector3 Target
    {
        get => target; 
        set => target = value;
    }

    [ShowNonSerializedField]
    private Vector3 currentVelocity;
    public override Vector2 Velocity => currentVelocity;
    private void Start()
    {
        target = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, MoveSpeed * Time.deltaTime);
    }
}


