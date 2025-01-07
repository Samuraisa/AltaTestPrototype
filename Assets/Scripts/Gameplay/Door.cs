using UnityEngine;

namespace Gameplay
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int AP_Open = Animator.StringToHash("Open");

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Consts.PlayerTag))
                animator.SetBool(AP_Open, true);
        }
    }
}