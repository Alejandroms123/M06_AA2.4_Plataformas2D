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

    private void OnEnable()
    {
        _masterVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", _defaultMasterVolume));
        _musicVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", _defaultMusicVolume));
        _effecsVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("EffectsVolume", _defaultEffectsVolume));
    }

    public void OnMasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
        SoundManager.Instance.SetMasterVolume(value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        SoundManager.Instance.SetMusicVolume(value);
    }

    public void OnEffectsVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("EffectsVolume", value);
        PlayerPrefs.Save();
        SoundManager.Instance.SetEffectsVolume(value);
    }
}