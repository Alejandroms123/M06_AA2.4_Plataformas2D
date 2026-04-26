using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _effecsVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    [SerializeField, Range(0f, 1f)] private float _defaultMasterVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _defaultMusicVolume = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _defaultEffectsVolume = 0.5f;

    private void Start()
    {
        OnMasterVolumeChanged(_masterVolumeSlider.value);
        OnMusicVolumeChanged(_musicVolumeSlider.value);
        OnEffectsVolumeChanged(_effecsVolumeSlider.value);
    }

    private void OnEnable()
    {
        _masterVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVolume", _defaultMasterVolume));
        _musicVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", _defaultMusicVolume));
        _effecsVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("EffectsVolume", _defaultEffectsVolume));
    }

    public void OnMasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
    }

    public void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
    }

    public void OnEffectsVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("EffectsVolume", value);
        _audioMixer.SetFloat("SfxVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
    }
}