using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private int viewRadius = 10;
    [SerializeField] private ChunkGenerator chunkPrefab;
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private Camera mainCamera;

    public static int seed = 321;
    
    private Dictionary<Vector2Int, ChunkData> ChunkDatas = new Dictionary<Vector2Int, ChunkData>(); 
    private Vector2Int currentplayerChunk;

    private void Start()
    {
        StartCoroutine(GenerateChunks(false));
    }

    private IEnumerator GenerateChunks(bool wait) //запуск создания чанков в зоне радиуса видимости
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

    private void LoadChunk(Vector2Int chunkPosition) //создание чанка
    {
        float xPos = chunkPosition.x * (ChunkInfo.Instance.ChunkWidth - 1);
        float zPos = chunkPosition.y * (ChunkInfo.Instance.ChunkWidth - 1);

        var chunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);

        var chunkData = new ChunkData(chunkPosition, terrainGenerator.GenerateChunkHeight(xPos, zPos), chunk.gameObject);
        chunk.ChunkData = chunkData;

        ChunkDatas.Add(chunkPosition, chunkData);    
    }

    private void Update()
    {
        Vector3Int playerWorldPos = Vector3Int.FloorToInt(mainCamera.transform.position);
        Vector2Int playerChunk = GetChunkContainingPlayer(playerWorldPos);
        if(playerChunk != currentplayerChunk)
        {
            currentplayerChunk = playerChunk;
            RemoveUnnecessaryChunks();
            StartCoroutine(GenerateChunks(true));          
        }
    }

    private void RemoveUnnecessaryChunks() // удаление чанков, находящихся вне зоны радиуса видимости
    {
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in ChunkDatas.Values)
        {
            if(Vector2Int.Distance(chunk.position, currentplayerChunk) > viewRadius + 2)
            {
                chunksToRemove.Add(chunk.position);
                Destroy(chunk.gameObj);
            }
        }
        foreach (var chunk in chunksToRemove)
        {
            ChunkDatas.Remove(chunk);
        }
    }

    private Vector2Int GetChunkContainingPlayer(Vector3Int worldPos) //получение чанка, на которм находится игрок
    {
        return new Vector2Int(worldPos.x / ChunkInfo.Instance.ChunkWidth, worldPos.z / ChunkInfo.Instance.ChunkWidth);
    }
}
