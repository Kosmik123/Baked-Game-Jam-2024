using UnityEngine;

public class FollowerController : MonoBehaviour
{
    private Vector3 target;
    public Vector3 Target
    {
        get => target; 
        set => target = value;
    }

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    private void Start()
    {
        target = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }
}
