using UnityEngine;

namespace util
{
    public class OnEnableAnimatorReset : MonoBehaviour
    {
        public Animator animator;
        
        public void ResetAnimator()
        {
            animator.Rebind();
            animator.Update(0);
        }
    }
}