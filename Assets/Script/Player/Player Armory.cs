using System.Collections;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerArmory : MonoBehaviour
{
    private CogwheelSpinner cogwheelScript;

    private float damage=10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileCooldown = 2;
    [SerializeField] Transform gunTransform;
    [SerializeField] GameObject homingMissiles;
    [SerializeField] float homingMissileCooldown = 1f;
    [SerializeField] GameObject minePrefab;
    [SerializeField] float mineCooldown = 4f;
    [SerializeField] Vector3 trackOffset = new Vector3(0f, 10f, 0f);
    private float nextMissileSpawnTime;
    private float nextMineSpawnTime;

    public bool hasFlameThrower;
    public bool hasHomingMissiles;
    public bool hasCogWheel; private bool cogWheelSignal;
    public bool hasMines;



    void Start()
    {
        cogwheelScript= GetComponent<CogwheelSpinner>();
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

        if (hasCogWheel && !cogWheelSignal)
        {
            cogWheelSignal = true;
            cogwheelScript.enabled = true;
            cogwheelScript.SpawnCogwheels();
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
        Instantiate(homingMissiles, spawnPosition + trackOffset, homingMissiles.transform.rotation);
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

    public void setHomingCooldown(float time)
    {
        homingMissileCooldown = time;
    }

    public void setCogWheelLevel(int level)
    {
        cogwheelScript.SetSkillLevel(level);
    }

    public void setMineLevel(int level)
    {
        if (level == 2)
        {
            mineCooldown = 2f;
        } else if (level == 3)
        {
            mineCooldown = 1f;
        }
    }
}
