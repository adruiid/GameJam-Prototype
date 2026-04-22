using System.Collections;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerArmory : MonoBehaviour
{
    private float damage=10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileCooldown = 2;
    [SerializeField] Transform gunTransform;
    [SerializeField] GameObject homingMissiles;
    public bool hasHomingMissiles;

    [SerializeField] float homingMissileCooldown = 0.5f;
    public bool hasFlameThrower;
    [SerializeField] bool hasMines = true;
    [SerializeField] GameObject minePrefab;
    [SerializeField] float mineCooldown = 1.5f;
    private float nextMissileSpawnTime;
    private float nextMineSpawnTime;
    void Start()
    {
        StartCoroutine(SpawnProjectiles());
    }

    void Update()
    {
        if (hasHomingMissiles && Time.time >= nextMissileSpawnTime)
        {
            FireHomingMissiles();
            nextMissileSpawnTime = Time.time + homingMissileCooldown;
        }

        if (hasMines && Time.time >= nextMineSpawnTime)
        {
            PlaceMine();
            nextMineSpawnTime = Time.time + mineCooldown;
        }
    }

    IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            Vector3 spawnPos= gunTransform.position+gunTransform.forward*2f;
            Instantiate(projectilePrefab, spawnPos, projectilePrefab.transform.rotation);
            yield return new WaitForSeconds(projectileCooldown);
        }
    }
    private void FireHomingMissiles()
    {
        Vector3 spawnPosition = gunTransform.position;
        Instantiate(homingMissiles, spawnPosition, homingMissiles.transform.rotation);
    }

    public float getDamage()
    {
        return damage;
    }

    public void setDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void PlaceMine()
    {
        Vector3 spawnPosition = transform.position + Vector3.down * 0f;
        Instantiate(minePrefab, spawnPosition, minePrefab.transform.rotation);
    }
}
