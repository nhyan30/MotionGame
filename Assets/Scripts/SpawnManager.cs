using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform player;
    [SerializeField] private float coinDistance = 3;

    private float startDelay = 2;
    private float repeatRate = 2;

    private void Start()
    {
        InvokeRepeating("SpawnCoin", startDelay, repeatRate);
    }

    private void SpawnCoin()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-1.225f, -0.192f), player.transform.position.y + 0.15f, player.transform.position.z + coinDistance);
        Instantiate(coinPrefab, spawnPos, coinPrefab.transform.rotation);
    }
}
