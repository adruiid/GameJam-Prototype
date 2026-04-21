using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{   
    [SerializeField] float attackRange = 5f;
    [SerializeField] Vector3 projectileSpawnOffset = new Vector3(0, 0.5f, 0);
    GameObject player;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float projectileSpeed = 15f;
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

        BulletShooter(vectorToPlayer, distanceToPlayer);
    }

    void BulletShooter(Vector3 directionToPlayer, float distanceToPlayer)
    {
        if (distanceToPlayer < attackRange)
        {
            // Look at player while shooting
            Vector3 lookDir = directionToPlayer.normalized;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
            }

            if (Time.time >= nextShotTime)
            {
                nextShotTime = Time.time + attackCooldown;
                Vector3 spawnPos = transform.position + projectileSpawnOffset;
                GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
                
                // Pass the fire direction to the projectile
                EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
                if (projectileScript != null)
                {
                    projectileScript.SetDirection(directionToPlayer.normalized, projectileSpeed);
                }
            }
            // Allow separation/obstacle avoidance to prevent pile-up
            enemyBehavior.SetCanShoot(true);
        }
        else
        {
            enemyBehavior.SetCanShoot(false);
        }
    }
}
