using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move properties")]
    [SerializeField, Range(0f, 10f)] private float _maxXSpeed;
    [SerializeField, Range(0f, 10f)] private float _acceleration;
    [SerializeField, Range(0f, 10f)] private float _deceleration;

    private float _velocityXSmoothing;

    public void MoveX(Rigidbody2D rb, float dir)
    {
        float targetVelocityX = dir * _maxXSpeed;
        float smoothTime = (Mathf.Abs(targetVelocityX) > 0.01f) ? (1f / _acceleration) : (1f / _deceleration);
        float newVelocityX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref _velocityXSmoothing, smoothTime);
        rb.linearVelocityX = newVelocityX;
    }

    public void ApplyKnockout(Rigidbody2D rb, float hit, float _kockbackX, float _kockbackY)
    {
        float dir = transform.position.x < hit ? -1 : 1;
        Vector2 knockback = new Vector2(dir * _kockbackX, _kockbackY);
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = knockback;
    }

    public void FlipSr(SpriteRenderer sr, float dir) { sr.flipX = sr.flipX = -dir > 0f; }
}