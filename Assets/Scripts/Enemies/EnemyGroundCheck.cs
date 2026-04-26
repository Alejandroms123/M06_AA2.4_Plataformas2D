using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    public bool isGrounded { get; private set; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<BoxCollider2D>();
        if (col == null) return;

        if (isGrounded)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, col.bounds.size);
    }
}