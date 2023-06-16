using System;
using System.Collections.Generic;
using System.Linq;
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
        Front = new Face(width, height);
        Back = new Face(width, height);
        Right = new Face(depth, height);
        Left = new Face(depth, height);
        Top = new Face(width, depth);
        Bottom = new Face(width, depth);

        // Zip Height Seams
        for (int i = 0; i < height; i++)
        {
            Connect(Front.Squares[width - 1, i], Orientation.Right, Right.Squares[0, i], Orientation.Left, new Wall());
            // Front.Squares[width - 1, i].RightNeighbor.Item1 = Right.Squares[0, i];
            // Right.Squares[0, i].LeftNeighbor.Item1 = Front.Squares[width - 1, i];
            
            Connect(Back.Squares[width - 1, i], Orientation.Right, Left.Squares[0, i], Orientation.Left, new Wall());
            // Back.Squares[width - 1, i].RightNeighbor.Item1 = Left.Squares[0, i];
            // Left.Squares[0, i].LeftNeighbor.Item1 = Back.Squares[width - 1, i];
            
            Connect(Right.Squares[depth - 1, i], Orientation.Right, Back.Squares[0, i], Orientation.Left, new Wall());
            // Right.Squares[depth - 1, i].RightNeighbor.Item1 = Back.Squares[0, i];
            // Back.Squares[0, i].LeftNeighbor.Item1 = Right.Squares[depth - 1, i];
            
            Connect(Left.Squares[depth - 1, i], Orientation.Right, Front.Squares[0, i], Orientation.Left, new Wall());
            // Left.Squares[depth - 1, i].RightNeighbor.Item1 = Front.Squares[0, i];
            // Front.Squares[0, i].LeftNeighbor.Item1 = Left.Squares[depth - 1, i];
        }
        
        // Zip Width Seams
        for (int i = 0; i < width; i++)
        {
            Connect(Front.Squares[i, height-1], Orientation.Up, Top.Squares[i, 0], Orientation.Down, new Wall());
            // Front.Squares[i, height-1].TopNeighbor.Item1 = Top.Squares[i, 0];
            // Top.Squares[i, 0].BottomNeighbor.Item1 = Front.Squares[i, height-1];
            
            Connect(Back.Squares[width-i-1, 0], Orientation.Up, Bottom.Squares[i, 0], Orientation.Down, new Wall());
            // Back.Squares[width-i-1, 0].TopNeighbor.Item1 = Bottom.Squares[i, 0];
            // Bottom.Squares[i, 0].BottomNeighbor.Item1 = Back.Squares[width-i-1, 0];
            
            Connect(Top.Squares[i, depth-1], Orientation.Down, Back.Squares[width-i-1, height-1], Orientation.Up, new Wall());
            // Top.Squares[i, depth-1].TopNeighbor.Item1 = Back.Squares[width-i-1, height-1];
            // Back.Squares[width-i-1, height-1].BottomNeighbor.Item1 = Top.Squares[i, depth-1];
            
            Connect(Bottom.Squares[i, depth-1], Orientation.Up, Front.Squares[i, 0], Orientation.Down, new Wall());
            // Bottom.Squares[i, depth-1].TopNeighbor.Item1 = Front.Squares[i, 0];
            // Front.Squares[i, 0].BottomNeighbor.Item1 = Bottom.Squares[i, depth-1];
        }
        //
        // Zip Depth Seams
        for (int i = 0; i < depth; i++)
        {
            Connect(Right.Squares[i, height - 1], Orientation.Up, Top.Squares[width - 1, i], Orientation.Right, new Wall());
            // Right.Squares[i, height - 1].TopNeighbor.Item1 = Top.Squares[width - 1, i];
            // Top.Squares[width - 1, i].RightNeighbor.Item1 = Right.Squares[i, height - 1];
        
            Connect(Top.Squares[0, depth-i-1], Orientation.Left, Left.Squares[i, height - 1], Orientation.Up, new Wall());
            // Top.Squares[0, depth-i-1].LeftNeighbor.Item1 = Left.Squares[i, height - 1];
            // Left.Squares[i, height - 1].TopNeighbor.Item1 = Top.Squares[0, depth-i-1];
        
            Connect(Left.Squares[i, 0], Orientation.Down, Bottom.Squares[0, i], Orientation.Left, new Wall());
            // Left.Squares[i, 0].BottomNeighbor.Item1 = Bottom.Squares[0, i];
            // Bottom.Squares[0, i].LeftNeighbor.Item1 = Left.Squares[i, 0];
        
            Connect(Bottom.Squares[width - 1, depth-i-1], Orientation.Right, Right.Squares[i, 0], Orientation.Down, new Wall());
            // Bottom.Squares[width - 1, depth-i-1].RightNeighbor.Item1 = Right.Squares[i, 0];
            // Right.Squares[i, 0].BottomNeighbor.Item1 = Bottom.Squares[width - 1, depth-i-1];
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
}

public class Face
{
    public Square[,] Squares;
    public Face(int width, int height)
    {
        CreateSquares(width, height);
        ConnectDirectNeighbors(width, height);
    }

    private void CreateSquares(int width, int height)
    {
        Squares = new Square[width, height];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Squares[w, h] = new Square();
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
                    // Squares[w, h].RightNeighbor.Item1 = Squares[w + 1, h];
                    // Squares[w, h].LeftNeighbor.Item1 = Squares[w - 1, h];
                }
                
                if (h < height - 1)
                {
                    var wall = new Wall();
                    Squares[w, h].SetNeighbor(Orientation.Up, (Squares[w, h + 1], wall));
                    Squares[w, h + 1].SetNeighbor(Orientation.Down, (Squares[w, h], wall));
                    // Squares[w, h].TopNeighbor.Item1 = Squares[w, h + 1];
                    // Squares[w, h].BottomNeighbor.Item1 = Squares[w, h - 1];
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
