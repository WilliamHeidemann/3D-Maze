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
            var newNearest = Nearest(_nearest);
            if (newNearest.Orientation != _nearest.Orientation) directionCalculator.HandleLogicalFaceChange(lastMoveDirection);
            _nearest = newNearest;

            directionCalculator.SetTargetRotationDirection(_target.Orientation);
            _mazeRotator.SetTarget(directionCalculator.GetRotation());
            
            transform.position = Vector3.MoveTowards(transform.position, _squareTransforms[_target].position, Time.deltaTime * 5.3f);
            
            if (_nearest == _objectiveSquare) AdvanceLevel();
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
                var calculatedDirection = directionCalculator.CalculateWorldDirection(direction);
                var (square, wall) = GetNeighbor(_nearest, calculatedDirection);
                if (square == null) continue;
                if (!wall.IsOpen) continue;
                Move(calculatedDirection, square, wall, direction);
                return;
            }
            var d = input.First();
            var calculatedD = directionCalculator.CalculateWorldDirection(d);
            var (s, w) = GetNeighbor(_nearest, calculatedD);
            if (s == null) return;
            Move(calculatedD, s, w, d);
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
        private void Move(CardinalDirection direction, Square square, Wall wall, CardinalDirection keyDirection)
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
