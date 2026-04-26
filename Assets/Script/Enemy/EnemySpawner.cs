using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform playerTransform;
    [SerializeField] int spawnIdxLimit = 0;
    [SerializeField] float spawnRate = 2f;
    
    [Header("Distance Settings")]
    [SerializeField] float maxSpawnDistance = 40f; 
    [SerializeField] float minSpawnDistance = 10f; 

    [Header("Level Bounds & Collision")]
    [SerializeField] Collider planeCollider;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float enemySpawnRadius = 1f; 
    [SerializeField] int maxSpawnAttempts = 15;
    
    [SerializeField] float mapEdgeBuffer = 10f; 

    [Header("Boss Settings")]
    [SerializeField] GameObject boss;

    private float nextSpawnTime;
    private float currentEnemyCount;
    private float currentEnemyLimit;

    void Update()
    {
        if (currentEnemyCount >= currentEnemyLimit)
        {
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + (1f / spawnRate);
        }
    }

    public void SpawnBoss()
    {
        if (boss != null)
        {
            boss.SetActive(true);
        }

    }

    void SpawnEnemy()
    {
        if (playerTransform == null || planeCollider == null) return;

        currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        int randomIndex = Random.Range(0, spawnIdxLimit + 1);
        GameObject enemyToSpawn = enemyPrefabs[randomIndex];

        Vector3 validSpawnPosition = Vector3.zero;
        bool foundValidPosition = false;

        Bounds bounds = planeCollider.bounds;

        float safeMinX = bounds.min.x + mapEdgeBuffer;
        float safeMaxX = bounds.max.x - mapEdgeBuffer;
        float safeMinZ = bounds.min.z + mapEdgeBuffer;
        float safeMaxZ = bounds.max.z - mapEdgeBuffer;

        float playerMinX = playerTransform.position.x - maxSpawnDistance;
        float playerMaxX = playerTransform.position.x + maxSpawnDistance;
        float playerMinZ = playerTransform.position.z - maxSpawnDistance;
        float playerMaxZ = playerTransform.position.z + maxSpawnDistance;

        float validMinX = Mathf.Clamp(playerMinX, safeMinX, safeMaxX);
        float validMaxX = Mathf.Clamp(playerMaxX, safeMinX, safeMaxX);
        
        float validMinZ = Mathf.Clamp(playerMinZ, safeMinZ, safeMaxZ);
        float validMaxZ = Mathf.Clamp(playerMaxZ, safeMinZ, safeMaxZ);

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float randomX = Random.Range(validMinX, validMaxX);
            float randomZ = Random.Range(validMinZ, validMaxZ);

            Vector3 potentialPosition = new Vector3(randomX, 0f, randomZ);

            float distanceFromPlayer = Vector3.Distance(potentialPosition, playerTransform.position);
            if (distanceFromPlayer < minSpawnDistance)
            {
                continue; 
            }

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

    public void changeSpawnMaxLimit(int newMaxLimit)
    {
        currentEnemyLimit = newMaxLimit;
    }
}