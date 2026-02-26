using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    [Header("Jump properties")]
    [SerializeField, Range(0f, 20f)] private float _jumpForce;
    [SerializeField, Range(0f, 1f)] private float _jumpCancelVelMultiplier;
    [SerializeField, Range(0f, 1f)] private float _jumpBufferTime;
    [SerializeField, Range(0f, 1f)] private float _coyoteTime;

    [Space(10), Header("Walljump properties")]
    [SerializeField, Range(0f, 10f)] private float _wallSlideSpeed;
    [SerializeField, Range(0f, 10f)] private float _climbSpeed;
    [SerializeField] private Vector2 _wallJumpDir;

    private float _jumpBufferTimer;
    private float _coyoteTimer;

    public void UpdateJump(bool jumpPressed, bool isGrounded)
    {
        _jumpBufferTimer = jumpPressed ? _jumpBufferTime : Mathf.Max(0f, _jumpBufferTimer - Time.deltaTime);
        _coyoteTimer = isGrounded ? _coyoteTime : Mathf.Max(0f, _coyoteTimer - Time.deltaTime);
    }

    public void JumpCancel(Rigidbody2D rb, bool jumpHeld)
    {
        if (!jumpHeld && rb.linearVelocity.y > 0f)
            rb.linearVelocityY *= _jumpCancelVelMultiplier;
    }

    public void HandleJump(Rigidbody2D rb, bool isWallJump, float lastMoveDir)
    {
        Vector2 dir;

        if (_jumpBufferTimer > 0f)
        {
            if (_coyoteTimer > 0f)
            {
                dir = new Vector2(rb.linearVelocity.x, _jumpForce);
                Jump(rb, dir);
            }
            else if (isWallJump && StaminaManager.instance.stamina != 0f)
            {
                StaminaManager.instance.LoseStamina();
                dir = new Vector2(_wallJumpDir.x * _jumpForce * -lastMoveDir, _wallJumpDir.y * _jumpForce);
                Jump(rb, dir);
            }
        }
    }

    private void Jump(Rigidbody2D rb, Vector2 dir)
    {
        rb.linearVelocity = dir;
        _jumpBufferTimer = 0f;
        _coyoteTimer = 0f;
    }

    public void Climb(Rigidbody2D rb, float dir)
    {
        if (dir != 0f)
            rb.linearVelocityY = dir * _climbSpeed;
    }

    public void WallSlide(Rigidbody2D rb)
    {
        if (rb.linearVelocityY <= 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocityX, -_wallSlideSpeed);
    }
}