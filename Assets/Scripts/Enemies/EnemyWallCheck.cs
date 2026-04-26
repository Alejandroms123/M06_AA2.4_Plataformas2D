using UnityEngine;

public class EnemyWallCheck : MonoBehaviour
{
    public bool wallInFront { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            wallInFront = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            wallInFront = false;
    }

    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<BoxCollider2D>();
        if (col == null) return;

        if (wallInFront)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, col.bounds.size);
    }
}