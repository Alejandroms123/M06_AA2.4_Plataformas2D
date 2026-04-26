using UnityEngine;

public class TutoTrigger : MonoBehaviour
{
    [SerializeField] private string _message = "Pulsa ESPACIO para saltar";
    [SerializeField] private Collider2D _col;
    [SerializeField] private AudioSource _computerSound;
    private bool _isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            WorldHintUI.Instance.ShowHint(_message);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            WorldHintUI.Instance.HideHint();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isPlayerInside)
        {
            WorldHintUI.Instance.HideHint();
            _computerSound.Play();
            _col.enabled = false;
        }
    }
}