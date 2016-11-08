using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorMapEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())//如果界面上有什么数据改变
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMapInEditor();
            }
        }
        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMapInEditor();
        }


    }
}
