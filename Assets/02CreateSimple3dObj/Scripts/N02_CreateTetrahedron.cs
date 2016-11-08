using UnityEngine;
using System.Collections;

//shading is correct
public class N02_CreateTetrahedron : MonoBehaviour
{

    void Start()
    {
        CreateObj.CreateObjByMesh("四面体", CreateTetrahedronMesh());
    }



    //创建四面体(4个顶点，4个面)
    Mesh CreateTetrahedronMesh()
    {
        Mesh mesh = new Mesh();
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(1, 0, 0);
        Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
        Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);

        mesh.Clear();
        //左手法则，拇指normal放下过，四指卷曲为顺时针方向
        mesh.vertices = new Vector3[4 * 3]
        {
            p0,p1,p2, 
            p0,p2,p3,
            p0,p3,p1,
            p1,p3,p2
        };
        //The triangle must be defined using clockwise polygon winding order – this is used for backface culling  (usually only the frontside of every triangle is draw)
        mesh.triangles = new int[4 * 3]
        {
             0,1,2,
             3,4,5,
             6,7,8,
             9,10,11
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;

    }
}
