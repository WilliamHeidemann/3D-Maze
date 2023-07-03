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
    private Square _targetSquare;
    private GameObject target;
    public Square ObjectiveSquare;
    public GameObject nextLevelButton;

    public GameObject joystickGameObject;

    
    public void SetSquares(Dictionary<Square, GameObject> squarePositions, Square startingSquare)
    {
        _squarePositions = squarePositions;
        _current = startingSquare;
        MoveTo(startingSquare);
        SetStartOrientation(startingSquare);
    }
    

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 5f);
        ProximityCheck();
    }

    private void ProximityCheck()
    {
        if (_targetSquare == null) return;
        if (Vector3.Distance(target.transform.position, transform.position) < 0.01f)
        {
            _current = _targetSquare;
            _targetSquare = null;
        }
    }

    public void TryMove(CardinalDirection direction)
    {
        if (_targetSquare != null) return;
        var rotator = FindObjectOfType<MazeRotator>();
        rotator.RotateToTarget();
        var neighbor = CalculateNeighbor(direction);
        rotator.ReturnToTempRotation();
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
        // _current = nextSquare;
        target = _squarePositions[nextSquare];
        _targetSquare = nextSquare;
        if (nextSquare == ObjectiveSquare)
        {
            nextLevelButton.SetActive(true);
            var timer = FindObjectOfType<Timer>();
            timer.levelComplete = true;
            timer.RecordAttempt();
            joystickGameObject.SetActive(false);
        }
    }
    
    private void SetStartOrientation(Square startingSquare)
    {
        var mazeRotator = FindObjectOfType<MazeRotator>();
        mazeRotator.Rotate(startingSquare.Orientation);
        mazeRotator.SnapToTarget();
        transform.position = target.transform.position;
    }
}
