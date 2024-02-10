using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class TriggerAnimator : MonoBehaviour
{
    public Animator animator;
    public string animatorParameter;
    private int animatorParameterID;

    void Start() =>
        animatorParameterID = Animator.StringToHash(animatorParameter);

#if UNITY_EDITOR
    void Update() => // Inspector changes
        animatorParameterID = Animator.StringToHash(animatorParameter);
#endif

    public void OnBooleanInput(InputAction.CallbackContext context)
        => OnBooleanInput();

    public void OnBooleanInput()
        => animator.SetTrigger(animatorParameterID);
}