using System.Collections;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerArmory : MonoBehaviour
{
    private CogwheelSpinner cogwheelScript;

    private float damage = 10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileCooldown = 2;
    [SerializeField] Transform gunTransform;


    [SerializeField] GameObject homingMissiles;
    [SerializeField] float homingMissileCooldown = 1f;

    [SerializeField] GameObject minePrefab;
    [SerializeField] float mineCooldown = 4f;
    [SerializeField] Vector3 trackOffset = new Vector3(1.5f, 0f, 0.5f);

    [SerializeField] float flameThrowerCoolDown = 2f;
    [SerializeField] GameObject flameThrowerPrefab;
    [SerializeField] GameObject flameThrowerMediumPrefab;
    [SerializeField] GameObject flameThrowerLargePrefab;
    private GameObject flameThrowerToSpawn;
    [SerializeField] GameObject flameThrowerMuzzle;
    private float flameThrowerDamagePerTick = 2f;

    private float nextMissileSpawnTime;
    private float nextMineSpawnTime;
    private float nextFlameThrowerTime;

    public bool hasFlameThrower;
    public bool hasHomingMissiles;
    public bool hasCogWheel; private bool cogWheelSignal;
    public bool hasMines;



    void Start()
    {
        cogwheelScript = GetComponent<CogwheelSpinner>();
        StartCoroutine(SpawnProjectiles());
        flameThrowerToSpawn = flameThrowerPrefab;
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

        if (hasFlameThrower && Time.time >= nextFlameThrowerTime)
        {
            StartCoroutine(FireFlameThrower());
            nextFlameThrowerTime = Time.time + flameThrowerCoolDown;
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
            Vector3 spawnPos = gunTransform.position + gunTransform.forward * 2f;
            Instantiate(projectilePrefab, spawnPos, projectilePrefab.transform.rotation);
            yield return new WaitForSeconds(projectileCooldown);
        }
    }
    private void FireHomingMissiles()
    {
        Vector3 rotatedOffset = gunTransform.TransformDirection(trackOffset);
        Vector3 spawnPosition = gunTransform.position + rotatedOffset;

        Instantiate(homingMissiles, spawnPosition, homingMissiles.transform.rotation);
    }

    IEnumerator FireFlameThrower()
    {
        GameObject particle = Instantiate(flameThrowerToSpawn, flameThrowerMuzzle.transform.position, flameThrowerMuzzle.transform.rotation, flameThrowerMuzzle.transform);
        ParticleSystem[] allParticles;
        allParticles = particle.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in allParticles)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(0.5f);
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
            mineCooldown = 1f;
        }
        else if (level == 3)
        {
            mineCooldown = 0.5f;
        }
    }

    public void setFlameThrowerLevel(int level)
    {
        if (level == 2)
        {
            flameThrowerToSpawn = flameThrowerMediumPrefab;
            flameThrowerDamagePerTick = 4f;
            flameThrowerCoolDown = 1.5f;

        }
        else if (level == 3)
        {
            flameThrowerToSpawn = flameThrowerLargePrefab;
            flameThrowerDamagePerTick = 8f;
        }
    }

    public float getFlameThrowerDamagePerTick()
    {
        return flameThrowerDamagePerTick;
    }

}
