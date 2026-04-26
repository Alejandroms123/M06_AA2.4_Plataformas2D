using UnityEngine;

public class YouWon : MonoBehaviour
{
    [SerializeField] private GameObject youWonUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            youWonUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}