using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSystem _inputSystem;
    private PlayerMovement _movement;
    private PlayerLook _look;
    private PlayerWeaponController _weaponController;

    public Vector2 Move => _inputSystem.Player.Movement.ReadValue<Vector2>();
    public Vector2 Look => _inputSystem.Player.Look.ReadValue<Vector2>();

    private void Awake()
    {
        _inputSystem = new InputSystem();
        _movement = GetComponent<PlayerMovement>();
        _look = GetComponent<PlayerLook>();
        _weaponController = GetComponent<PlayerWeaponController>();

        _inputSystem.Player.Jump.performed += ctx => _movement.Jump();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Enable();
    }

    private void OnDisable()
    {
        _inputSystem.Player.Disable();
    }

    private void OnDestroy()
    {
        _inputSystem?.Dispose();
    }

    private void Update()
    {
        _weaponController.ProcessFire(_inputSystem.Player.Fire.IsPressed(), _inputSystem.Player.AlternateFire.IsPressed());

        if (_inputSystem.Player.Reload.WasPressedThisFrame())
        {
            _weaponController.Reload();
        }
    }

    private void FixedUpdate()
    {
        _movement.ProcessMove(Move);
    }

    private void LateUpdate()
    {
        _look.ProcessLook(Look);
    }
}