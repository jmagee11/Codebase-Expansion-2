using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class BooleanAnimator : MonoBehaviour
{
    public Animator animator;
    public string animatorParameter;
    private int animatorParameterID;

    public bool inverse;

    void Start() =>
        animatorParameterID = Animator.StringToHash(animatorParameter);

#if UNITY_EDITOR
    void Update() => // Inspector changes
        animatorParameterID = Animator.StringToHash(animatorParameter);
#endif

    public void OnBooleanInput(InputAction.CallbackContext context)
        => OnBooleanInput(context.ReadValueAsButton());
    
    public void OnBooleanInput(bool context) 
        => animator.SetBool(animatorParameterID, context != inverse);
}