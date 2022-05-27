using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;

        if (DrawDefaultInspector())
        {
            
        }

        if (GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }

        if (GUILayout.Button("Generate Single Room"))
        {
            map.GenerateSingleRoom();
        }

        if (GUILayout.Button("Clear Map Generator"))
        {
            map.ClearMapGenerator();
        }

        if (GUILayout.Button("Reset Map Options"))
        {
            map.ResetMapGeneratorOptions();
        }
    }
}
