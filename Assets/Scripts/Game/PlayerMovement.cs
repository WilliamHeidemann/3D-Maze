using System;
using System.Collections.Generic;
using System.Linq;
using Graph;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        private Dictionary<Square, Transform> _squareTransforms;
        private Square _current;
        private Square _targetSquare;
        private Square _objectiveSquare;
        private MazeRotator _mazeRotator;

        private void Awake()
        {
            _mazeRotator = FindObjectOfType<MazeRotator>();
        }
    
        public void SetSquares(Dictionary<Square, Transform> squarePositions, Square startingSquare, Square objectiveSquare)
        {
            _squareTransforms = squarePositions;
            _current = startingSquare;
            _targetSquare = startingSquare;
            _objectiveSquare = objectiveSquare;
            transform.position = _squareTransforms[_targetSquare].position;
        }
    
        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _squareTransforms[_targetSquare].position, Time.deltaTime * 5.3f);
            ProximityCheck();
        }

        private void ProximityCheck()
        {
            if (Vector3.Distance(_squareTransforms[_targetSquare].position, transform.position) < 0.3f)
            {
                _current = _targetSquare;
                if (_current == _objectiveSquare)
                {
                    AdvanceLevel();
                }
            }
        }

        public void TryMove(CardinalDirection direction)
        {
            _mazeRotator.RotateToTarget();
            var (square, wall) = CalculateNeighbor(direction);
            _mazeRotator.ReturnToTempRotation();
            if (wall.IsOpen)
            {
                CheckRotation(square, direction);
                _targetSquare = square;
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
                    .Select(neighbor => (neighbor, _squareTransforms[neighbor.Item1].transform)) // Why .transform?
                    .ToArray();
        
            switch (direction)
            {
                case CardinalDirection.North:
                {
                    var pair = neighbors.First(neighbor =>
                        neighbor.ToTuple().Item2.position.y - _squareTransforms[_current].transform.position.y > 0.001f);
                    return pair.neighbor;
                }
                case CardinalDirection.South:
                {
                    var pair = neighbors.First(neighbor =>
                        neighbor.ToTuple().Item2.position.y - _squareTransforms[_current].transform.position.y < -0.001f);
                    return pair.neighbor;
                }
                case CardinalDirection.East:
                {
                    var pair = neighbors.First(neighbor =>
                        neighbor.ToTuple().Item2.position.x - _squareTransforms[_current].transform.position.x > 0.001f);
                    return pair.neighbor;
                }
                case CardinalDirection.West:
                {
                    var pair = neighbors.First(neighbor =>
                        neighbor.ToTuple().Item2.position.x - _squareTransforms[_current].transform.position.x < -0.001f);
                    return pair.neighbor;
                }
                default:
                    throw new Exception("This is not possible");
            }
        }

        private void AdvanceLevel()
        {
            FindObjectOfType<Timer>()?.IncrementTimer();
            FindObjectOfType<Points>()?.IncrementPoints(_squareTransforms.Count);
            FindObjectOfType<SurvivalModeStarter>()?.NextMaze();
        }
    }
}
