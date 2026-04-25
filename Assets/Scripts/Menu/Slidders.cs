using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _effecsVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    [SerializeField, Range(0f, 1f)] private float _defaultMasterVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _defaultMusicVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _defaultEffectsVolume = 0.5f;

    private void Start()
    {
        _masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", _defaultMasterVolume);
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", _defaultMusicVolume);
        _effecsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume", _defaultEffectsVolume);

        SoundManager.Instance.SetMasterVolume(_masterVolumeSlider.value);
        SoundManager.Instance.SetMusicVolume(_musicVolumeSlider.value);
        SoundManager.Instance.SetEffectsVolume(_effecsVolumeSlider.value);
    }

    public void OnMasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        SoundManager.Instance.SetMasterVolume(value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        SoundManager.Instance.SetMusicVolume(value);
    }

    public void OnEffectsVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("EffectsVolume", value);
        SoundManager.Instance.SetEffectsVolume(value);
    }
}