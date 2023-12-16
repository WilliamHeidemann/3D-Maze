using UnityEngine;

namespace Game
{
    public class CameraLock : MonoBehaviour
    {
        private Transform _target;
        [SerializeField] private float offset;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (!_target) return;
            var targetPosition = _target.position;
            transform.position = new Vector3(targetPosition.x, targetPosition.y, offset);
        }
    }
}
