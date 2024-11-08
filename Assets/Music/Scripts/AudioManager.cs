using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- AudioSource ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip Background;
    public AudioClip death;
    public AudioClip collectable;
    public AudioClip pickup;
    public AudioClip wallbang;

    private void Start()
    {
        musicSource.clip = Background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
