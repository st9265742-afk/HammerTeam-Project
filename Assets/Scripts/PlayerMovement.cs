using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float wallJumpForce = 5f;
    public float wallJumpDuration = 0.15f;
    public float wallCheckDistance = 0.6f;
    public int _wallJumpLimit = 1;

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

        if (_wallJumpsUsed >= _wallJumpLimit)
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