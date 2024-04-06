using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationSettings : ScriptableObject
{
    [SerializeField, Range(0, 1)]
    private float spawnProbability = 0.5f;
    public float SpawnProbability => spawnProbability;

    [SerializeField]
    private List<ChunkChance> chunkChances = new List<ChunkChance>();

    private List<WorldChunk> chunksPrototypesList = null;

    public WorldChunk GetChunkPrototype()
    {
        if (chunksPrototypesList == null || chunksPrototypesList.Count <= 0)
            InitializedChunksPrototypesList();

        return chunksPrototypesList[Random.Range(0, chunksPrototypesList.Count)];
    }

    private void InitializedChunksPrototypesList()
    {
        chunksPrototypesList = new List<WorldChunk>();
        foreach (var chunkChance in chunkChances)
            for (int i = 0; i < chunkChance.Chance; i++)
                chunksPrototypesList.Add(chunkChance.ChunkPrototype);
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