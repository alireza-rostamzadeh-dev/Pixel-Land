using UnityEngine;

public class CoinSound : MonoBehaviour
{
    [SerializeField] private AudioClip coinClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (coinClip != null)
            audioSource.PlayOneShot(coinClip);
    }
}
