using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;

    private float _xRotation = 0f;

    private CharacterController _controller;
    private float _standingHeight;
    private float _standingCameraY;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _standingHeight = _controller.height;
        _standingCameraY = camera.transform.localPosition.y;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        _xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

        FollowControllerHeight();
    }

    private void FollowControllerHeight()
    {
        Vector3 pos = camera.transform.localPosition;
        pos.y = _standingCameraY - (_standingHeight - _controller.height);
        camera.transform.localPosition = pos;
    }
}