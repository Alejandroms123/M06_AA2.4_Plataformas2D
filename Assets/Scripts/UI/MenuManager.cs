using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public static MenuManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject _settingsPanel;
    public GameObject _otherUI;

    private bool _isMainMenu => SceneManager.GetActiveScene().buildIndex == 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleSettings();
    }

    public void Play() => StartCoroutine(LoadSceneAfterSound(1));
    public void Credits() => StartCoroutine(LoadSceneAfterSound(2));

    public void ReturnToMainMenu()
    {
        _audioSource.Play();
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAfterSound(0));
    }

    private IEnumerator LoadSceneAfterSound(int sceneIndex)
    {
        _audioSource.Play();
        yield return new WaitForSecondsRealtime(_audioSource.clip.length);
        SceneManager.LoadScene(sceneIndex);
    }

    public void ToggleSettings()
    {
        bool settingsActive = !_settingsPanel.activeSelf;
        _settingsPanel.SetActive(settingsActive);
        if (_isMainMenu)
            _otherUI.SetActive(!settingsActive);
        Time.timeScale = settingsActive ? 0f : 1f;
        AudioListener.pause = false;
        _audioSource.Play();
    }

    public void Exit()
    {
        _audioSource.Play();
        Application.Quit();
    }
}