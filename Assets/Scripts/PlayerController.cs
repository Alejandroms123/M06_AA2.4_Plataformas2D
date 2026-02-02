using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] CharacterJump _characterJump;
    [SerializeField] CharacterCheck _characterCheck;
    [SerializeField] CharacterMovement _characterMovement;

    [Space(10)]

    [SerializeField] Collider2D _col;
    [SerializeField] Rigidbody2D _rb;

    public float _lastMoveDir { get; private set; }

    private bool _isGrounded;
    private bool _isOnwall;
    private float _moveInput;

    private void Update()
    {
        _playerInput.GatherInput();

        _moveInput = _playerInput._moveInput;
        _lastMoveDir = (_moveInput != 0) ? Mathf.Sign(_moveInput) : _lastMoveDir;

        _isGrounded = _characterCheck.IsGrounded(_col, _rb);
        _isOnwall = _characterCheck.IsOnWall(_col, _rb, _lastMoveDir);

        bool jumpPressed = _playerInput._jumpPressed;
        bool jumpHeld = _playerInput._jumpHeld;

        _characterJump.UpdateJumpBuffers(jumpPressed, _isGrounded, _isOnwall);
    }

    private void FixedUpdate()
    {
        float wallJumpTimer = _playerJump._wallJumpTimer;

        if (wallJumpTimer <= 0f)
        {
            _characterMovement.Move(_rb, _moveInput);
        }

        _characterJump.WallSlide(_rb, _isGrounded, _isOnwall);
        _characterJump.HandleJump(_rb, _isGrounded, _isOnwall, _lastMoveDir);
        _characterJump.JumpCancel(_rb, _playerInput._jumpHeld);
    }
}