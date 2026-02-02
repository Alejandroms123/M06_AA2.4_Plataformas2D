using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float _moveInput { get; private set; }
    public bool _jumpPressed { get; private set; }
    public bool _jumpHeld { get; private set; }

    public void GatherInput()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
        _jumpPressed = Input.GetButtonDown("Jump");
        _jumpHeld = Input.GetButton("Jump");
    }
}