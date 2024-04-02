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
        private DirectionCalculator directionCalculator;
        private CardinalDirection lastMoveDirection;
        [SerializeField] private GameObject rewardEffect1;
        [SerializeField] private GameObject rewardEffect2;
        [SerializeField] private GameObject rewardEffect3;

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
            directionCalculator = new DirectionCalculator(startingSquare.Orientation);
        }
    
        private void Update()
        {
            CalculateRotation();
            transform.position = Vector3.MoveTowards(transform.position, _squareTransforms[_target].position, Time.deltaTime * 5.3f);
            if (_nearest == _objectiveSquare) AdvanceLevel();
        }

        public void CalculateNearest()
        {
            var newNearest = Nearest(_nearest);
            if (newNearest.Orientation != _nearest.Orientation) directionCalculator.HandleLogicalFaceChange(lastMoveDirection);
            _nearest = newNearest;
        }

        private void CalculateRotation()
        {
            directionCalculator.SetTargetRotationDirection(_target.Orientation);
            _mazeRotator.SetTarget(directionCalculator.GetRotation());
        }

        private Square Nearest(Square current)
        {
            var neighbors = current.Neighbors
                .Where(pair => pair.Item2.IsOpen)
                .Select(pair => pair.Item1)
                .ToList();
            neighbors.Add(current);
            return neighbors.OrderBy(square =>
                Vector3.SqrMagnitude(transform.position - _squareTransforms[square].position)).First();
        }

        public void DetermineInput(List<CardinalDirection> input)
        {
            if (input.Count == 0) return;
            foreach (var direction in input.Select(direction => directionCalculator.CalculateWorldDirection(direction)))
            {
                var (square, wall) = GetNeighbor(_nearest, direction);
                if (square == null) continue;
                if (!wall.IsOpen) continue;
                Move(direction, square, wall);
                return;
            }
            var d = directionCalculator.CalculateWorldDirection(input.First());
            var (s, w) = GetNeighbor(_nearest, d);
            if (s == null) return;
            Move(d, s, w);
        }

        private static (Square, Wall) GetNeighbor(Square square, CardinalDirection direction)
        {
            return direction switch
            {
                CardinalDirection.North => square.TopNeighbor,
                CardinalDirection.South => square.BottomNeighbor,
                CardinalDirection.East => square.RightNeighbor,
                CardinalDirection.West => square.LeftNeighbor,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        private void Move(CardinalDirection direction, Square square, Wall wall)
        {
            _target = wall.IsOpen ? square : _nearest;
            lastMoveDirection = direction;
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
            else if (gameMode == GameMode.Campaign)
            {
                Destroy(gameObject);
                FindObjectOfType<LevelManager>()?.AdvanceLevel();
            }
            Instantiate(rewardEffect1);
            Instantiate(rewardEffect2);
            Instantiate(rewardEffect3);
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
