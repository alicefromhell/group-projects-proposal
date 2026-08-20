using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    private Vector2 _moveInput = Vector2.zero;
    [SerializeField] float _Speed = 10f;

    void Update()
    {
        Vector3 pos = new Vector3( _moveInput.x,  0, _moveInput.y);
        transform.position += pos * _Speed * Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if (_moveInput.SqrMagnitude() > 0.01f)
        {
            _moveInput.Normalize();
        }
    }
}
