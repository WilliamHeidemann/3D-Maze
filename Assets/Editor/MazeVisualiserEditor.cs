using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeVisualiser))]
public class MazeVisualiserEditor : Editor
{
    private Transform parent;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var visualiser = (MazeVisualiser)target;
        parent = visualiser.transform;
        if (GUILayout.Button("Create Face Cube"))
        {
            ClearOldMaze();
            visualiser.CreateFaceCube();
        }
        
        if (GUILayout.Button("Clear Maze"))
        {
            ClearOldMaze();
        }
    }

    private void ClearOldMaze()
    {
        while (parent.childCount > 0)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }
}
