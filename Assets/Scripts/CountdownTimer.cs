

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("Configuración del tiempo")]
    public float timeLimit = 10f;
    private float timeLeft;
    private bool timerStarted = false;

    [Header("Referencias")]
    public TextMeshProUGUI timerText;
    public PlayerController playerController; 
    public GameObject playerObject; 

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (!timerStarted && playerController.HasMoved)
        {
            timerStarted = true;
        }

        if (timerStarted)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerUI();

            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                timerStarted = false;
                ExplodeAndRestart();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = timeLeft.ToString("F1");
        }
    }

    private void ExplodeAndRestart()
    {
        if (playerObject != null)
        {
            playerObject.SetActive(false); // Muere el Player 
        }

        Invoke(nameof(RestartScene), 1f); // Espera 1 segundo y reinicia
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetTimer()
    {
        timeLeft = timeLimit;
        UpdateTimerUI();
    }
}
