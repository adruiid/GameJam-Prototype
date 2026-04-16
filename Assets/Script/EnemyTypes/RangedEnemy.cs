using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{   
    [SerializeField] float attackRange = 5f;
    GameObject player;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float attackCooldown = 2f;
    float nextShotTime;
    EnemyBehavior enemyBehavior;
    void Start()
    {
        player= GameObject.Find("Player");
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        BulletShooter(distanceToPlayer);
    }

    void BulletShooter(float distanceToPlayer)
    {
        if (distanceToPlayer < attackRange)
        {
            if (Time.time >= nextShotTime)
            {
                nextShotTime = Time.time + attackCooldown;
                Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            }
            enemyBehavior.StopMovementWhenShooting();
        }
        else
        {
            enemyBehavior.StartMoving();
        }
    }
}
