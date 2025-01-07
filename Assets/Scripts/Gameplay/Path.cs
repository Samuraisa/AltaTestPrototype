using Extensions;
using UnityEngine;

namespace Gameplay
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private Sphere playerSphere;
        [SerializeField] private Transform targetTransform;
        [SerializeField, Range(0f, 0.5f)] private float offsetFromGround = 0.01f;

        private void Start()
        {
            UpdatePosition();
            UpdateWidth();
        }

        [ContextMenu("Update")]
        void LateUpdate()
        {
            UpdatePosition();
            UpdateWidth();
        }

        private void UpdatePosition()
        {
            var t = transform;
            var playerPosition = playerSphere.transform.position.WithY(offsetFromGround);
            var targetPosition = targetTransform.position.WithY(offsetFromGround);
            var rotation = Quaternion.LookRotation(targetPosition - playerPosition);
            t.SetLocalPositionAndRotation(playerPosition, rotation);
            t.localScale = t.localScale.WithZ(Vector3.Distance(targetPosition, playerPosition));
        }

        private void UpdateWidth()
        {
            transform.localScale = transform.localScale.WithX(playerSphere.Radius * 2f);
        }
    }
}