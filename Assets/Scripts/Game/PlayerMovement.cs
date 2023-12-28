using System;
using System.Collections.Generic;
using System.Linq;
using Game.CampaignMode;
using Game.SurvivalMode;
using Graph;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        private Dictionary<Square, Transform> _squareTransforms;
        private Square _nearest;
        private Square _target;
        private Square _objectiveSquare;
        private MazeRotator _mazeRotator;
        [SerializeField] private float adjacentProximity;
        [SerializeField] private float aheadProximity;

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
            if (_nearest == _objectiveSquare) AdvanceLevel();
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

        public void DetermineInput(List<CardinalDirection> input)
        {
            if (input.Count == 0) return;
            foreach (var direction in input)
            {
                var (square, wall) = CalculateNeighbor(_nearest, direction);
                if (square == null) continue;
                if (!wall.IsOpen) continue;
                TryMove(direction, square, wall);
                return;
            }
            var d = input.First();
            var (s, w) = CalculateNeighbor(_nearest, d);
            if (s == null) return;
            TryMove(d, s, w);
        }

        private void TryMove(CardinalDirection direction, Square square, Wall wall)
        {
            if (_nearest.Orientation != _target.Orientation && square.Orientation == _nearest.Orientation && wall.IsOpen) _mazeRotator.GoBack(); // a (c) -> a open
            else if (_nearest.Orientation != _target.Orientation && _target.Orientation != square.Orientation && !wall.IsOpen) _mazeRotator.GoBack(); // a (c) -> b closed
            else if (_nearest.Orientation != _target.Orientation && _target.Orientation != square.Orientation && wall.IsOpen) 
            {
                _mazeRotator.GoBack();
                _mazeRotator.SetTarget(direction);
            } // a (c) -> b open
            else if (_target.Orientation != square.Orientation && wall.IsOpen) { _mazeRotator.SetTarget(direction); } // (a) -> b open

            _target = wall.IsOpen ? square : _nearest;
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
                        neighbor.ToTuple().Item2.position.y - _squareTransforms[current].transform.position.y)
                        .FirstOrDefault(tuple => Mathf.Abs(tuple.Item2.position.x - _squareTransforms[current].transform.position.x) < adjacentProximity //1f
                                                 && tuple.Item2.position.y - _squareTransforms[current].transform.position.y > aheadProximity //0.1f
                        );
                    return pair.neighbor;
                }
                case CardinalDirection.South:
                {
                    var pair = neighbors.OrderBy(neighbor =>
                        neighbor.ToTuple().Item2.position.y - _squareTransforms[current].transform.position.y)
                        .FirstOrDefault(tuple => Mathf.Abs(tuple.Item2.position.x - _squareTransforms[current].transform.position.x) < adjacentProximity
                                                 && _squareTransforms[current].transform.position.y - tuple.Item2.position.y > aheadProximity
                        );
                    return pair.neighbor;
                }
                case CardinalDirection.East:
                {
                    var pair = neighbors.OrderByDescending(neighbor =>
                        neighbor.ToTuple().Item2.position.x - _squareTransforms[current].transform.position.x)
                        .FirstOrDefault(tuple => Mathf.Abs(tuple.Item2.position.y - _squareTransforms[current].transform.position.y) < adjacentProximity
                        && tuple.Item2.position.x - _squareTransforms[current].transform.position.x > aheadProximity
                        );
                    return pair.neighbor;
                }
                case CardinalDirection.West:
                {
                    var pair = neighbors.OrderBy(neighbor =>
                        neighbor.ToTuple().Item2.position.x - _squareTransforms[current].transform.position.x)
                        .FirstOrDefault(tuple => Mathf.Abs(tuple.Item2.position.y - _squareTransforms[current].transform.position.y) < adjacentProximity
                                                 && _squareTransforms[current].transform.position.x - tuple.Item2.position.x > aheadProximity
                        );
                    return pair.neighbor;
                }
                default:
                    throw new Exception("This is not possible");
            }
        }

        private void AdvanceLevel()
        {
            var gameMode = FindObjectOfType<GameModeManager>()?.gameMode;

            if (gameMode == GameMode.TimeTrial)
            {
                FindObjectOfType<Timer>()?.IncrementTimer();
                FindObjectOfType<Points>()?.IncrementPoints(_squareTransforms.Count);
                FindObjectOfType<SurvivalModeStarter>()?.NextMaze();
            }
            else
            {
                FindObjectOfType<LevelManager>()?.AdvanceLevel();
            }
            SoundManager.Instance.PlayCompleteLevelSound();
        }

        private void OnDrawGizmos()
        {
            // Gizmos.DrawSphere(_squareTransforms[_target].position, .05f);
            // Gizmos.color = Color.red;
            // Gizmos.DrawSphere(_squareTransforms[_nearest].position, .05f);
        }
    }
}
