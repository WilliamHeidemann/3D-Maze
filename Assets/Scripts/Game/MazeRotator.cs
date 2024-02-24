using System;
using Graph;
using UnityEngine;

namespace Game
{
    public class MazeRotator : MonoBehaviour
    {
        private Quaternion target;
        [SerializeField] [Range(1f, 300f)] private float rotationSpeed;
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
    }
}