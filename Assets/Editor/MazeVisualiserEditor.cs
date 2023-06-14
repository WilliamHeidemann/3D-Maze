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
        if (GUILayout.Button("Visualise Maze"))
        {
            ClearOldMaze();
            VisualiseMaze();
        }
        if (GUILayout.Button("Create Cube Maze"))
        {
            ClearOldMaze();
            // visualiser.CreateCubeMaze();
        }

        if (GUILayout.Button("Create Face Cube"))
        {
            ClearOldMaze();
            visualiser.CreateFaceCube();
        }
    }

    private void ClearOldMaze()
    {
        while (parent.childCount > 0)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }

    private void VisualiseMaze()
    {
        const int size = 10;
        var maze = new Maze(size);
        HashSet<(int, int)> toNotSpawn = new();

        maze.Rooms.ForEach(r =>
        {
            toNotSpawn.Add((r.x * 2, r.y * 2));
        });
        maze.Rooms.ForEach(r =>
        {
            r.Connections.ToList().ForEach(c =>
            {
                toNotSpawn.Add(((r.x * 2 + c.x * 2) / 2, (r.y * 2 + c.y * 2) / 2));
            });
        });

        var toSpawn = new List<(int, int)>();
        for (int i = -1; i < size * 2; i++)
        {
            for (int j = -1; j < size * 2; j++)
            {
                if (toNotSpawn.Contains((j, i))) continue;
                toSpawn.Add((j,i));
            }
        }
        toSpawn.ToList().ForEach(r =>
        {
            var room = GameObject.CreatePrimitive(PrimitiveType.Cube);
            room.transform.parent = parent;
            room.transform.position = new Vector3(r.Item1, 0, r.Item2);
        });
    }
}
