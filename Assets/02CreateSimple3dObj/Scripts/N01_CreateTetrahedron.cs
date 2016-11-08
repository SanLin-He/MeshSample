using UnityEngine;
using System.Collections;

//https://blog.nobel-joergensen.com/2010/12/25/procedural-generated-mesh-in-unity/

//shading is wrong 
//The problem is that the shading uses the vertex normals – and there is one vertex normal for each vertex
//In this case we want sharp edges and do not want to share vertex normals. This can be done by not sharing the vertices. If you are creating a mesh without sharp edges, you should use shared normals
public class N01_CreateTetrahedron : MonoBehaviour {

	void Start () {
        CreateObj.CreateObjByMesh("四面体", CreateTetrahedronMesh());
	}
	


    //创建四面体(4个顶点，4个面)
    Mesh CreateTetrahedronMesh()
    {
        Mesh mesh = new Mesh();
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(1, 0, 0);
        Vector3 p2 = new Vector3(0.5f,0,Mathf.Sqrt(0.75f));
        Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);

        mesh.Clear();
        mesh.vertices = new Vector3[]{p0,p1,p2,p3};
        //The triangle must be defined using clockwise polygon winding order – this is used for backface culling  (usually only the frontside of every triangle is draw)
        mesh.triangles = new int[]
        {
            0,1,2,
            0,2,3,
            2,1,3,
            0,3,1
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;

    }
}
