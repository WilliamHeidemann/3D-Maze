using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube
{
    public Face Front;
    public Face Back;
    public Face Right;
    public Face Left;
    public Face Top;
    public Face Bottom;

    public Face[] Faces => new[]
    {
        Front,
        Back,
        Right,
        Left,
        Top,
        Bottom
    };

    public Cube(int width, int height, int depth)
    {
        Front = new Face(width, height, Orientation.Front);
        Back = new Face(width, height, Orientation.Back);
        Right = new Face(depth, height, Orientation.Right);
        Left = new Face(depth, height, Orientation.Left);
        Top = new Face(width, depth, Orientation.Up);
        Bottom = new Face(width, depth, Orientation.Down);

        // Zip Height Seams
        for (int i = 0; i < height; i++)
        {
            Connect(Front.Squares[width - 1, i], Orientation.Right, Right.Squares[0, i], Orientation.Left, new Wall());
            Connect(Back.Squares[width - 1, i], Orientation.Right, Left.Squares[0, i], Orientation.Left, new Wall());
            Connect(Right.Squares[depth - 1, i], Orientation.Right, Back.Squares[0, i], Orientation.Left, new Wall());
            Connect(Left.Squares[depth - 1, i], Orientation.Right, Front.Squares[0, i], Orientation.Left, new Wall());
        }
        
        // Zip Width Seams
        for (int i = 0; i < width; i++)
        {
            Connect(Front.Squares[i, height-1], Orientation.Up, Top.Squares[i, 0], Orientation.Down, new Wall());
            Connect(Back.Squares[width-i-1, 0], Orientation.Down, Bottom.Squares[i, 0], Orientation.Down, new Wall());
            Connect(Top.Squares[i, depth-1], Orientation.Up, Back.Squares[width-i-1, height-1], Orientation.Up, new Wall());
            Connect(Bottom.Squares[i, depth-1], Orientation.Up, Front.Squares[i, 0], Orientation.Down, new Wall());
        }

        // Zip Depth Seams
        for (int i = 0; i < depth; i++)
        {
            Connect(Right.Squares[i, height - 1], Orientation.Up, Top.Squares[width - 1, i], Orientation.Right, new Wall());
            Connect(Top.Squares[0, depth-i-1], Orientation.Left, Left.Squares[i, height - 1], Orientation.Up, new Wall());
            Connect(Left.Squares[i, 0], Orientation.Down, Bottom.Squares[0, i], Orientation.Left, new Wall());
            Connect(Bottom.Squares[width - 1, depth-i-1], Orientation.Right, Right.Squares[i, 0], Orientation.Down, new Wall());
        }

        CreateMaze();
    }

    private static void Connect(Square square1, Orientation direction1, Square square2, Orientation direction2, Wall wall)
    {
        square1.SetNeighbor(direction1, (square2, wall));
        square2.SetNeighbor(direction2, (square1, wall));
    }

    private void CreateMaze()
    {
        // Choose a random starting square in the future
        var current = Front.Squares[0, 0];
        var stack = new Stack<Square>();
        stack.Push(current);
        var visited = new HashSet<Square> { current };
        while (stack.Any())
        {
            var neighbors = current.Neighbors.Where(neighbor => !visited.Contains(neighbor.Item1)).ToArray();
            if (neighbors.Any())
            {
                var neighbor = neighbors[Random.Range(0, neighbors.Length)];
                neighbor.Item2.IsOpen = true;
                visited.Add(neighbor.Item1);
                stack.Push(neighbor.Item1);
                current = neighbor.Item1;
            }
            else
            {
                current = stack.Pop();
            }
        }
    }

    public static (Square, Square) FurthestApart(IEnumerable<Square> squares)
    {
        var startSquare = squares.First();
        var endPointOne = FindEndpoint(startSquare);
        var endPointTwo = FindEndpoint(endPointOne);
        return (endPointOne, endPointTwo);
    }

    private static Square FindEndpoint(Square startingPoint)
    {
        List<Square> current = new() { startingPoint };
        HashSet<Square> visited = new() { startingPoint };
        var endpoint = startingPoint;
        while (current.Any())
        {
            endpoint = current.First();
            var neighbors = current
                .SelectMany(square => square.Neighbors)
                .Select(pair => pair.Item1)
                .Where(square => visited.Contains(square) == false)
                .ToList();
            neighbors.ForEach(neighbor => visited.Add(neighbor));
            current = neighbors;
        }

        return endpoint;
    }
}

public class Face
{
    public Square[,] Squares;
    public Face(int width, int height, Orientation orientation)
    {
        CreateSquares(width, height, orientation);
        ConnectDirectNeighbors(width, height);
    }

    private void CreateSquares(int width, int height, Orientation orientation)
    {
        Squares = new Square[width, height];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Squares[w, h] = new Square(orientation);
            }
        }
    }

    private void ConnectDirectNeighbors(int width, int height)
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (w < width - 1)
                {
                    var wall = new Wall();
                    Squares[w, h].SetNeighbor(Orientation.Right, (Squares[w + 1, h], wall));
                    Squares[w + 1, h].SetNeighbor(Orientation.Left, (Squares[w, h], wall));
                }
                
                if (h < height - 1)
                {
                    var wall = new Wall();
                    Squares[w, h].SetNeighbor(Orientation.Up, (Squares[w, h + 1], wall));
                    Squares[w, h + 1].SetNeighbor(Orientation.Down, (Squares[w, h], wall));
                }
            }
        }
    }
}

public class Square
{
    public (Square, Wall) TopNeighbor;
    public (Square, Wall) BottomNeighbor;
    public (Square, Wall) RightNeighbor;
    public (Square, Wall) LeftNeighbor;
    public Orientation FaceDirection;

    public Square(Orientation faceDirection)
    {
        FaceDirection = faceDirection;
    }

    public void SetNeighbor(Orientation direction, (Square, Wall) neighbor)
    {
        switch (direction)
        {
            case Orientation.Up:
                TopNeighbor = neighbor;
                break;
            case Orientation.Right:
                RightNeighbor = neighbor;
                break;
            case Orientation.Down:
                BottomNeighbor = neighbor;
                break;
            case Orientation.Left:
                LeftNeighbor = neighbor;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
    public IEnumerable<(Square, Wall)> Neighbors => new[]
    {
        TopNeighbor,
        BottomNeighbor,
        RightNeighbor,
        LeftNeighbor
    }.Where(neighbor => neighbor.Item1 != null);
}
public class Wall
{
    public bool IsOpen = false;
}
