using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour
{


    public Renderer texturerRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width,height);

        Color[] colourMap = new Color[width*height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y*width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        texture.SetPixels(colourMap);
        texture.Apply();

        texturerRenderer.sharedMaterial.mainTexture = texture;
        texturerRenderer.transform.localScale = new Vector3(width,1,height);
    }

    public void DrawTexture(Texture2D texture)
    {
        texturerRenderer.sharedMaterial.mainTexture = texture;
        texturerRenderer.transform.localScale = new Vector3(texture.width,1,texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }


}
