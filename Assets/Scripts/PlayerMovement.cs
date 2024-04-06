using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 6;

    private Camera viewCamera;

    [ShowNonSerializedField]
    private Vector2 movementTarget;

    private Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody2D>();
            return _rigidbody;
        }
    }

    private void Awake()
    {
        viewCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var screenPosition = Input.mousePosition;
            movementTarget = viewCamera.ScreenToWorldPoint(screenPosition);
        }
    }
    private void FixedUpdate()
    {
        var direction = movementTarget - (Vector2)transform.position;
        if (direction.sqrMagnitude > 1)
            direction.Normalize();
        
        Rigidbody.velocity = direction * moveSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(movementTarget, 0.2f);
        Gizmos.DrawLine(transform.position, movementTarget);
    }
}
