using UnityEngine;
using System.Collections;

public class N01_CreateSquare : MonoBehaviour
{


    private Vector3[] _vertices;
    private Vector2[] _uv;
    private int[] _triangles;

    private Mesh _mesh;
	// Use this for initialization
	void Start ()
	{
	    CreateMesh();
        CreateObj.CreateObjByMesh("正方形",_mesh);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateMesh()
    {
        _mesh = new Mesh();
        _mesh.name = "square";
        //顶点 4个，按照物体坐标
        _vertices = new Vector3[4]
        {
            //new Vector3(0,0,0),//左下
            //new Vector3(1,0,0),//右下
            //new Vector3(0,1,0),//左上
            //new Vector3(1,1,0), //右上

            new Vector3(1,1,0),//左下
            new Vector3(2,1,0),//右下
            new Vector3(1,2,0),//左上
            new Vector3(2,2,0), //右上
 
        };

        //一般是美术绑定好
        //uv 4个顶点映射的uv位置(注意uv的坐标是0到1)
        _uv = new Vector2[4]
        {
            new Vector2(0,0),  // 顶点0的纹理坐标
            new Vector2(1,0), 
            new Vector2(0,1), 
            new Vector2(1,1), 

        };

        //三角形 正方形由两个三角形构成（确定网格的空间构成）
        //6个顶点缓冲
        //顺时针在正面，逆时针在背面（三角形的渲染顺序与三角形的正面法线呈左手螺旋定则。 backface culling ）
        //顺时针和逆时针中的点的先后关系不重要0,1,2=1,2,0=2,0,1
        //但最好可以连接，提升一丁点性能
        _triangles = new int[6]//用来记录连接三角形的顺序，长度是三的倍数
        {
            //逆时针
            0, 1, 2,
            1, 3, 2
        };

        _mesh.vertices = _vertices;
        _mesh.uv = _uv;
        _mesh.triangles = _triangles;

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }
}
