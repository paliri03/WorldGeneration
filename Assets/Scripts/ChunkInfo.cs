using System.Collections.Generic;
using UnityEngine;

public class ChunkInfo : Singleton<ChunkInfo>
{
    [SerializeField] private int chunkWidth;
    [SerializeField] private List<GameObject> treePrefabs;
    [SerializeField] private int treeFrequency;
    [SerializeField] private List<GameObject> decorPrefabs;
    [SerializeField] private int decorFrequency;
    [SerializeField] private List<GameObject> grassPrefabs;
    [SerializeField] private int grassFrequency;


    public int ChunkWidth => chunkWidth;
    public List<GameObject> ChunkObjects => treePrefabs;
    public int TreeFrequency => treeFrequency;
    public List <GameObject> DecorObjects => decorPrefabs;
    public int DecorFrequency => decorFrequency;
    public List<GameObject> GrassObjects => grassPrefabs;
    public int GrassFrequency => grassFrequency;
}
