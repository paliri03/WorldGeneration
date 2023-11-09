using UnityEngine;
public class ChunkData //������ ������� �����
{
    public readonly Vector2Int position; //������������ � ����
    public readonly float[,] heightMap; //����� �����
    public readonly GameObject gameObj;

    public ChunkData(Vector2Int chunkPosition, float[,] heightMap, GameObject gameObj)
    {
        this.position = chunkPosition;
        this.heightMap = heightMap;
        this.gameObj = gameObj;
    }
}
