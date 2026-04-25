using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    Steps, Jump, Land, Slide, Attack, Hit, Death, Respawn,
    EnemyDeath, EnemySlimeSteps, EnemyFlyerFlaps,
    MenuMusic, Button
}

[System.Serializable]
public class SoundData
{
    public SoundType soundType;
    public AudioClip[] clips;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;
    public bool randomizePitch;
    public bool is3D = false;
    public float minDistance = 0.3f;
    public float maxDistance = 3f;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _effectsGroup;
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _loopSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private SoundData[] _sounds;

    [SerializeField] private int _poolSize = 15;
    private AudioSource[] _audioPool;
    private int _poolIndex = 0;
    private Coroutine _loopFade;
    private AudioClip _lastClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _loopSource.ignoreListenerPause = true;

            InitPool();
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadVolumes();
    }

    private void LoadVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0.5f));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        SetEffectsVolume(PlayerPrefs.GetFloat("EffectsVolume", 0.5f));
    }

    public void SetMasterVolume(float value)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
    }

    public void SetMusicVolume(float value)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
    }

    public void SetEffectsVolume(float value)
    {
        _audioMixer.SetFloat("EffectsVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
    }

    private void InitPool()
    {
        _audioPool = new AudioSource[_poolSize];

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject go = new GameObject("AudioPool_" + i);
            go.transform.parent = transform;

            AudioSource source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = _effectsGroup;

            _audioPool[i] = source;
        }
    }

    private AudioSource GetPooledSource()
    {
        for (int i = 0; i < _audioPool.Length; i++)
            if (!_audioPool[i].isPlaying) return _audioPool[i];

        AudioSource s = _audioPool[_poolIndex];
        _poolIndex = (_poolIndex + 1) % _poolSize;
        return s;
    }

    public void PlaySound(SoundType soundType, Vector3? position = null)
    {
        SoundData data = GetData(soundType);
        if (data == null) return;

        AudioClip clip = GetRandomClip(data);

        if (data.is3D)
        {
            AudioSource source = GetPooledSource();
            Vector3 pos = position ?? Camera.main.transform.position;

            source.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);
            source.clip = clip;
            source.volume = data.volume;
            source.pitch = data.randomizePitch ? Random.Range(0.9f, 1.1f) : data.pitch;
            source.spatialBlend = 1f;
            source.minDistance = data.minDistance;
            source.maxDistance = data.maxDistance;
            source.rolloffMode = data.rolloffMode;
            source.Play();
        }
        else
        {
            _audioSource.pitch = data.randomizePitch ? Random.Range(0.9f, 1.1f) : data.pitch;
            _audioSource.PlayOneShot(clip, data.volume);
        }
    }

    public void PlayLoop(SoundType soundType, Vector3? position = null)
    {
        SoundData data = GetData(soundType);
        if (data == null) return;

        AudioClip clip = GetRandomClip(data);

        if (_loopFade != null) StopCoroutine(_loopFade);

        _loopSource.clip = clip;
        _loopSource.pitch = data.randomizePitch ? Random.Range(0.9f, 1.1f) : data.pitch;
        _loopSource.volume = data.volume;
        _loopSource.loop = true;

        if (data.is3D)
        {
            Vector3 pos = position ?? Camera.main.transform.position;
            _loopSource.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);

            _loopSource.spatialBlend = 1f;
            _loopSource.minDistance = data.minDistance;
            _loopSource.maxDistance = data.maxDistance;
            _loopSource.rolloffMode = data.rolloffMode;
        }
        else
        {
            _loopSource.spatialBlend = 0f;
        }

        _loopSource.Play();
    }

    public void PlayMusic(SoundType soundType, Vector3? position = null)
    {
        SoundData data = GetData(soundType);
        if (data == null) return;

        AudioClip clip = GetRandomClip(data);

        if (_loopFade != null) StopCoroutine(_loopFade);

        _musicSource.clip = clip;
        _musicSource.pitch = data.randomizePitch ? Random.Range(0.9f, 1.1f) : data.pitch;
        _musicSource.volume = data.volume;
        _musicSource.loop = true;

        if (data.is3D)
        {
            Vector3 pos = position ?? Camera.main.transform.position;
            _musicSource.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);

            _musicSource.spatialBlend = 1f;
            _musicSource.minDistance = data.minDistance;
            _musicSource.maxDistance = data.maxDistance;
            _musicSource.rolloffMode = data.rolloffMode;
        }
        else
        {
            _musicSource.spatialBlend = 0f;
        }

        _musicSource.Play();
    }

    public void StopLoop()
    {
        if (_loopSource.isPlaying)
        {
            if (_loopFade != null) StopCoroutine(_loopFade);
            _loopFade = StartCoroutine(FadeOut(_loopSource));
        }
    }

    private IEnumerator FadeOut(AudioSource source)
    {
        float startVolume = source.volume;
        while (source.volume > 0f)
        {
            source.volume -= startVolume * Time.unscaledDeltaTime * 3f;
            yield return null;
        }
        source.Stop();
        source.volume = startVolume;
        source.clip = null;
    }

    private SoundData GetData(SoundType soundType)
    {
        return System.Array.Find(_sounds, s => s.soundType == soundType);
    }

    private AudioClip GetRandomClip(SoundData data)
    {
        if (data.clips.Length == 1) return data.clips[0];

        AudioClip clip;
        do
        {
            clip = data.clips[Random.Range(0, data.clips.Length)];
        } while (clip == _lastClip);

        _lastClip = clip;
        return clip;
    }

    public float GetSoundDuration(SoundType soundType)
    {
        SoundData data = GetData(soundType);
        if (data == null || data.clips == null || data.clips.Length == 0)
            return 0f;

        float maxLength = 0f;

        foreach (var clip in data.clips)
        {
            if (clip != null && clip.length > maxLength)
                maxLength = clip.length;
        }

        return maxLength;
    }
}