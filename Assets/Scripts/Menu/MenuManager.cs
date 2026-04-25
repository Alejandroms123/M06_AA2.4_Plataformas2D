using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
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
        SoundManager.Instance.PlaySound(SoundType.Button);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private IEnumerator LoadSceneAfterSound(int sceneIndex)
    {
        SoundManager.Instance.PlaySound(SoundType.Button);
        yield return new WaitForSecondsRealtime(SoundManager.Instance.GetSoundDuration(SoundType.Button));
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
        SoundManager.Instance.PlaySound(SoundType.Button);
    }

    public void Exit()
    {
        SoundManager.Instance.PlaySound(SoundType.Button);
        Application.Quit();
    }
}