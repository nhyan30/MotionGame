using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform player;
    [SerializeField] private float coinDistance = 15;

    private float startDelay = 2;
    private float repeatRate = 2;

    private void Start()
    {
        InvokeRepeating("SpawnCoin", startDelay, repeatRate);
    }

    private void SpawnCoin()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-3, 3), 1, player.transform.position.z + coinDistance);
        Instantiate(coinPrefab, spawnPos, coinPrefab.transform.rotation);
    }
}
