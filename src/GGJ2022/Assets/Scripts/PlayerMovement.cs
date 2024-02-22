using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using util.sound;

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

    [SerializeField] SfxSoundLibraryUser splitSound;
    [SerializeField] SfxSoundLibraryUser connectSound;

    [Header("Fields for Combat")]

    [SerializeField] private GameObject redProjectileOrigin, whiteProjectileOrigin;
    [SerializeField] private GameObject redProjectile;
    [SerializeField] private GameObject whiteProjectile;
    [SerializeField] SfxSoundLibraryUser shootSound;
    //private MovementAPI _movementAPI;
    public float shootingSpeed = 20f;
    [SerializeField] private float attackSpeed = 0.5f;

    private BooleanAnimator[] animators;
    private bool _shootingRed, _shootingWhite;
    private bool _redDominant;
    private float _lastShootTime = 0f;

    private void Start()
    {
        //holds all the animators that are on the player
        animators = GetComponents<BooleanAnimator>();

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

        //Update function from PlayerCombat
        if (!(_shootingRed || _shootingWhite)) return;
        if (_lastShootTime + attackSpeed > Time.time) return;

        _lastShootTime = Time.time;

        var proj = _redDominant ? redProjectile : whiteProjectile;
        var transform1 = (_redDominant ? redProjectileOrigin : whiteProjectileOrigin).transform;
        var projectile = Instantiate(proj, transform1.position, transform.rotation).GetComponent<MovementAPI>();
        projectile.MoveDirection = Vector2.up + new Vector2(movementAPI.MoveDirection.x, 0f) * 0.3f;
        projectile.speed = new Vector2(1, 1) * shootingSpeed;

        shootSound.PlaySound();
    }

    //method from Player Combat
    public void OnShootRed(InputAction.CallbackContext context)
    {
        _shootingRed = context.ReadValueAsButton();
        _redDominant = _shootingRed;
    }

    //method from Player Combat
    public void OnShootWhite(InputAction.CallbackContext context)
    {
        _shootingWhite = context.ReadValueAsButton();
        _redDominant = !_shootingWhite;
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

            //removal of the onDashLeft event
            animators[1].OnBooleanInput(true);
            animators[4].OnBooleanInput(true);

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

            //removal of the onDashRight event
            animators[2].OnBooleanInput(true);
            animators[3].OnBooleanInput (true);
            
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

            //removale of onDashUp Event
            animators[5].OnBooleanInput(true);
            animators[6].OnBooleanInput(true);

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

    //get rid of the onSepperate and onConnect events
    public void OnSplit(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            animators[0].OnBooleanInput(true);
            splitSound.PlaySound();
        }
        else
        {
            animators[0].OnBooleanInput(false);
            connectSound.PlaySound();

        }
    }

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