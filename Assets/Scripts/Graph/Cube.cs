using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Graph
{
    public class Cube
    {
        public Face Front;
        public Face Back;
        public Face Right;
        public Face Left;
        public Face Top;
        public Face Bottom;

        private IEnumerable<Face> Faces => new[] {
            Front,
            Back,
            Right,
            Left,
            Top,
            Bottom
        };
        public List<Square> AllSquares => Faces.Select(face => face.Squares).SelectMany(twoDArray => twoDArray.Cast<Square>()).ToList();
        public Cube(int width, int height, int depth)
        {
            CreateFaces(width, height, depth);
            ZipEdges(width, height, depth);
            CreateMaze();
        }
        private void CreateFaces(int width, int height, int depth)
        {
            Front = new Face(width, height, Orientation.Front);
            Back = new Face(width, height, Orientation.Back);
            Right = new Face(depth, height, Orientation.Right);
            Left = new Face(depth, height, Orientation.Left);
            Top = new Face(width, depth, Orientation.Up);
            Bottom = new Face(width, depth, Orientation.Down);
        }
        private void ZipEdges(int width, int height, int depth)
        {
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
                Connect(Front.Squares[i, height - 1], Orientation.Up, Top.Squares[i, 0], Orientation.Down, new Wall());
                Connect(Back.Squares[width - i - 1, 0], Orientation.Down, Bottom.Squares[i, 0], Orientation.Down, new Wall());
                Connect(Top.Squares[i, depth - 1], Orientation.Up, Back.Squares[width - i - 1, height - 1], Orientation.Up, new Wall());
                Connect(Bottom.Squares[i, depth - 1], Orientation.Up, Front.Squares[i, 0], Orientation.Down, new Wall());
            }

            // Zip Depth Seams
            for (int i = 0; i < depth; i++)
            {
                Connect(Right.Squares[i, height - 1], Orientation.Up, Top.Squares[width - 1, i], Orientation.Right, new Wall());
                Connect(Top.Squares[0, depth - i - 1], Orientation.Left, Left.Squares[i, height - 1], Orientation.Up, new Wall());
                Connect(Left.Squares[i, 0], Orientation.Down, Bottom.Squares[0, i], Orientation.Left, new Wall());
                Connect(Bottom.Squares[width - 1, depth - i - 1], Orientation.Right, Right.Squares[i, 0], Orientation.Down, new Wall());
            }
        }
        private static void Connect(Square square1, Orientation direction1, Square square2, Orientation direction2, Wall wall)
        {
            square1.SetNeighbor(direction1, (square2, wall));
            square2.SetNeighbor(direction2, (square1, wall));
        }
        private void CreateMaze()
        {
            var current = GetRandomSquare();
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
        private Square GetRandomSquare()
        {
            var allSquares = AllSquares;
            var count = allSquares.Count;
            var index = Random.Range(0, count);
            return allSquares[index];
        }
        public static (Square, Square) FurthestApart(IEnumerable<Square> squares)
        {
            var startSquare = squares.First();
            var endPointOne = FurthestAway(startSquare);
            var endPointTwo = FurthestAway(endPointOne);
            return (endPointOne, endPointTwo);
        }
        private static Square FurthestAway(Square startingPoint)
        {
            HashSet<Square> visited = new() { startingPoint };
            Queue<Square> queue = new();
            queue.Enqueue(startingPoint);
            var endPoint = startingPoint;
            while (queue.Any())
            {
                endPoint = queue.Dequeue();
                var neighbors = endPoint.Neighbors
                    .Where(pair => visited.Contains(pair.Item1) == false && pair.Item2.IsOpen)
                    .Select(pair => pair.Item1)
                    .ToList();
                neighbors.ForEach(neighbor =>
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                });
            }

            return endPoint;
        }
    }
}