﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public class Square
    {
        public (Square, Wall) TopNeighbor;
        public (Square, Wall) BottomNeighbor;
        public (Square, Wall) RightNeighbor;
        public (Square, Wall) LeftNeighbor;
        public readonly Orientation Orientation;

        public Square(Orientation orientation)
        {
            Orientation = orientation;
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
}