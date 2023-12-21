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
        private Square _nearest;
        // private Square Current => _squareTransforms
            // .OrderBy(pair => Vector3.Distance(pair.Value.position, transform.position)).First().Key;
        private Square _target;
        private Square _objectiveSquare;
        private MazeRotator _mazeRotator;
        private List<Quaternion> _quaternions;

        private void Awake()
        {
            _mazeRotator = FindObjectOfType<MazeRotator>();
        }
    
        public void SetSquares(Dictionary<Square, Transform> squarePositions, Square startingSquare, Square objectiveSquare)
        {
            _squareTransforms = squarePositions;
            _nearest = startingSquare;
            _target = startingSquare;
            _objectiveSquare = objectiveSquare;
            transform.position = _squareTransforms[_target].position;
        }
    
        private void Update()
        {
            _nearest = Nearest(_nearest);
            transform.position = Vector3.MoveTowards(transform.position, _squareTransforms[_target].position, Time.deltaTime * 5.3f);
        }

        private Square Nearest(Square current)
        {
            var neighbors = current.Neighbors
                .Where(pair => pair.Item2.IsOpen)
                .Select(pair => pair.Item1)
                .ToList();
            neighbors.Add(current);
            return neighbors.OrderBy(square =>
                Vector3.Distance(transform.position, _squareTransforms[square].position)).First();
        }

        public void TryMove(CardinalDirection direction)
        {
            var (square, wall) = CalculateNeighbor(_nearest, direction);
            if (square == _target) return;
            _target = wall.IsOpen ? square : _nearest;
            // Hvis wall er closed og _nearest.orientation er anderledes end rotater.targetOrientation skal rotate gå modsat rotater.targetOrientation
            _mazeRotator.SetTarget(direction, _target.Orientation, _squareTransforms[_target].name);
        }

        private (Square, Wall) CalculateNeighbor(Square current, CardinalDirection direction)
        {
            var neighbors = 
                current.Neighbors
                    .Select(neighbor => (neighbor, _squareTransforms[neighbor.Item1]))
                    .ToArray();
        
            switch (direction)
            {
                case CardinalDirection.North:
                {
                    var pair = neighbors.OrderByDescending(neighbor =>
                        neighbor.ToTuple().Item2.position.y - _squareTransforms[current].transform.position.y).First();
                    return pair.neighbor;
                }
                case CardinalDirection.South:
                {
                    var pair = neighbors.OrderBy(neighbor =>
                        neighbor.ToTuple().Item2.position.y - _squareTransforms[current].transform.position.y).First();
                    return pair.neighbor;
                }
                case CardinalDirection.East:
                {
                    var pair = neighbors.OrderByDescending(neighbor =>
                        neighbor.ToTuple().Item2.position.x - _squareTransforms[current].transform.position.x).First();
                    return pair.neighbor;
                }
                case CardinalDirection.West:
                {
                    var pair = neighbors.OrderBy(neighbor =>
                        neighbor.ToTuple().Item2.position.x - _squareTransforms[current].transform.position.x).First();
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

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_squareTransforms[_target].position, .05f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_squareTransforms[_nearest].position, .05f);
        }
    }
}
