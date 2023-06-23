using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    public Dictionary<Square, GameObject> Positions = new();

    private void Start()
    {
        CreateFaceCube();
        var endpointSquares = Cube.FurthestApart(Squares);
        var startingPoint = endpointSquares.Item1;
        FindObjectOfType<PlayerMovement>().SetSquares(Positions, startingPoint);
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = Positions[endpointSquares.Item2].transform.position;
    }

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

        var anchor = Instantiate(new GameObject());
        anchor.transform.position = new Vector3(x-1, y-1, z-1) / 2;
        anchor.AddComponent<MazeRotator>();
        transform.parent = anchor.transform;
    }

    private void CreateFace(Face face, Vector3 startingPoint, Orientation orientation)
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
                if (square.TopNeighbor.Item2.IsOpen) 
                    spawn.transform.GetChild(0).gameObject.SetActive(false);
                if (square.RightNeighbor.Item2.IsOpen) 
                    spawn.transform.GetChild(1).gameObject.SetActive(false);
                if (square.BottomNeighbor.Item2.IsOpen) 
                    spawn.transform.GetChild(2).gameObject.SetActive(false);
                if (square.LeftNeighbor.Item2.IsOpen) 
                    spawn.transform.GetChild(3).gameObject.SetActive(false);
                Positions.Add(square, spawn);
                // spawn.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
                // spawn.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Random.InitState(0);
        if (Squares == null) return;
        foreach (var square in Squares)
        {
            if (square.Neighbors == null) return;
            Gizmos.color = new Color(Random.value, Random.value, Random.value);
            foreach (var neighbor in square.Neighbors)
            {
                var midPoint = Vector3.Lerp(Positions[square].transform.position, Positions[neighbor.Item1].transform.position, 0.5f);
                Gizmos.DrawLine(Positions[square].transform.position, midPoint);
                if (neighbor.Item2 == null) Gizmos.DrawSphere(Positions[square].transform.position, 0.2f);
            }
        }
    }
}