using UnityEngine;

public class LootBoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject boxPrefabs;
    [SerializeField] Collider planeCollider;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float spawnRate = 2f;

    private float nextSpawnTime;

    [SerializeField] int maxSpawnAttempts = 15;

    private float currentLootBoxCount;
    private float currentLootBoxLimit = 5f;

    [SerializeField] float chestSpawnRadius = 40f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentLootBoxCount = GameObject.FindGameObjectsWithTag("LootBox").Length;

        if (currentLootBoxCount >= currentLootBoxLimit)
        {
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + (1f / spawnRate);
        }
    }

    void SpawnEnemy()
    {
        GameObject chestToSpawn = boxPrefabs;

        Vector3 validSpawnPosition = Vector3.zero;
        bool foundValidPosition = false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Bounds bounds = planeCollider.bounds;
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            Vector3 potentialPosition = new Vector3(randomX, 0f, randomZ);

            if (!Physics.CheckSphere(potentialPosition, chestSpawnRadius, obstacleLayer))
            {
                validSpawnPosition = potentialPosition;
                foundValidPosition = true;
                break;
            }
        }
        if (foundValidPosition)
        {
            Instantiate(chestToSpawn, validSpawnPosition, Quaternion.identity);
        }

    }
}
