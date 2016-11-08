using UnityEngine;
using System.Collections;

public class CreateObj {

   public static void CreateObjByMesh(string name, Mesh mesh)
    {
        GameObject obj = new GameObject();
        obj.name = name;
        //模型必须的两个组件
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();

        //mesh
        #region mesh
        //mesh信息注意应该有：
        //*1.顶点缓存
        //*2.三角形（顶点序）缓存
        //*3.uv


        //4.法线
        //5.切线
        filter.mesh = mesh; 
        #endregion

        // 使用Shader构建一个材质，并设置材质的颜色。
        #region material
        Material material = new Material(Shader.Find("Diffuse"));
        material.SetColor("_Color", Color.yellow);
        renderer.sharedMaterial = material; 
        #endregion


    }
}
