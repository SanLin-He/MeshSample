using UnityEngine;
using System.Collections;
//https://blog.nobel-joergensen.com/2011/04/05/procedural-generated-mesh-in-unity-part-2-with-uv-mapping/

//UV mapping 基础：
//1.所谓UV mapping指的是3D物体的表面与2D文理图片的坐标映射
//2.3D物体mesh的每个顶点都有个UV坐标（地球仪上经纬线）
//3.UV坐标体系中（0.0,0.0）对应文理图片左下角，(1.0,1.0)对应文理图片右上角
//4.如果坐标超出了0~1的范围，那么坐标的算法要么a.clamped,b.repeated the textur(这取决于 texture的import setting)

//与shader计算相关的顶点normal一样，每个顶点对应于一个uv coordinates,这就意味着硬边顶点可能会对应多个uv coordinates（有时几个顶点对应于一个uv coordinates）
public class N04_CreateTetrahedronUVMappng : MonoBehaviour
{

   
    void Start()
    {
        CreateObj.CreateObjByMesh("四面体有贴图", UvMappingMesh());
    }

    //
    Mesh UvMappingMesh()
    {


        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(1, 0, 0);
        Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
        Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);

        Mesh mesh = new Mesh();
        mesh.Clear();

        
        mesh.vertices = new Vector3[4*3]//四个顶点，每个顶点使用三次
        {
            p0,p1,p2, 
            p0,p2,p3,
            p0,p3,p1,
            p1,p3,p2
        };
        //注意左手螺旋，顺时针顺。这里是依次排序，那么在顶点的设置时，就将顺序额填好
        mesh.triangles = new int[4*3]//四个面，每个面三个顶点
        {
            0,1,2,
            3,4,5,
            6,7,8,
            9,10,11
        };

        //注意uv的对应
        Vector2 uv3a = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(0.5f, 0);
        Vector2 uv0 = new Vector2(0.25f, Mathf.Sqrt(0.75f) / 2);
        Vector2 uv2 = new Vector2(0.75f, Mathf.Sqrt(0.75f) / 2);
        Vector2 uv3b = new Vector2(0.5f, Mathf.Sqrt(0.75f));
        Vector2 uv3c = new Vector2(1, 0);

        mesh.uv = new Vector2[]//这里与顶点的数量一样
        {
            uv0,uv1,uv2,
            uv0,uv2,uv3b,
            uv0,uv1,uv3a,
            uv1,uv2,uv3c
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;
    }
}
