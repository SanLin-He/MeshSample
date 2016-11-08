using UnityEngine;
using System.Collections;

public class N02_CreatePentagon : MonoBehaviour
{


    private Mesh _mesh;

    void Start()
    {
        CreatePenagon();
        CreateObj.CreateObjByMesh("五边形", _mesh);
    }

    void CreatePenagon()
    {
        _mesh = new Mesh();

        //以物体的坐标系计算
        _mesh.vertices = new Vector3[]
        {
            new Vector3 (0, 0, 0),
            new Vector3 (5,0, 0),
            new Vector3 (6.54f,4.75f, 0),
            new Vector3 (2.50f,7.69f, 0),
            new Vector3 (-1.54f,4.75f, 0),

        };

        
        //_mesh.uv = new Vector2[]
        //{
            
        //};

        _mesh.triangles = new int[9] 
        { 
          //逆时针
          //0, 1, 2, 
          //0, 2, 3, 
          //0, 3, 4 

          //顺时针
          1,0,4,
          4,3,1,
          1,3,2
        };

        //_mesh.RecalculateNormals();
        //_mesh.RecalculateBounds();
    }
}
