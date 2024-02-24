using System;
using Graph;
using UnityEngine;

namespace Game
{
    public class MazeRotator : MonoBehaviour
    {
        private Quaternion target;
        [SerializeField] [Range(1f, 300f)] private float rotationSpeed;
        private CardinalDirection TargetDirection { get; set; }
        private void Update()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

        public void SnapToFace(Orientation face)
        {
            target = face switch
            {
                Orientation.Front => Quaternion.Euler(0,0,0),
                Orientation.Back => Quaternion.Euler(180, 0, 180),
                Orientation.Right => Quaternion.Euler(0, 90, 0),
                Orientation.Left => Quaternion.Euler(0, -90, 0),
                Orientation.Up => Quaternion.Euler(-90, 0, 0),
                Orientation.Down => Quaternion.Euler(90, 0, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
            };
            transform.rotation = target;
        }

        public void SetTarget(Quaternion targetRotation)
        {
            target = targetRotation;
        }

        public Quaternion Target => target;
        public void SetTarget(CardinalDirection direction)
        {
            target = Get90DegreeRotation(direction);
            TargetDirection = direction;
        }

        public void GoBack()
        {
            target = Get90DegreeRotation(TargetDirection.Opposite());
            TargetDirection = TargetDirection.Opposite();
        }

        private Quaternion Get90DegreeRotation(CardinalDirection direction)
        {
            var axis = GetRotationAxis(direction);
            var angle = GetRotationAngle(direction);
            var rotationQuaternion = Quaternion.AngleAxis(angle, axis);
            return rotationQuaternion * target;
        }

        private static float GetRotationAngle(CardinalDirection direction) => direction switch
        {
            CardinalDirection.North => -90f,
            CardinalDirection.South => 90,
            CardinalDirection.East => 90,
            CardinalDirection.West => -90f,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        private static Vector3 GetRotationAxis(CardinalDirection direction) => direction switch
        {
            CardinalDirection.North => Vector3.right,
            CardinalDirection.South => Vector3.right,
            CardinalDirection.East => Vector3.up,
            CardinalDirection.West => Vector3.up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}