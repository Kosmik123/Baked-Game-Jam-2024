using UnityEngine;

public class ChunkFloor : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    private float probability = 0.5f;

    private void Awake()
    {
        gameObject.SetActive(Random.value < probability);
    }
}
