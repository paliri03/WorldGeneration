using UnityEngine;
public class ChunkData //данные каждого чанка
{
    public readonly Vector2Int position; //расположение в мире
    public readonly float[,] heightMap; //карта высот
    public readonly GameObject gameObj;

    public ChunkData(Vector2Int chunkPosition, float[,] heightMap, GameObject gameObj)
    {
        this.position = chunkPosition;
        this.heightMap = heightMap;
        this.gameObj = gameObj;
    }
}
