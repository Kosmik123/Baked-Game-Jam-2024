using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class WorldGenerator : MonoBehaviour
{
    [SerializeField]
    private WorldGenerationSettings settings;
    [SerializeField]
    private Camera observerCamera;

    private Grid _grid;
    public Grid Grid
    {
        get 
        {
            if (_grid == null)
                _grid = GetComponent<Grid>();
            return _grid; 
        }
    }

    private readonly Dictionary<Vector2Int, WorldChunk> chunksByCoord = new Dictionary<Vector2Int, WorldChunk>();

    [SerializeField, ReadOnly]
    private readonly List<Vector2Int> coordsVisibleLastFrame = new List<Vector2Int>();

    private void Reset()
    {
        observerCamera = Camera.main;
    }

    private void Start()
    {
        UpdateChunks();
    }

    private void Update()
    {
        UpdateChunks();
    }

    private void UpdateChunks()
    {
        foreach (var position in coordsVisibleLastFrame)
            if (chunksByCoord.TryGetValue(position, out var chunk))
                if (chunk)
                    chunk.IsVisible = false;

        coordsVisibleLastFrame.Clear();

        float yExtent = observerCamera.orthographicSize;
        float xExtent = yExtent * observerCamera.aspect;
        var observerPosition = observerCamera.transform.position;
        var bottomLeft = WorldToGrid(new Vector2(
            Mathf.Floor(observerPosition.x - xExtent),
            Mathf.Floor(observerPosition.y - yExtent)));

        var topRight = WorldToGrid(new Vector2(
            Mathf.Ceil(observerPosition.x + xExtent),
            Mathf.Ceil(observerPosition.y + yExtent)));

        for (int j = bottomLeft.y - 1; j <= topRight.y + 1; j++)
        {
            for (int i = bottomLeft.x - 1; i <= topRight.x + 1; i++)
            {
                var coord = new Vector2Int(i, j);
                coordsVisibleLastFrame.Add(coord);
                if (chunksByCoord.TryGetValue(coord, out var chunk))
                {
                    if (chunk != null)
                        chunk.IsVisible = true;
                }
                else
                {
                    HandleEmptyCoord(coord);
                }
            }
        }
    }

    private void HandleEmptyCoord(Vector2Int coord)
    {
        var chunk = (Random.value < settings.SpawnProbability) ? CreateChunk(coord) : null;
        chunksByCoord.Add(coord, chunk);
    }

    private WorldChunk CreateChunk(Vector2Int coord)
    {
        var chunkPosition = GridToWorld(coord);
        var prototype = settings.GetChunkPrototype();
        var chunk = Instantiate(prototype, chunkPosition, Quaternion.identity, transform);
        chunk.IsVisible = true;
        chunk.name = $"{prototype.name} {coord}";
        return chunk;
    }

    public Vector2Int WorldToGrid(Vector3 position)
    {
        return (Vector2Int)Grid.WorldToCell(position);
    }

    public Vector2 GridToWorld(Vector2 grid)
    {
        Vector3 localPosition = Grid.CellToLocalInterpolated(grid);
        return transform.TransformPoint(localPosition);
    }
}
