using UnityEngine;

namespace util
{
    public class FloatAnimator : MonoBehaviour
    {
        public Animator animator;
        public float damping = 0.25f;
        public string animatorParameter;
        private int _animatorParameterID;

        private float _value;
    
        private void Start() =>
            _animatorParameterID = Animator.StringToHash(animatorParameter);
    
        void Update()
        {
#if UNITY_EDITOR
            // Inspector changes
            _animatorParameterID = Animator.StringToHash(animatorParameter);
#endif     
            animator.SetFloat(_animatorParameterID, _value,damping, Time.deltaTime);
        }

        public void OnFloatInput(float value)
        {
            _value = value;
        }
    }
}