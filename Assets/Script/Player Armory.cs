using System.Collections;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerArmory : MonoBehaviour
{
    private float damage=10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileCooldown = 2;
    [SerializeField] GameObject homingMissiles;
    public bool hasHomingMissiles;
    public bool hasFlameThrower;
    [SerializeField] float homingMissileCooldown = 0.5f;
    private float nextSpawnTime;
    void Start()
    {
        StartCoroutine(SpawnProjectiles());
    }

    void Update()
    {
        if (hasHomingMissiles && Time.time >= nextSpawnTime)
        {
            FireHomingMissiles();
            nextSpawnTime = Time.time + homingMissileCooldown;
        }
    }

    IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            yield return new WaitForSeconds(projectileCooldown);
        }
    }
    private void FireHomingMissiles()
    {
        Instantiate(homingMissiles, transform.position, homingMissiles.transform.rotation);
    }

    public float getDamage()
    {
        return damage;
    }

    public void setDamage(float newDamage)
    {
        damage = newDamage;
    }
}
