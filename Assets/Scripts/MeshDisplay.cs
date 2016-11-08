using UnityEngine;
using System.Collections;

/// <summary>
/// Utility class that let you see normals and tangent vectors for a mesh.
/// This is really useful when debugging mesh appearance
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class MeshDisplay : MonoBehaviour
{
    public bool showNormals = true;
    public bool showTangents = true;
    public bool showBounds;
    public bool showAllVertexs;
    public float displayLengthScale = 1.0f;

    public Color normalColor = Color.red;
    public Color tangentColor = Color.blue;
    public Color BoundsColor = Color.green;

    void OnDrawGizmosSelected()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogWarning("Cannot find MeshFilter");
            return;
        }
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            Debug.LogWarning("Cannot find mesh");
            return;
        }
        bool doShowNormals = showNormals && mesh.normals.Length == mesh.vertices.Length;
        bool doShowTangents = showTangents && mesh.tangents.Length == mesh.vertices.Length;

        foreach (int idx in mesh.triangles)
        {
            Vector3 vertex = transform.TransformPoint(mesh.vertices[idx]);

            if (doShowNormals)
            {
                Vector3 normal = transform.TransformDirection(mesh.normals[idx]);
                Gizmos.color = normalColor;
                Gizmos.DrawLine(vertex, vertex + normal * displayLengthScale);
            }
            if (doShowTangents)
            {
                Vector3 tangent = transform.TransformDirection(mesh.tangents[idx]);
                Gizmos.color = tangentColor;
                Gizmos.DrawLine(vertex, vertex + tangent * displayLengthScale);
            }

            if (showBounds)
            {
                Gizmos.color = BoundsColor;
                Gizmos.DrawCube(mesh.bounds.center, mesh.bounds.size);
            }

            if (showAllVertexs)
            {
                Gizmos.color = Color.green;
                foreach (Vector3 v in mesh.vertices)
                {
                    Gizmos.DrawCube(v,new Vector3(0.05f,0.05f,0.05f));
                }
            }
        }
    }
}