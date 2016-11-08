using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EndlessTerrain : MonoBehaviour
{

    public const float maxViewDst = 450;
    public Material material;
    public Transform viwer;

    public static Vector2 viewPosition;
    private static MapGenerator mapGenerator;
    int chunkSize;
    int chunksVisibleInViewDst;

    Dictionary<Vector2,TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>(); 
    List<TerrainChunk> terrainChunksLastVisibleUpdate = new List<TerrainChunk>(); 
    void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDst = Mathf.RoundToInt((maxViewDst/chunkSize));
    }

    void Update()
    {
        viewPosition = new Vector2(viwer.position.x,viwer.position.z);
        UpdateVisibleChunks();  
    }

    void UpdateVisibleChunks()
    {

        for (int i = 0; i < terrainChunksLastVisibleUpdate.Count; i++)
        {
            terrainChunksLastVisibleUpdate[i].SetVisible(false);
        }
        terrainChunksLastVisibleUpdate.Clear(); 
        int currentChunkCoordX = Mathf.RoundToInt(viewPosition.x/chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewPosition.y/chunkSize);
        for (int yOffset = -chunksVisibleInViewDst; yOffset <=chunksVisibleInViewDst ; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset,currentChunkCoordY + yOffset);
                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].Update();
                    if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
                    {
                        terrainChunksLastVisibleUpdate.Add((terrainChunkDictionary[viewedChunkCoord]));
                    }

                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCoord,new TerrainChunk(viewedChunkCoord,chunkSize,transform, material));
                }
            }


        }

    }

    public class TerrainChunk
    {
        GameObject meshObjcet;
        Vector2 position;
        private Bounds bounds;

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        public TerrainChunk(Vector2 coord, int size,Transform parent,Material mat)
        {
            position = coord * size;
            bounds = new Bounds(position,Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x,0,position.y);

            meshObjcet = new GameObject("Terrain Chunk ");
            meshRenderer = meshObjcet.AddComponent<MeshRenderer>();
            meshRenderer.material = mat;
            meshFilter = meshObjcet.AddComponent<MeshFilter>();
            meshObjcet.transform.position = positionV3;
            meshObjcet.transform.parent = parent;
            SetVisible(false);



            mapGenerator.RequestMapData(OnMapDataReceived);
           
        }

        void OnMapDataReceived(MapGenerator.MapData mapData)
        {
            //绘制地图
            mapGenerator.RequesMeshData(mapData,OnMeshDataReceived);
        }

        void OnMeshDataReceived(MeshData meshData)
        {
            //绘制地图网格
            meshFilter.mesh = meshData.CreateMesh();
        }
        public void Update()
        {
           float viewerDstFormNearestEdge = Mathf.Sqrt( bounds.SqrDistance(viewPosition));
            bool visible = viewerDstFormNearestEdge <= maxViewDst;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObjcet.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObjcet.activeSelf;
        }
    }
}
