using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip shoot, explosion;
    [HideInInspector] public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlaySoundWithRandomPitch(AudioSource audioSource, AudioClip clip, int min, int max)
    {
        int rnd = Random.Range(min, max);
        audioSource.pitch = (float)rnd / 100;
        audioSource.clip = clip;
        audioSource.Play();
    }

}
