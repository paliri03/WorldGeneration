using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private float scaleNoise;
    [SerializeField] private float amplitude;
    public float[,] GenerateChunkHeight(float xOffset, float zOffset)//генерация карты высот для чанка
    {
        var result = new float[ChunkInfo.Instance.ChunkWidth, ChunkInfo.Instance.ChunkWidth];

        for(int x = 0; x < ChunkInfo.Instance.ChunkWidth; x++)
        {
            for(int z = 0; z < ChunkInfo.Instance.ChunkWidth; z++)
            {
                float height = Mathf.PerlinNoise((x + xOffset) * scaleNoise, (z + zOffset) * scaleNoise) * amplitude;
                result[x, z] = height;
            }
        }
        return result;
    }
}
