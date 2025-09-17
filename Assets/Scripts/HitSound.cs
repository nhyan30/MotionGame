using UnityEngine;

public class HitSound : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        if (!TryGetComponent(out audioSource))
            audioSource = GetComponentInChildren<AudioSource>();
    }
    public void PlaySound()
    {
        audioSource.Play();
    }
}
