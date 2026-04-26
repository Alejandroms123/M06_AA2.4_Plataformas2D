using UnityEngine;

public class Damagers : MonoBehaviour
{
    public int damage;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LifesManager.Instance._isInvulnerable) return;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<LifesManager>().getHurt(damage);

            CharacterKnockout knockoutScript = collision.GetComponent<CharacterKnockout>();
            float dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
            knockoutScript.ApplyKnockout(dir);

            PlayerVisuals visualScript = collision.GetComponent<PlayerVisuals>();
            visualScript.getHurt();
        }
    }
}