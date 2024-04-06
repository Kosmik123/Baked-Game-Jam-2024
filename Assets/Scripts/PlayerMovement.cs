using NaughtyAttributes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 6;

    private Camera viewCamera;

    [ShowNonSerializedField]
    private Vector2 movementTarget;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(movementTarget, 0.2f);
        Gizmos.DrawLine(transform.position, movementTarget);
    }
}
