using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float gravity = -9.8f;

    [Header("Crouch")]
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = 8f;
    [SerializeField] private LayerMask ceilingMask = ~0;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 3f;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForce = 5f;
    [SerializeField] private float wallJumpDuration = 0.15f;
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private int wallJumpLimit = 1;

    private bool _isGrounded = false;
    private Vector3 _playerVelocity;
    private bool _wantsCrouch;
    private bool _isCrouching;
    private float _standingHeight;
    private float _standingCenterY;
    private Vector3 _wallJumpVelocity;
    private float _wallJumpTimer;
    private int _wallJumpsUsed;
    private float _boostMultiplier = 1f;
    private float _boostTimer;

    private CharacterController _controller;

    public event Action<bool> SpeedBoostChanged;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _standingHeight = _controller.height;
        _standingCenterY = _controller.center.y;
    }

    private void Update()
    {
        if (_boostTimer > 0f)
        {
            _boostTimer -= Time.deltaTime;

            if (_boostTimer <= 0f)
            {
                _boostTimer = 0f;
                _boostMultiplier = 1f;
                SpeedBoostChanged?.Invoke(false);
            }
        }

        _isGrounded = _controller.isGrounded;

        if (_isGrounded)
        {
            _wallJumpsUsed = 0;
        }

        UpdateCrouch();
    }

    public void ProcessMove(Vector2 input, bool isSprinting)
    {
        float currentSpeed = (_isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed)) * _boostMultiplier;

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = transform.TransformDirection(moveDirection) * currentSpeed;

        _playerVelocity.y += gravity * Time.deltaTime;

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }

        if (_wallJumpTimer > 0)
        {
            _wallJumpTimer -= Time.deltaTime;
        }
        else
        {
            _wallJumpVelocity = Vector3.zero;
        }

        Vector3 finalVelocity = moveDirection + _wallJumpVelocity + Vector3.up * _playerVelocity.y;
        _controller.Move(finalVelocity * Time.deltaTime);
    }

    public void SetCrouch(bool crouch)
    {
        _wantsCrouch = crouch;
    }

    private void UpdateCrouch()
    {
        if (_wantsCrouch)
        {
            _isCrouching = true;
        }
        else if (_isCrouching && CanStandUp())
        {
            _isCrouching = false;
        }

        float targetHeight = _isCrouching ? crouchHeight : _standingHeight;
        float newHeight = Mathf.MoveTowards(_controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);
        float delta = _standingHeight - newHeight;

        _controller.height = newHeight;
        _controller.center = new Vector3(_controller.center.x, _standingCenterY - delta / 2f, _controller.center.z);
    }

    private bool CanStandUp()
    {
        float radius = _controller.radius;
        Vector3 start = transform.position + Vector3.up * (_controller.height - radius);
        float distance = _standingHeight - _controller.height;

        return !Physics.SphereCast(start, radius * 0.95f, Vector3.up, out _, distance, ceilingMask, QueryTriggerInteraction.Ignore);
    }

    public void Jump()
    {
        if (_isCrouching) return;

        if (_isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            return;
        }

        if (_wallJumpsUsed >= wallJumpLimit)
        {
            return;
        }

        if (TryGetWallNormal(out Vector3 wallNormal))
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            _wallJumpVelocity = wallNormal * wallJumpForce;
            _wallJumpTimer = wallJumpDuration;

            _wallJumpsUsed++;
        }
    }

    private bool TryGetWallNormal(out Vector3 wallNormal)
    {
        Vector3 origin = transform.position + Vector3.up * (_controller.height / 2f);

        if (Physics.SphereCast(origin, _controller.radius, transform.forward, out RaycastHit hit, wallCheckDistance))
        {
            wallNormal = hit.normal;
            return true;
        }

        if (Physics.SphereCast(origin, _controller.radius, -transform.forward, out hit, wallCheckDistance))
        {
            wallNormal = hit.normal;
            return true;
        }

        if (Physics.SphereCast(origin, _controller.radius, transform.right, out hit, wallCheckDistance))
        {
            wallNormal = hit.normal;
            return true;
        }

        if (Physics.SphereCast(origin, _controller.radius, -transform.right, out hit, wallCheckDistance))
        {
            wallNormal = hit.normal;
            return true;
        }

        wallNormal = Vector3.zero;
        return false;
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        _boostMultiplier = multiplier;
        _boostTimer = duration;

        SpeedBoostChanged?.Invoke(true);
    }
}