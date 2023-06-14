using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeMaze
{
    public readonly List<Square> Squares;
    
    public CubeMaze(int width, int height, int depth)
    {
        Squares = CreateSquares(width, height, depth);
        ConnectSquares(Squares);
    }

    private static void ConnectSquares(List<Square> squares)
    {
        squares.ForEach(square =>
        {
            var directNeighbors = squares.Where(potentialNeighbor => DirectNeighbor(square, potentialNeighbor));
            foreach (var directNeighbor in directNeighbors)
            {
                square.Neighbors.Add(directNeighbor);
            }
        });
        
        squares.ForEach(square =>
        {
            var diagonalNeighbors = squares.Where(potentialNeighbor => DiagonalNeighbor(square, potentialNeighbor));
            foreach (var diagonalNeighbor in diagonalNeighbors)
            {
                square.Neighbors.Add(diagonalNeighbor);
            }
        });
    }

    private static bool DirectNeighbor(Square square, Square potentialNeighbor)
    {
        var xDistance = Mathf.Abs(square.position.x - potentialNeighbor.position.x);
        var yDistance = Mathf.Abs(square.position.y - potentialNeighbor.position.y);
        var zDistance = Mathf.Abs(square.position.z - potentialNeighbor.position.z);
        return zDistance + xDistance + yDistance == 1;
    }
    
    private static bool DiagonalNeighbor(Square square, Square potentialNeighbor)
    {
        if (square.isDepthBorder && potentialNeighbor.isDepthBorder) return false;
        if (square.isHeightBorder && potentialNeighbor.isHeightBorder) return false;
        if (square.isWidthBorder && potentialNeighbor.isWidthBorder) return false;
        var xDistance = Mathf.Abs(square.position.x - potentialNeighbor.position.x);
        var yDistance = Mathf.Abs(square.position.y - potentialNeighbor.position.y);
        var zDistance = Mathf.Abs(square.position.z - potentialNeighbor.position.z);
        if (xDistance > 1) return false;
        if (yDistance > 1) return false;
        if (zDistance > 1) return false;
        return zDistance + xDistance + yDistance == 2;
    }

    private static List<Square> CreateSquares(int width, int height, int depth)
    {
        var squares = new List<Square>();
        for (int i = 0; i < depth + 2; i++)
        {
            for (int j = 0; j < height + 2; j++)
            {
                for (int k = 0; k < width + 2; k++)
                {
                    var atDepthBorder = i == 0 || i == depth + 1;
                    var atHeightBorder = j == 0 || j == height + 1;
                    var atWidthBorder = k == 0 || k == width + 1;

                    var atBorder = atDepthBorder || atHeightBorder || atWidthBorder;

                    var overlapping = (atDepthBorder && atHeightBorder) 
                                      || (atHeightBorder && atWidthBorder) 
                                      || (atDepthBorder && atWidthBorder);
                    
                    if (atBorder && !overlapping)
                    {
                        var square = new Square(new Vector3Int(k, j, i))
                        {
                            isDepthBorder = atDepthBorder,
                            isHeightBorder = atHeightBorder,
                            isWidthBorder = atWidthBorder,
                            orientation = GetOrientation(i, j, k, depth, height, width)
                        };
                        squares.Add(square);
                    }
                }
            }
        }

        return squares;
    }

    private static Orientation GetOrientation(int i, int j, int k, int depth, int height, int width)
    {
        if (i == 0) return Orientation.Back;
        if (i == depth + 1) return Orientation.Forward;
        if (j == 0) return Orientation.Down;
        if (j == height + 1) return Orientation.Up;
        if (k == 0) return Orientation.Left;
        if (k == width + 1) return Orientation.Right;
        throw new ArgumentOutOfRangeException("No orientation");
    }
    
    [Serializable]
    public class Square
    {
        public Vector3Int position;
        public bool isDepthBorder;
        public bool isWidthBorder;
        public bool isHeightBorder;
        public Orientation orientation;
        public HashSet<Square> Neighbors = new();
        public bool hasTopWall;
        public bool hasBottomWall;
        public bool hasRightWall;
        public bool hasLeftWall;
        public Square(Vector3Int position)
        {
            this.position = position;
        }
    }

    public enum Orientation
    {
        Forward,
        Back,
        Right,
        Left,
        Up,
        Down
    }
}