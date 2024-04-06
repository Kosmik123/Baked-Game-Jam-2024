using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 6;

    [SerializeField, Tooltip("Saved positions count per unit")]
    private int pathResolution = 10;

    private Camera viewCamera;

    [ShowNonSerializedField]
    private Vector2 movementTarget;

    [SerializeField, ReadOnly]
    private List<Vector3> pathBehindPlayer = new List<Vector3>();

    [SerializeField]
    public int TeamSize => teamMembers.Length;
    public int MaxPathLength => pathResolution * TeamSize + 1;

    [SerializeField]
    private FollowerController[] teamMembers;

    private void Awake()
    {
        viewCamera = Camera.main;
    }

    private void Start()
    {
        pathBehindPlayer.Add(transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var screenPosition = Input.mousePosition;
            movementTarget = viewCamera.ScreenToWorldPoint(screenPosition);
        }

        var currentPosition = transform.position;
        if (Vector2.Distance(currentPosition, pathBehindPlayer.Last()) > 1f / pathResolution) 
        {
            pathBehindPlayer.Add(currentPosition);
            if (pathBehindPlayer.Count > MaxPathLength)
            {
                pathBehindPlayer.RemoveAt(0);
            }
        }

        for (int i = 0; i < teamMembers.Length; i++)
        {
            var pathIndex = pathBehindPlayer.Count - pathResolution * (i + 1);
            if (pathIndex > 0 && pathIndex < pathBehindPlayer.Count )
                teamMembers[i].Target = pathBehindPlayer[pathIndex];
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(movementTarget, 0.2f);
        Gizmos.DrawLine(transform.position, movementTarget);

        Gizmos.color = Color.gray;
        for (int i = 1; i < pathBehindPlayer.Count; i++) 
        {
            Gizmos.DrawLine(pathBehindPlayer[i - 1], pathBehindPlayer[i]);
        }
    }
}
