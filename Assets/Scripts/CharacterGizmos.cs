using UnityEngine;

public class CharacterGizmos : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [Space(10)]
    [SerializeField] Collider2D _col;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * 0.12f, _col.bounds.size * 0.8f);

        Gizmos.color = Color.green;
        Vector3 lastMoveDir = Vector3.right * _playerController._lastMoveDir;
        Gizmos.DrawWireCube(transform.position + lastMoveDir * 0.08f, _col.bounds.size * 0.8f);
    }
}