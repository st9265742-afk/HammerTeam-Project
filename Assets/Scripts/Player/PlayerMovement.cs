using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.8f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 3f;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForce = 5f;
    [SerializeField] private float wallJumpDuration = 0.15f;
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private int wallJumpLimit = 1;

    private bool _isGrounded = false;
    private Vector3 _playerVelocity;
    private Vector3 _wallJumpVelocity;
    private float _wallJumpTimer;
    private int _wallJumpsUsed;

    private CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded)
        {
            _wallJumpsUsed = 0;
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        moveDirection = transform.TransformDirection(moveDirection) * speed;

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

    public void Jump()
    {
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
}