using UnityEngine;

public class AudioDebugger : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DebugAfterFrame());
    }

    private System.Collections.IEnumerator DebugAfterFrame()
    {
        yield return null;

        var sm = SoundManager.Instance;

        if (sm == null)
        {
            Debug.LogError("SoundManager.Instance es NULL");
            yield break;
        }

        Debug.Log("SoundManager encontrado OK");
        Debug.Log($"Duracion clip MenuMusic: {sm.GetSoundDuration(SoundType.MenuMusic)}");

        sm.PlayMusic(SoundType.MenuMusic);

        yield return new WaitForSeconds(0.5f);

        var field = typeof(SoundManager).GetField("_musicSource",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field != null)
        {
            var musicSource = field.GetValue(sm) as AudioSource;
            if (musicSource == null)
                Debug.LogError("_musicSource es NULL — no está asignado en el Inspector");
            else
            {
                Debug.Log($"_musicSource.isPlaying: {musicSource.isPlaying}");
                Debug.Log($"_musicSource.clip: {musicSource.clip}");
                Debug.Log($"_musicSource.volume: {musicSource.volume}");
                Debug.Log($"_musicSource.mute: {musicSource.mute}");
                Debug.Log($"_musicSource.outputAudioMixerGroup: {musicSource.outputAudioMixerGroup}");
            }
        }
    }
}