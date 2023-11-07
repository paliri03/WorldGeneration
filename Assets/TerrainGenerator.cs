using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public BlockType[,,] Generate(float xOffset, float zOffset)
    {
        var result = new BlockType[ChunkGenerator.chunkWidth, ChunkGenerator.chunkHeight, ChunkGenerator.chunkWidth];

        for(int x = 0; x < ChunkGenerator.chunkWidth; x++)
        {
            for(int z = 0; z < ChunkGenerator.chunkWidth; z++)
            {
                float height = Mathf.PerlinNoise((x + xOffset) * 0.2f, (z + zOffset) * 0.2f) * 5 + 10;

                for(int y = 0; y < height; y++)
                {
                    result[x, y, z] = BlockType.Grass;
                }
            }
        }

        return result;
    }
}
