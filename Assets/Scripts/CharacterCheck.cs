using UnityEngine;



public class CharacterCheck : MonoBehaviour

{

    public bool IsGrounded(Collider2D col, Rigidbody2D rb)

    {

        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size * 0.6f, 0f, Vector2.down, 0.15f, LayerMask.GetMask("Ground"));

        return hit.collider != null;

    }



    public bool IsOnWall(Collider2D col, Rigidbody2D rb, float lastMoveDir)

    {

        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size * 0.6f, 0f, Vector2.right * lastMoveDir, 0.09f, LayerMask.GetMask("Ground"));

        return hit.collider != null;

    }

}
