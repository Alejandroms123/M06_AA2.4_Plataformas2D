using UnityEngine;

public class Damagers : MonoBehaviour
{
    public int damage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<LifesManager>().getHurt(damage);

            PlayerVisuals visualScript = collision.GetComponent<PlayerVisuals>();
            visualScript.getHurt();
        }
    }
}
