using UnityEngine;

public class CharacterJump : MonoBehaviour
{

    [SerializeField, Range(0f, 1f)] private float _jumpCancelVelMultiplier;

    [Space(10)]

    [SerializeField, Range(0f, 20f)] private float _jumpForce;
    [SerializeField, Range(0f, 1f)] private float _jumpBufferTime;
    [SerializeField, Range(0f, 1f)] private float _coyoteTime;

    [Space(10)]

    [SerializeField, Range(0f, 10f)] private float _wallSlideSpeed;
    [SerializeField, Range(0f, 1f)] private float _wallJumpTime;
    [SerializeField] private Vector2 _wallJumpDir;

    [HideInInspector] public float _wallJumpTimer { get; private set; }

    private float _jumpBufferTimer;
    private float _coyoteTimer;

    public void UpdateJumpBuffers(bool jumpPressed, bool isGrounded, bool isOnWall)
    {
        _jumpBufferTimer = jumpPressed ? _jumpBufferTime : Mathf.Max(0f, _jumpBufferTimer - Time.deltaTime);
        _coyoteTimer = isGrounded ? _coyoteTime : Mathf.Max(0f, _coyoteTimer - Time.deltaTime);
        _wallJumpTimer = (_wallJumpTimer > 0f) ? Mathf.Max(0f, _wallJumpTimer - Time.deltaTime) : 0f;
    }

    public void JumpCancel(Rigidbody2D rb, bool jumpHeld)
    {
        if (!jumpHeld && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocityY *= _jumpCancelVelMultiplier;
        }
    }

    public void WallSlide(Rigidbody2D rb, bool isGrounded, bool isOnWall)
    {
        if (!isGrounded && isOnWall && rb.linearVelocity.y < _wallSlideSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -_wallSlideSpeed);
        }
    }

    public void HandleJump(Rigidbody2D rb, bool isGrounded, bool isOnWall, float lastMoveDir)
    {
        Vector2 dir;

        if (_jumpBufferTimer > 0f)
        {
            if (_coyoteTimer > 0f)
            {
                dir = new Vector2(rb.linearVelocity.x, _jumpForce);
                Jump(rb, dir);
            }
            else if (!isGrounded && isOnWall)
            {
                dir = new Vector2(_wallJumpDir.x * _jumpForce * -lastMoveDir, _wallJumpDir.y * _jumpForce);
                Jump(rb, dir);
                _wallJumpTimer = _wallJumpTime;
            }
        }
    }

    private void Jump(Rigidbody2D rb, Vector2 dir)
    {
        rb.linearVelocity = dir;
        _jumpBufferTimer = 0f;
        _coyoteTimer = 0f;
    }
}