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
    private float maxMoveSpeed;

    private void Start()
    {
        target = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, maxMoveSpeed * Time.deltaTime);
    }
}
