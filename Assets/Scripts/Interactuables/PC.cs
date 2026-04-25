using UnityEngine;

public class PC : MonoBehaviour
{
    bool isPlayerNear = false;
    public GameObject trampa;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entra en el PC");
        isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Sale del PC");
        isPlayerNear = true;
    }

    private void Update()
    {
        if (isPlayerNear && (Input.GetKeyDown(KeyCode.E)))
        {
            Debug.Log("Trampa desactivada");
            trampa.GetComponent<Animator>().Play("Laser_Desactivated");
            trampa.GetComponent<Collider2D>().enabled = false;
        }
    }
}
