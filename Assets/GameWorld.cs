using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    private const int viewRadius = 5;

    public Dictionary<Vector2Int, ChunkData> ChunkDatas = new Dictionary<Vector2Int, ChunkData>();
    public ChunkGenerator chunkPrefab;
    public TerrainGenerator terrainGenerator;

    public Camera mainCamera;
    private Vector2Int currentplayerChunk;

    private void Start()
    {
        StartCoroutine(Generate(false));
    }

    private IEnumerator Generate(bool wait)
    {
        for (int x = currentplayerChunk.x - viewRadius; x < currentplayerChunk.x + viewRadius; x++)
        {
            for (int y = currentplayerChunk.y - viewRadius; y < currentplayerChunk.y + viewRadius; y++)
            {
                Vector2Int chunkPosition = new Vector2Int(x, y);
                if (ChunkDatas.ContainsKey(chunkPosition)) continue;

                LoadChunk(chunkPosition);

                if (wait) yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void LoadChunk(Vector2Int chunkPosition)
    {
        float xPos = chunkPosition.x * ChunkGenerator.chunkWidth * ChunkGenerator.blockScale;
        float zPos = chunkPosition.y * ChunkGenerator.chunkWidth * ChunkGenerator.blockScale;


        var chunkData = new ChunkData();
        chunkData.chunkPosition = chunkPosition;
        chunkData.blocks = terrainGenerator.Generate(xPos, zPos);
        ChunkDatas.Add(chunkPosition, chunkData);

        var chunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
        chunk.chunkData = chunkData;
        chunk.parentWorld = this;
    }

    private void Update()
    {
        Vector3Int playerWorldPos = Vector3Int.FloorToInt(mainCamera.transform.position / ChunkGenerator.blockScale);
        Vector2Int playerChunk = GetChunkContainingPlayer(playerWorldPos);
        if(playerChunk != currentplayerChunk)
        {
            currentplayerChunk = playerChunk;
            StartCoroutine(Generate(true));
        }
    }

    private Vector2Int GetChunkContainingPlayer(Vector3Int blockWorldPos)
    {
        return new Vector2Int(blockWorldPos.x / ChunkGenerator.chunkWidth, blockWorldPos.z / ChunkGenerator.chunkWidth);
    }
}
