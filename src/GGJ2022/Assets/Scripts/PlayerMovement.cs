using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MovementAPI movementAPI;
    [SerializeField] private BoxCollider2D dimensions;
    [SerializeField] private GameStateHandler GameStateHandler;
    [SerializeField] private float limitUpper = 0.45f;
    [SerializeField] private float limitLower = 0.45f;
    [SerializeField] private float limitSide = 0.8f;
    [SerializeField] private Vector2 dashSpeed = new Vector2(1f, 3f);
    private float moveSpeed = 20f;
    private float _sideBounds;
    private float _lowerBounds;
    private float _upperBounds;
    private string _controlScheme;
    [SerializeField] private float dashDelay = 0.5f;
    private float DashTimer;
    private bool Dashing;
    public UnityEvent onSepperate;
    public UnityEvent onConnect;
    public UnityEvent onDashLeft;
    public UnityEvent onDashRight;
    public UnityEvent onDashUp;
    
    private void Start()
    {
        movementAPI = GetComponent<MovementAPI>();
        moveSpeed = movementAPI.speed.x;
        var size = dimensions.size;
        _sideBounds = size.x/2f * limitSide;
        _lowerBounds = size.y * -limitLower;
        _upperBounds = size.y * limitUpper;
    }

    private void Update()
    {
        if(DashTimer > 0f) DashTimer -= Time.deltaTime;
        movementAPI.MoveDirection = CheckBounds(movementAPI.MoveDirection);
    }

    private void OnEnable()
    {
        EliasPlayer.instance.RunActionPreset("IntroStart");
    }

    // Input events
    public void OnMoveDirection(InputAction.CallbackContext context)
    {
        if(Dashing) return;
        var input = context.ReadValue<Vector2>();
        input = CheckBounds(input);
        movementAPI.MoveDirection = input;
    }

    public void OnDashLeft(InputAction.CallbackContext context)
    {
        if(DashTimer > 0f) return;
        if (context.ReadValueAsButton())
        {
            Dashing = true;
            DashTimer = dashDelay;
            onDashLeft.Invoke();
            movementAPI.MoveDirection = Vector2.left;
            movementAPI.speed += movementAPI.speed * Vector2.right * dashSpeed.x;
        }
    }
    
    public void OnDashRight(InputAction.CallbackContext context)
    {
        if(DashTimer > 0f) return;
        if (context.ReadValueAsButton())
        {
            DashTimer = dashDelay;
            Dashing = true;
            onDashRight.Invoke();
            movementAPI.MoveDirection = Vector2.right;
            movementAPI.speed += movementAPI.speed * Vector2.right * dashSpeed.x;
        }
    }

    public void OnDashUp(InputAction.CallbackContext context)
    {
        if(DashTimer > 0f) return;
        
        if (context.ReadValueAsButton())
        {
            DashTimer = dashDelay;
            Dashing = true;
            onDashUp.Invoke();
            movementAPI.MoveDirection = Vector2.up;
            movementAPI.speed += movementAPI.speed * Vector2.up * dashSpeed.y;
        }
    }

    public void StopDashing()
    {
        movementAPI.speed = Vector2.one * moveSpeed;
        movementAPI.MoveDirection = Vector2.zero;
        Dashing = false;
    }

    public void OnSplit(InputAction.CallbackContext context) =>
        (context.ReadValueAsButton() ? onSepperate : onConnect).Invoke();

    public void LockMouse(PlayerInput playerInput)
    {
        _controlScheme = playerInput.currentControlScheme;
        if (_controlScheme == "Mouse")
        {
            if (GameStateHandler.InGame)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private Vector2 CheckBounds(Vector2 direction)
    {
        var output = direction;
        if (transform.position.x > _sideBounds && output.x > 0f)
            output = new Vector2(0, output.y);
        if (transform.position.x < -_sideBounds && output.x < 0f)
            output = new Vector2(0, output.y);
        if (transform.position.y > _upperBounds && output.y > 0f)
            output = new Vector2(output.x, 0);
        if (transform.position.y < _lowerBounds && output.y < 0f)
            output = new Vector2(output.x, 0);
        return output;
    }
}