using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using Maze;

public class PlayerMovement : MonoBehaviour
{
    private Dictionary<Square, Transform> _squarePositions;
    private Square _current;
    private Square _targetSquare;
    private Transform _target;
    public Square ObjectiveSquare;
    private MazeRotator _mazeRotator;

    private void Awake()
    {
        _mazeRotator = FindObjectOfType<MazeRotator>();
    }
    
    public void SetSquares(Dictionary<Square, Transform> squarePositions, Square startingSquare)
    {
        _squarePositions = squarePositions;
        _current = startingSquare;
        _target = _squarePositions[startingSquare];
        _targetSquare = startingSquare;
        transform.position = _target.position;
    }
    
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * 5.5f);
        ProximityCheck();
    }

    private void ProximityCheck()
    {
        if (_targetSquare == null) return;
        if (Vector3.Distance(_target.position, transform.position) < 0.3f)
        {
            _current = _targetSquare;
            _targetSquare = null;
            if (_current == ObjectiveSquare)
            {
                FindObjectOfType<Timer>()?.IncrementTimer();
                FindObjectOfType<Points>()?.IncrementPoints(_squarePositions.Count);
                FindObjectOfType<GameStarter>()?.NextMaze();
            }
        }
    }

    public void TryMove(CardinalDirection direction)
    {
        if (_targetSquare != null) return;
        _mazeRotator.RotateToTarget();
        var (square, wall) = CalculateNeighbor(direction);
        _mazeRotator.ReturnToTempRotation();
        if (wall.IsOpen)
        {
            CheckRotation(square, direction);
            MoveTo(square);
        }
    }

    private void CheckRotation(Square nextSquare, CardinalDirection direction)
    {
        if (nextSquare.Orientation != _current.Orientation)
        {
            _mazeRotator.Rotate(direction);
        }
    }
    
    private (Square, Wall) CalculateNeighbor(CardinalDirection direction)
    {
        var neighbors = 
            _current.Neighbors
                .Select(neighbor => (neighbor, _squarePositions[neighbor.Item1].transform))
                .ToArray();
        
        switch (direction)
        {
            case CardinalDirection.North:
            {
                var pair = neighbors.First(neighbor =>
                    neighbor.ToTuple().Item2.position.y - _squarePositions[_current].transform.position.y > 0.001f);
                return pair.neighbor;
            }
            case CardinalDirection.South:
            {
                var pair = neighbors.First(neighbor =>
                    neighbor.ToTuple().Item2.position.y - _squarePositions[_current].transform.position.y < -0.001f);
                return pair.neighbor;
            }
            case CardinalDirection.East:
            {
                var pair = neighbors.First(neighbor =>
                    neighbor.ToTuple().Item2.position.x - _squarePositions[_current].transform.position.x > 0.001f);
                return pair.neighbor;
            }
            case CardinalDirection.West:
            {
                var pair = neighbors.First(neighbor =>
                    neighbor.ToTuple().Item2.position.x - _squarePositions[_current].transform.position.x < -0.001f);
                return pair.neighbor;
            }
            default:
                throw new Exception("This is not possible");
        }
    }

    private void MoveTo(Square nextSquare)
    {
        _target = _squarePositions[nextSquare];
        _targetSquare = nextSquare;
    }
}
