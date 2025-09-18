using UnityEngine;

public class HitSound : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] GameObject player;
    [SerializeField] float destroyDistance = 15f;

    private void Awake()
    {
        if (!TryGetComponent(out audioSource))
            audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        if (player.transform.position.z - transform.position.z > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
