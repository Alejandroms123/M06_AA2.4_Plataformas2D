using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _step1;
    [SerializeField] private AudioSource _step2;



    public void PlayStep1()
    {
        _step1.Play();
    }

    public void PlayStep2()
    {
        _step2.Play();
    }
}