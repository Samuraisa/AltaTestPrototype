using UnityEngine;

namespace Alta.Gameplay
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Player player;
        [SerializeField] private float openDoorAtDistance = 5f;
        
        private static readonly int AP_Open = Animator.StringToHash("Open");

        private void LateUpdate()
        {
            var distance = Vector3.Distance(player.transform.position, transform.position);
            animator.SetBool(AP_Open, distance <= openDoorAtDistance);
        }
    }
}