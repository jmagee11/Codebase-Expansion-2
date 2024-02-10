using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementAPI : MonoBehaviour
{
    private static readonly int HorizontalSpeedID = Animator.StringToHash("horizontal_speed");
    private static readonly int VerticalSpeedID = Animator.StringToHash("vertical_speed");

    public Vector2 startDirection;
    
    private Vector2 _moveDirection;

    public Vector2 speed = new Vector2(20, 20);

    [SerializeField] private List<Animator> animators;
    [SerializeField] private Vector2 animatorDampTime = new Vector2(0.1f, 0.1f);

    private void Awake() =>
        _moveDirection = startDirection;

    public Vector2 MoveDirection
    {
        get => _moveDirection;
        set => _moveDirection = new Vector2(
            Mathf.Min(Mathf.Max(value.x, -1), 1),
            Mathf.Min(Mathf.Max(value.y, -1), 1)
        );
    }

    private void Update()
    {
        transform.position += transform.TransformDirection((Vector3)(_moveDirection * speed * Time.deltaTime));
        foreach (var animator in animators)
        {
            animator.SetFloat(HorizontalSpeedID, MoveDirection.x, animatorDampTime.x, Time.deltaTime);
            animator.SetFloat(VerticalSpeedID, MoveDirection.y, animatorDampTime.y, Time.deltaTime);
        }
    }
}