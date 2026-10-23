
using UnityEngine;

public class CoinSound : MonoBehaviour
{
    [SerializeField] private AudioClip coinClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (coinClip != null && audioSource != null)
            audioSource.PlayOneShot(coinClip);
    }
}
