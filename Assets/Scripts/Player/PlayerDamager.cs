using System.Collections;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public static PlayerDamager instance;

    [SerializeField] private Collider2D _attackCol;
    [SerializeField] private float _attackDuration;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerVisuals.instance.animator.SetTrigger("Attack");
            PlayerVisuals.instance.stopOtherAnims = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
                enemy.Die();
            else
                other.gameObject.SetActive(false);
        }
    }

    public void Attack()
    {
        _attackCol.enabled = true;
        StartCoroutine(RunAfterTime(_attackDuration, StopAttack));
    }

    public void StopAttack()
    {
        _attackCol.enabled = false;
    }

    private IEnumerator RunAfterTime(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    private void OnDrawGizmos()
    {
        if (_attackCol == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackCol.bounds.center, _attackCol.bounds.size);
    }
}