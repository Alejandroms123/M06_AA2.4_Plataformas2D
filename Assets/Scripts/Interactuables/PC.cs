using System.Collections;
using UnityEngine;

public class PC : MonoBehaviour
{
    bool isPlayerNear = false;
    public GameObject trampa;
    [SerializeField] private AudioSource _trapSound;
    [SerializeField] private AudioSource _computerSound;
    [SerializeReference] private ParticleSystem _trapParticles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPlayerNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerNear = true;
    }

    private void Update()
    {
        if (isPlayerNear && (Input.GetKeyDown(KeyCode.E)))
        {
            _computerSound.Stop();
            StartCoroutine(DesactivarTrampa());
        }
    }

    private IEnumerator DesactivarTrampa()
    {
        yield return new WaitForSeconds(0.5f);
        trampa.GetComponent<Animator>().Play("Laser_Desactivated");
        trampa.GetComponent<AudioSource>().Stop();
        trampa.GetComponent<Collider2D>().enabled = false;
        _trapParticles.Stop();
        _trapSound.Play();
    }
}