using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Maze
{
    public List<Room> Rooms;
    private Stack<Room> stack;

    public Maze(int size)
    {
        ConstructRooms(size);
        stack = new Stack<Room>();
        var current = Rooms.First();
        current.Visited = true;
        stack.Push(current);
        while (stack.Any())
        {
            var neighbors = Neighbors(current);
            Room next;
            if (neighbors.Any())
            {
                next = neighbors[Random.Range(0, neighbors.Count)];
                next.Visited = true;
                next.Connections.Add(current);
                current.Connections.Add(next);
                stack.Push(next);
            }
            else
            {
                next = stack.Pop();
            }
            current = next;
        }
    }

    private List<Room> Neighbors(Room current)
    {
        return Rooms.Where(room => 
            (Mathf.Abs(room.x - current.x) == 1 && room.y == current.y ||
            Mathf.Abs(room.y - current.y) == 1 && room.x == current.x) &&
            !room.Visited
        ).ToList();
    }

    private void ConstructRooms(int size)
    {
        Rooms = new List<Room>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Rooms.Add(new Room(j, i));
            }
        }
    }

    public class Room
    {
        public int x, y;
        public bool Visited;
        public readonly HashSet<Room> Connections = new();
        public Room(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}


