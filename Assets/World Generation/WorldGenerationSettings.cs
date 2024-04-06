using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorldGenerationSettings : ScriptableObject
{
    [SerializeField, Range(0, 1)]
    private float spawnProbability = 0.5f;
    public float SpawnProbability => spawnProbability;

    [SerializeField]
    private List<ChunkChance> chunkChances = new List<ChunkChance>();

    private List<WorldChunk> chunksPrototypes;
    public IReadOnlyList<WorldChunk> ChunksPrototypes
    {
        get
        {
            if (chunksPrototypes == null || chunksPrototypes.Count <= 0)
                InitializedChunksPrototypesList();

            return chunksPrototypes;
        }
    }

    private void InitializedChunksPrototypesList()
    {
        chunksPrototypes = new List<WorldChunk>();
        foreach (var chunkChance in chunkChances)
            for (int i = 0; i < chunkChance.Chance; i++)
                chunksPrototypes.Add(chunkChance.ChunkPrototype);
    }
}

[System.Serializable]
public class ChunkChance
{
    [field: SerializeField]
    public WorldChunk ChunkPrototype { get; private set; }

    [field: SerializeField, Min(0)]
    public int Chance { get; private set; } = 1;
}