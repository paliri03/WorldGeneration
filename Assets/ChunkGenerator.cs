using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public const int chunkWidth = 10;
    public const int chunkHeight = 32;//128
    public const float blockScale = 1;

    public ChunkData chunkData;
    public GameWorld parentWorld;

    private List<Vector3> verticies = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        Mesh chunkMesh = new Mesh();

        for(int y = 0; y < chunkHeight; y++)
            for(int x = 0; x < chunkWidth; x++)
                for(var z = 0; z < chunkWidth; z++)
                    GenerateBlock(x, y, z);


        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize(); // будет дольше грузиться, но в итоге лучше работать

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    private void GenerateBlock(int x, int y, int z)
    {
        var blockPosition = new Vector3Int(x, y, z);

        var blockType = GetBlockAtPosition(blockPosition);
        if (blockType == BlockType.Air) return;


        if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0)
        {
            GenerateRightSide(blockPosition);
            AddUVs(blockType, Vector2Int.right);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0)
        {
            GenerateLeftSide(blockPosition);
            AddUVs(blockType, Vector2Int.right);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0)
        {
            GenerateFrontSide(blockPosition);
            AddUVs(blockType, Vector2Int.right);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0)
        { 
            GenerateBackSide(blockPosition);
            AddUVs(blockType, Vector2Int.right);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0)
        { 
            GenerateTopSide(blockPosition);
            AddUVs(blockType, Vector2Int.up);
        }
        if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0)
        {
            GenerateBottomSide(blockPosition);
            AddUVs(blockType, Vector2Int.down);
        }
    }

    private BlockType GetBlockAtPosition(Vector3Int blockPosition)
    {
        if(blockPosition.x >= 0 && blockPosition.x < chunkWidth && blockPosition.y >= 0 && blockPosition.y < chunkHeight && blockPosition.z >= 0 && blockPosition.z < chunkWidth)
        {
            return chunkData.blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            if (blockPosition.y < 0 || blockPosition.y >= chunkWidth) 
                return BlockType.Air;

            var neighborChunkPosition = chunkData.chunkPosition;

            if(blockPosition.x < 0)
            {
                neighborChunkPosition.x--;
                blockPosition.x += chunkWidth;
            }
            else if(blockPosition.x >= chunkWidth)
            {
                neighborChunkPosition.x++;
                blockPosition.x -= chunkWidth;
            }

            if (blockPosition.z < 0)
            {
                neighborChunkPosition.y--;
                blockPosition.z += chunkWidth;
            }
            else if (blockPosition.z >= chunkWidth)
            {
                neighborChunkPosition.y++;
                blockPosition.z -= chunkWidth;
            }

            if (parentWorld.ChunkDatas.TryGetValue(neighborChunkPosition, out ChunkData neighborChunk))
            {
                return neighborChunk.blocks[blockPosition.x, blockPosition.y, blockPosition.z];
            }
            else
            {
                return BlockType.Air;
            }
        }
    }

    private void GenerateRightSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * blockScale);

        AddLastVerticiesSquare();
    }
    private void GenerateLeftSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * blockScale);
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * blockScale);


        AddLastVerticiesSquare();
    }
    private void GenerateFrontSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * blockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * blockScale);


        AddLastVerticiesSquare();
    }
    private void GenerateBackSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * blockScale);

        AddLastVerticiesSquare();
    }
    private void GenerateTopSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * blockScale);

        AddLastVerticiesSquare();
    }
    private void GenerateBottomSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * blockScale);
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * blockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * blockScale);

        AddLastVerticiesSquare();
    }

    private void AddLastVerticiesSquare()
    {
        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);

        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 1);
        triangles.Add(verticies.Count - 2);
    }

    private void AddUVs(BlockType blockType, Vector2Int normal)
    {
        Vector2 uv;

        if (blockType == BlockType.Grass)
        {
            uv = normal == Vector2Int.up ? new Vector2(256 / 2048f, 768 / 2048f) :
                normal == Vector2Int.down ? new Vector2(256 / 2048f, 1920 / 2048f) :
                                             new Vector2(384 / 2048f, 1920 / 2048f);
        }
        else
        {
            uv = new Vector2(1, 1); // для ошибки
        }

        for(int i = 0; i < 4; i++)
        {
            uvs.Add(uv);
        }
    }
}
