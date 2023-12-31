using System;
using System.Collections.Generic;
using Graph;
using UnityEngine;


namespace Game
{
    public class Maze : MonoBehaviour
    {
        public int debugWidth;
        public int debugHeight;
        public int debugDepth;

        [SerializeField] private GameObject squarePrefab;
        [SerializeField] private bool debugMode;
        private List<Square> Squares { get; set; }
        public Dictionary<Square, Transform> Positions { get; } = new();

        public (Square, Square) FurthestApart() => Cube.FurthestApart(Squares);
    
        public void VisualizeMaze(int width, int height, int depth)
        {
            // Random.InitState(DateTime.Now.Millisecond);
            if (debugMode)
            {
                width = debugWidth;
                height = debugHeight;
                depth = debugDepth;
            }
            CreateFaceCube(width, height, depth);
        }

        private void CreateFaceCube(int x, int y, int z)
        {
            transform.position = new Vector3(x - 1, y - 1, z - 1) / 2;
            var cube = new Cube(x, y, z);
            CreateFace(cube.Front, new Vector3(0, 0, -1), Orientation.Front);
            CreateFace(cube.Back, new Vector3(x - 1, 0, z), Orientation.Back);
            CreateFace(cube.Right, new Vector3(x, 0, 0), Orientation.Right);
            CreateFace(cube.Left, new Vector3(0 - 1, 0, z - 1), Orientation.Left);
            CreateFace(cube.Top, new Vector3(0, y, 0), Orientation.Up);
            CreateFace(cube.Bottom, new Vector3(0, -1, z - 1), Orientation.Down);
            Squares = cube.AllSquares;
        }

        private void CreateFace(Face face, Vector3 startingPoint, Orientation orientation)
        {
            var faceWidth = face.Squares.GetLength(0);
            var faceHeight = face.Squares.GetLength(1);
            for (int h = 0; h < faceHeight; h++)
            {
                for (int w = 0; w < faceWidth; w++)
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
                        Orientation.Front => Quaternion.Euler(0, 0, 0),
                        Orientation.Back => Quaternion.Euler(0, 180, 0),
                        Orientation.Right => Quaternion.Euler(0, -90, 0),
                        Orientation.Left => Quaternion.Euler(0, 90, 0),
                        Orientation.Up => Quaternion.Euler(90, 0, 0),
                        Orientation.Down => Quaternion.Euler(-90, 0, 0),
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
                    Positions.Add(square, spawn.transform);
                }
            }
        }

        // private void OnDrawGizmos()
        // {
        //     Random.InitState(0);
        //     if (Squares == null) return;
        //     foreach (var square in Squares)
        //     {
        //         if (square.Neighbors == null) return;
        //         Gizmos.color = new Color(Random.value, Random.value, Random.value);
        //         foreach (var neighbor in square.Neighbors)
        //         {
        //             var midPoint = Vector3.Lerp(_positions[square].transform.position,
        //                 _positions[neighbor.Item1].transform.position, 0.5f);
        //             Gizmos.DrawLine(_positions[square].transform.position, midPoint);
        //             if (neighbor.Item2 == null) Gizmos.DrawSphere(_positions[square].transform.position, 0.2f);
        //         }
        //     }
        // }
    }
}