using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _moveSpeed;
    [SerializeField, Range(0f, 100f)] private float _acceleration;
    [SerializeField, Range(0f, 100f)] private float _deceleration;

    float velocityXSmoothing;

    public void Move(Rigidbody2D rb, float moveInput)
    {
        float targetVelocityX = moveInput * _moveSpeed;
        float accelerationRate = (Mathf.Abs(targetVelocityX) > 0.01f) ? _acceleration : _deceleration;
        float newVelocityX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityXSmoothing, accelerationRate * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
    }
}