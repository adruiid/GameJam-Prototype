using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform playerTransform;
    [SerializeField] int spawnIdxLimit = 0;
    [SerializeField] float spawnRate = 2f;

    [Header("Level Bounds & Collision")]
    [SerializeField] Collider planeCollider;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float enemySpawnRadius = 1f; 
    [SerializeField] int maxSpawnAttempts = 15;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + (1f / spawnRate);
        }
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnIdxLimit+1);
        GameObject enemyToSpawn = enemyPrefabs[randomIndex];

        Vector3 validSpawnPosition = Vector3.zero;
        bool foundValidPosition = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Bounds bounds = planeCollider.bounds;
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            Vector3 potentialPosition = new Vector3(randomX, 1f, randomZ);

            if (!Physics.CheckSphere(potentialPosition, enemySpawnRadius, obstacleLayer))
            {
                validSpawnPosition = potentialPosition;
                foundValidPosition = true;
                break;
            }
        }
        if (foundValidPosition)
        {
            Instantiate(enemyToSpawn, validSpawnPosition, Quaternion.identity);
        }

    }

    public void changeSpawnRate(float newSpawnRate)
    {
        spawnRate = newSpawnRate;
    }

    public void changeSpawnIdxLimit(int newSpawnIdxLimit)
    {
        spawnIdxLimit = newSpawnIdxLimit;
    }
}