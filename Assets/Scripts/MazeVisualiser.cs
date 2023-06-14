using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CubeMaze.Orientation;
using Random = UnityEngine.Random;

public enum Orientation
{
    Front,
    Back,
    Right,
    Left,
    Up,
    Down
}

public class MazeVisualiser : MonoBehaviour
{
    public int x, y, z;
    // public List<CubeMaze.Square> Squares = new();
    // private Dictionary<CubeMaze.Square, GameObject> dictionary = new();

    public GameObject squarePrefab;

    public List<Square> Squares;
    public Dictionary<Square, Vector3> Positions = new();

    public void CreateFaceCube()
    {
        var cube = new Cube(x, y, z);
        CreateFace(cube.Front, new Vector3(0,0,-1), Orientation.Front);
        CreateFace(cube.Back, new Vector3(x-1, 0, z), Orientation.Back);
        CreateFace(cube.Right, new Vector3(x, 0,0), Orientation.Right);
        CreateFace(cube.Left, new Vector3(0-1, 0,z-1), Orientation.Left);
        CreateFace(cube.Top, new Vector3(0, y,0), Orientation.Up);
        CreateFace(cube.Bottom, new Vector3(0, -1,z-1), Orientation.Down);
        Squares = cube.Faces.Select(face => face.Squares).SelectMany(twoDArray => twoDArray.Cast<Square>()).ToList();
    }

    public void CreateFace(Face face, Vector3 startingPoint, Orientation orientation)
    {
        var width = face.Squares.GetLength(0);
        var height = face.Squares.GetLength(1);
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                var position = orientation switch
                {
                    Orientation.Front => new Vector3(w, h, 0),
                    Orientation.Back => new Vector3(-w, h, 0),
                    Orientation.Right => new Vector3(0, h, w),
                    Orientation.Left => new Vector3(0, h, -w),
                    Orientation.Up => new Vector3(w, 0, h),
                    Orientation.Down => new Vector3(w, 0, -h),
                    _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
                };
                position += startingPoint;
                var rotation = orientation switch
                {
                    Orientation.Front => Quaternion.Euler(0,0,0),
                    Orientation.Back => Quaternion.Euler(0,180,0),
                    Orientation.Right => Quaternion.Euler(0,-90,0),
                    Orientation.Left => Quaternion.Euler(0,90,0),
                    Orientation.Up => Quaternion.Euler(90,0,0),
                    Orientation.Down => Quaternion.Euler(-90,0,0),
                    _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
                };
                var spawn = Instantiate(squarePrefab, position, rotation, transform);
                spawn.name = $"{orientation.ToString()} {w},{h}";
                var square = face.Squares[w, h];
                // Wall is null
                // var a = square.BottomNeighbor;
                // var b = a.Item2;
                // var c = b.IsOpen;
                //
                // Back face has no wall (null) in top en bottom neighbors along x-axis in bottom and bottom-1 row and top and top-1 row
                if(square.TopNeighbor.Item2 != null) if (square.TopNeighbor.Item2.IsOpen) spawn.transform.GetChild(0).gameObject.SetActive(false);
                if(square.RightNeighbor.Item2 != null) if (square.RightNeighbor.Item2.IsOpen) spawn.transform.GetChild(1).gameObject.SetActive(false);
                if(square.BottomNeighbor.Item2 != null) if (square.BottomNeighbor.Item2.IsOpen) spawn.transform.GetChild(2).gameObject.SetActive(false);
                if(square.LeftNeighbor.Item2 != null) if (square.LeftNeighbor.Item2.IsOpen) spawn.transform.GetChild(3).gameObject.SetActive(false);
                Positions.Add(square, position);
                // spawn.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
                // spawn.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }


    // public void CreateCubeMaze()
    // {
    //     var cubeMaze = new CubeMaze(x, y, z);
    //     Squares = cubeMaze.Squares.ToList();
    //     Squares.ForEach(square =>
    //     {
    //         var cube = Instantiate(squarePrefab);
    //         cube.transform.position = square.position;
    //         cube.transform.parent = transform;
    //         cube.transform.rotation = square.orientation switch
    //         {
    //             Forward => Quaternion.Euler(0,180,0),
    //             Back => Quaternion.Euler(0,0,0),
    //             Right => Quaternion.Euler(0,-90,0),
    //             Left => Quaternion.Euler(0,90,0),
    //             Up => Quaternion.Euler(90,0,0),
    //             Down => Quaternion.Euler(-90,0,0),
    //             _ => throw new ArgumentOutOfRangeException()
    //         };
    //         dictionary.Add(square, cube);
    //     });
    // }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (Squares == null) return;
        foreach (var square in Squares)
        {
            if (square.Neighbors == null) return;
            foreach (var neighbor in square.Neighbors)
            {
                Gizmos.DrawLine(Positions[square], Positions[neighbor.Item1]);
                if (neighbor.Item2 == null) Gizmos.DrawSphere(Positions[square], 0.2f);
            }
        }
    }
}