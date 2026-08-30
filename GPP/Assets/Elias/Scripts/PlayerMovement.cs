using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{

    private Vector2 _moveInput = Vector2.zero;
    private float _verticalVelocity = 0f;
    private float _gravity = 9.81f;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CharacterController _Controller;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _fallingSpeed = 10f;
    [SerializeField] private float _jumpHeight = 5f;

    [Header("Audio Footsteps")]
    [SerializeField] private AudioSource _footstepsSfx;

    [SerializeField] private float _stepThreshold = 1.0f;

    private Vector3 _previousPosition; 
    private float _elapsedDistance = 0f;

    private void Start()
    {
        //Start Tracking from player's position
       _previousPosition = transform.position; 
    }
    void Update()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * _moveInput.y + right * _moveInput.x) * _moveSpeed;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), _rotationSpeed * Time.deltaTime);
        }

        //Apply gravity to player
        if (_Controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity -= _fallingSpeed;
        }
        else
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
        }

        moveDirection.y = _verticalVelocity;

        _Controller.Move(moveDirection * Time.deltaTime);

        //SND: Footsteps
        Vector3 delta = transform.position - _previousPosition;
        _elapsedDistance += delta.magnitude;

        if (_Controller.isGrounded && moveDirection.sqrMagnitude > 0.01f)
        {
            if (_elapsedDistance >= _stepThreshold)
            {
                if (_footstepsSfx != null)
                {
                    _footstepsSfx.Play();
                }
                _elapsedDistance %= _stepThreshold;
            }
                _previousPosition = transform.position; 
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if(_moveInput.SqrMagnitude() > 0.01f)
        {
            _moveInput.Normalize();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_Controller.isGrounded && context.performed)
        {
            _verticalVelocity = _jumpHeight;
        }
    }
}
