using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem _inputSystem;
    private PlayerMovement _movement;
    private PlayerLook _look;
    private PlayerWeaponController _weaponController;
    private PlayerKick _kick;

    private bool _gamepadSprintToggled;

    public Vector2 Move => _inputSystem.Player.Movement.ReadValue<Vector2>();
    public Vector2 Look => _inputSystem.Player.Look.ReadValue<Vector2>();
    public bool Interact => _inputSystem.Player.Interact.WasPressedThisFrame();

    private void Awake()
    {
        _inputSystem = new InputSystem();
        _movement = GetComponent<PlayerMovement>();
        _look = GetComponent<PlayerLook>();
        _weaponController = GetComponent<PlayerWeaponController>();
        _kick = GetComponent<PlayerKick>();

        _inputSystem.Player.Jump.performed += ctx => _movement.Jump();
        _inputSystem.Player.NextWeapon.performed += ctx => _weaponController.SwitchWeapon(1);
        _inputSystem.Player.PreviousWeapon.performed += ctx => _weaponController.SwitchWeapon(-1);

        _inputSystem.Player.Sprint.performed += OnSprintPerformed;
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
        _movement.SetCrouch(_inputSystem.Player.Crouch.IsPressed());

        _weaponController.ProcessFire(_inputSystem.Player.Fire.IsPressed(), _inputSystem.Player.AlternateFire.IsPressed());

        if (_inputSystem.Player.Reload.WasPressedThisFrame())
        {
            _weaponController.Reload();
        }

        if (_inputSystem.Player.Kick.WasPressedThisFrame())
        {
            _kick.TryKick();
        }
    }

    private void FixedUpdate()
    {
        Vector2 move = Move;

        if (move.sqrMagnitude < 0.01f)
        {
            _gamepadSprintToggled = false;
        }

        bool keyboardSprint =
            _inputSystem.Player.Sprint.IsPressed() &&
            _inputSystem.Player.Sprint.activeControl?.device is Keyboard;

        bool isSprinting = keyboardSprint || _gamepadSprintToggled;

        _movement.ProcessMove(move, isSprinting);
    }

    private void LateUpdate()
    {
        _look.ProcessLook(Look);
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        if (context.control.device is Gamepad)
        {
            _gamepadSprintToggled = !_gamepadSprintToggled;
        }
    }
}