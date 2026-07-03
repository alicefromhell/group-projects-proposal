using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100f;

    private float _xRotation;
    private float _yRotation;

    private Vector2 _rotationInput = Vector2.zero;

    public void OnCameraRotate(InputAction.CallbackContext context)
    {
        _rotationInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        float mouseX = _rotationInput.x * _rotationSpeed * Time.deltaTime;
        float mouseY = _rotationInput.y * _rotationSpeed * Time.deltaTime;

        _yRotation += mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
    }
}
