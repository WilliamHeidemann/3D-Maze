using System;
using Graph;
using UnityEngine;

namespace Game
{
    public class MazeRotator : MonoBehaviour
    {
        private Quaternion target;
        [SerializeField] [Range(1f, 300f)] private float rotationSpeed;
        private SoundManager soundManager;

        private void Start()
        {
            soundManager = FindObjectOfType<SoundManager>();
        }

        private void Update()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
            soundManager.PlayTurningNoise(IsStill);
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

        private bool IsStill => AnglesFromTarget < 1f;
        private float AnglesFromTarget => Quaternion.Angle(transform.rotation, target);
    }
}