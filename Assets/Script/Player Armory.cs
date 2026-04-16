using System.Collections;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerArmory : MonoBehaviour
{
    private float damage=10f;

    [SerializeField] GameObject homingMissiles;
    public bool hasHomingMissiles = true;
    [SerializeField] float homingMissileCooldown = 0.5f;
    private float nextSpawnTime;
    void Start()
    {
 
    }

    void Update()
    {
        if (hasHomingMissiles && Time.time >= nextSpawnTime)
        {
            FireHomingMissiles();
            nextSpawnTime = Time.time + homingMissileCooldown;
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
