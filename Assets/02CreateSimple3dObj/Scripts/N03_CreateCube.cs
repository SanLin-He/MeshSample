using UnityEngine;
using System.Collections;

public class N03_CreateCube : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {
        CreateObj.CreateObjByMesh("立方体", CreateCubeMesh());
    }

    // Update is called once per frame
    void Update()
    {

    }


    Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices;         // 模型顶点坐标数组  
        int[] triangles;      // 三角形的点序列

        //为什么需要24个顶点和36个顶点索引
        //Unity3D的Mesh.triangles是三角形索引数组，不仅依靠这个索引值索引三角形顶点坐标，而且索引纹理坐标，索引法线向量。即正方体的每个顶点都参与了3个平面，而这3个平面的法线向量是不同的，该顶点在渲染这3个平面的时候需要索引到不同的法线向量。而由于顶点坐标和法线向量是由同一个索引值triangles[Index]取得的，例如，根据triangles[0],triangles[14],triangles[17]在vertices中索引到的顶点都为（0.5，－0.5，0.5），但是在normals中索引到的法向量值各不相同。这就决定了在正方体中一个顶点，需要有3份存储。（如果你需要创建其它模型，需要根据实际情况决定顶点坐标的冗余度。实质上顶点坐标的冗余正是方便了法线坐标、纹理坐标的存取。
        vertices = new Vector3[24];
        triangles = new int[36];

        //物体pivot居于中心
        //forward
        vertices[0].Set(0.5f, -0.5f, 0.5f);
        vertices[1].Set(-0.5f, -0.5f, 0.5f);
        vertices[2].Set(0.5f, 0.5f, 0.5f);
        vertices[3].Set(-0.5f, 0.5f, 0.5f);
        //back
        vertices[4].Set(vertices[2].x, vertices[2].y, -0.5f);
        vertices[5].Set(vertices[3].x, vertices[3].y, -0.5f);
        vertices[6].Set(vertices[0].x, vertices[0].y, -0.5f);
        vertices[7].Set(vertices[1].x, vertices[1].y, -0.5f);
        //up
        vertices[8] = vertices[2];
        vertices[9] = vertices[3];
        vertices[10] = vertices[4];
        vertices[11] = vertices[5];
        //down
        vertices[12].Set(vertices[10].x, -0.5f, vertices[10].z);
        vertices[13].Set(vertices[11].x, -0.5f, vertices[11].z);
        vertices[14].Set(vertices[8].x, -0.5f, vertices[8].z);
        vertices[15].Set(vertices[9].x, -0.5f, vertices[9].z);
        //right
        vertices[16] = vertices[6];
        vertices[17] = vertices[0];
        vertices[18] = vertices[4];
        vertices[19] = vertices[2];
        //left
        vertices[20].Set(-0.5f, vertices[18].y, vertices[18].z);
        vertices[21].Set(-0.5f, vertices[19].y, vertices[19].z);
        vertices[22].Set(-0.5f, vertices[16].y, vertices[16].z);
        vertices[23].Set(-0.5f, vertices[17].y, vertices[17].z);

        //三角形有两面，正面可见，背面不可见。三角形的渲染顺序与三角形的正面法线呈左手螺旋定则
        int currentCount = 0;
        for (int i = 0; i < 24; i = i + 4)
        {
            triangles[currentCount++] = i;
            triangles[currentCount++] = i + 3;
            triangles[currentCount++] = i + 1;

            triangles[currentCount++] = i;
            triangles[currentCount++] = i + 2;
            triangles[currentCount++] = i + 3;

        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();//如果不重新计算normal那么shader依然是错误的
        mesh.RecalculateBounds();
        //mesh.Optimize();
        return mesh;
    }
}
