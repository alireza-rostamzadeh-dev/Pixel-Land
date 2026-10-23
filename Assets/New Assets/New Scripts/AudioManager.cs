
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSound;

    public AudioClip background;
    public AudioClip walking;
    public AudioClip death;

    private void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (musicSource == null && sources.Length > 0)
            musicSource = sources[0];

        if (SFXSound == null && sources.Length > 1)
            SFXSound = sources[1];
    }

    private void Start()
    {
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSound != null && clip != null)
            SFXSound.PlayOneShot(clip);
    }

    public void PlayDeathSound()
    {
        PlaySFX(death);
    }

    public void PlayWalkingSound()
    {
        PlaySFX(walking);
    }
}
