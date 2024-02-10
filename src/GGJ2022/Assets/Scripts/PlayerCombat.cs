
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerCombat : MonoBehaviour
{
    public UnityEvent playerShoot;
    
    [SerializeField] private GameObject redProjectileOrigin, whiteProjectileOrigin;
    [SerializeField] private GameObject redProjectile;
    [SerializeField] private GameObject whiteProjectile;
    private MovementAPI _movementAPI;
    public float shootingSpeed = 20f;
    [SerializeField] private float attackSpeed = 0.5f;

    private bool _shootingRed, _shootingWhite;
    private bool _redDominant;
    private float _lastShootTime = 0f;

    private void Start()
    {
        _movementAPI = GetComponent<MovementAPI>();
    }

    private void Update()
    {
        if (!(_shootingRed || _shootingWhite)) return;
        if (_lastShootTime + attackSpeed > Time.time) return;
        
        _lastShootTime = Time.time;

        var proj = _redDominant ? redProjectile : whiteProjectile;
        var transform1 = (_redDominant ? redProjectileOrigin : whiteProjectileOrigin).transform;
        var projectile = Instantiate(proj, transform1.position, transform.rotation).GetComponent<MovementAPI>();
        projectile.MoveDirection = Vector2.up + new Vector2(_movementAPI.MoveDirection.x, 0f) * 0.3f;
        projectile.speed = new Vector2(1,1) * shootingSpeed;
        
        playerShoot.Invoke();
    }

    public void OnShootRed(InputAction.CallbackContext context)
    {
        _shootingRed = context.ReadValueAsButton();
        _redDominant = _shootingRed;
    }
    
    public void OnShootWhite(InputAction.CallbackContext context)
    {
        _shootingWhite = context.ReadValueAsButton();
        _redDominant = !_shootingWhite;
    }
}
