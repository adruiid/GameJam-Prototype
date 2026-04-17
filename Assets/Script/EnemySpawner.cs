using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform playerTransform;
    [SerializeField] int spawnIdxLimit=0;
    
    [SerializeField] float spawnRadius = 20f;
    [SerializeField] float spawnRate = 2f; 

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
        int randomIndex = Random.Range(0, spawnIdxLimit);
        GameObject enemyToSpawn = enemyPrefabs[randomIndex];

        float randomAngle = Random.Range(0f, Mathf.PI * 2f); 
        float spawnX = playerTransform.position.x + Mathf.Cos(randomAngle) * spawnRadius; //rcos@
        float spawnZ = playerTransform.position.z + Mathf.Sin(randomAngle) * spawnRadius;

        Vector3 spawnPosition = new Vector3(spawnX, 1f, spawnZ);
        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
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