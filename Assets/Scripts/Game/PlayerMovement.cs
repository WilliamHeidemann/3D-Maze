using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using Maze;

public class PlayerMovement : MonoBehaviour
{
    private Dictionary<Square, GameObject> _squarePositions;
    private Square _current;
    private GameObject target;
    public Square ObjectiveSquare;
    public GameObject nextLevelButton;

    
    public void SetSquares(Dictionary<Square, GameObject> squarePositions, Square startingSquare)
    {
        _squarePositions = squarePositions;
        MoveTo(startingSquare);
        SetStartOrientation(startingSquare);
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(CardinalDirection.North);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryMove(CardinalDirection.South);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(CardinalDirection.East);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(CardinalDirection.West);
        }
        
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.1f);
    }

    private void TryMove(CardinalDirection direction)
    {
        var neighbor = CalculateNeighbor(direction);
        if (neighbor.Item2.IsOpen)
        {
            CheckRotation(neighbor.Item1, direction);
            MoveTo(neighbor.Item1);
        }
    }

    private void CheckRotation(Square nextSquare, CardinalDirection direction)
    {
        if (nextSquare.Orientation != _current.Orientation)
        {
            FindObjectOfType<MazeRotator>().Rotate(direction);
        }
    }


    private (Square, Wall) CalculateNeighbor(CardinalDirection direction)
    {
        var neighbors = 
            _current.Neighbors
                .Select(neighbor => (neighbor, _squarePositions[neighbor.Item1].transform))
                .ToArray();
        
        if (direction == CardinalDirection.North)
        {
            var pair = neighbors.First(neighbor =>
                neighbor.ToTuple().Item2.position.y - _squarePositions[_current].transform.position.y > 0.001f);
            return pair.neighbor;
        }
        
        if (direction == CardinalDirection.South)
        {
            var pair = neighbors.First(neighbor =>
                neighbor.ToTuple().Item2.position.y - _squarePositions[_current].transform.position.y < -0.001f);
            return pair.neighbor;
        }
        
        if (direction == CardinalDirection.East)
        {
            var pair = neighbors.First(neighbor =>
                neighbor.ToTuple().Item2.position.x - _squarePositions[_current].transform.position.x > 0.001f);
            return pair.neighbor;
        }
        
        if (direction == CardinalDirection.West)
        {
            var pair = neighbors.First(neighbor =>
                neighbor.ToTuple().Item2.position.x - _squarePositions[_current].transform.position.x < -0.001f);
            return pair.neighbor;
        }

        throw new Exception("This is not possible");
    }

    private void MoveTo(Square nextSquare)
    {
        _current = nextSquare;
        target = _squarePositions[nextSquare];
        if (nextSquare == ObjectiveSquare) nextLevelButton.SetActive(true);
    }
    
    private void SetStartOrientation(Square startingSquare)
    {
        var mazeRotator = FindObjectOfType<MazeRotator>();
        mazeRotator.Rotate(startingSquare.Orientation);
    }
}
