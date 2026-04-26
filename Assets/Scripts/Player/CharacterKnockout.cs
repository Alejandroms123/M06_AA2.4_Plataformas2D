using UnityEngine;

public class CharacterKnockout : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _knockbackX;
    [SerializeField, Range(0f, 100f)] private float _knockbackY;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private AudioSource _knockoutSound;


    public void ApplyKnockout(float dir, Rigidbody2D rb = null)
    {
        Rigidbody2D targetRb = rb ?? _rb;
        targetRb.linearVelocity = new Vector2(dir * _knockbackX, _knockbackY);
        _knockoutSound.Play();
    }
}