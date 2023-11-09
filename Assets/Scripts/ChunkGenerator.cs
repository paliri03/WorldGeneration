using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour  //создание чанка
{
    private ChunkData chunkData;

    private List<Vector3> verticies = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private System.Random rand;

    public ChunkData ChunkData { set { chunkData = value; } }

    void Start()
    {
        GenerateMesh(); //создание меша

        SetChunkSeed(); //установка сида для дальнейшей генерации для данного чанка
        GenerateObjects(); //добавление различных объектов
    }

    private void GenerateMesh()
    {
        Mesh chunkMesh = new Mesh();
        GenerateVerticies();
        GenerateTriangles();

        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize(); 

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }
    private void GenerateVerticies()
    {
        for (int z = 0; z < ChunkInfo.Instance.ChunkWidth; z++)
            for (var x = 0; x < ChunkInfo.Instance.ChunkWidth; x++)
            {
                verticies.Add(new Vector3(x, chunkData.heightMap[x, z], z));
            }
    }
    private void GenerateTriangles()
    {
        var chunkWidth = ChunkInfo.Instance.ChunkWidth;
        for (int z = 0; z < chunkWidth; z ++)
            for (var x = 0; x < chunkWidth - 1; x ++)
            {
                if (z < chunkWidth - 1 && z > 0)
                {
                    triangles.Add(x  + z * chunkWidth);                                     
                    triangles.Add(x + (z + 1) * chunkWidth);
                    triangles.Add((x + 1) + z * chunkWidth);

                    triangles.Add(x + z * chunkWidth);
                    triangles.Add((x + 1) + z * chunkWidth);
                    triangles.Add((x + 1)  + (z - 1) * chunkWidth);                   
                }
                else if (z < chunkWidth - 1)
                {
                    triangles.Add(x + z * chunkWidth);                  
                    triangles.Add(x + (z + 1) * chunkWidth);
                    triangles.Add((x + 1) + z * chunkWidth);
                }
                else
                {
                    triangles.Add(x + z * chunkWidth);                    
                    triangles.Add((x + 1) + z * chunkWidth);
                    triangles.Add((x + 1) + (z - 1) * chunkWidth);
                }
            }
    }

    private void SetChunkSeed()
    {
        var chunkSeed = $"{GameWorld.seed}{chunkData.position.x}{chunkData.position.y}";
        chunkSeed = chunkSeed.Replace('-', '0');
        rand = new System.Random(int.Parse(chunkSeed));
    }

    private void GenerateObjects()
    {
        for(int i = 0; i < ChunkInfo.Instance.TreeFrequency; i++)
            AddObject(ChunkInfo.Instance.ChunkObjects, 0.2f);
        for (int i = 0; i < ChunkInfo.Instance.DecorFrequency; i++)
            AddObject(ChunkInfo.Instance.DecorObjects, 0);
        for (int i = 0; i < ChunkInfo.Instance.GrassFrequency; i++)
            AddObject(ChunkInfo.Instance.GrassObjects, 0);
    }
    private void AddObject(List<GameObject> prefabs, float yOffset)
    {
        var x = (float)rand.NextDouble() * 20 + chunkData.position.x * (ChunkInfo.Instance.ChunkWidth - 1);
        var z = (float)rand.NextDouble() * 20 + chunkData.position.y * (ChunkInfo.Instance.ChunkWidth - 1);
        var y = GetHeight(x, z);

        if (y.HasValue)
        {
            var i = rand.Next(0, prefabs.Count);
            Instantiate(prefabs[i], new Vector3(x, y.Value - yOffset, z), Quaternion.identity, transform);
        }
    }

    private float? GetHeight(float x, float z)
    {
        if(Physics.Raycast(new Vector3(x, 30, z), Vector3.down * 10, out RaycastHit hit, 30, LayerMask.GetMask("Ground")))
        {
            return hit.point.y;
        }
        else
            return null;
    }
}
