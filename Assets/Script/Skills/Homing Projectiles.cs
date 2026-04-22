using UnityEngine;
using System.Collections;

public class HomingProjectiles : MonoBehaviour
{
    Transform target;
    Vector3 moveDirection; 
    
    [Header("Movement Settings")]
    public float speed = 10f;
    [SerializeField] float maxTargetingRange = 15f;
    [SerializeField] float upperBound = 50f;
    [SerializeField] float lowerBound = -50f;
    [SerializeField] Vector3 trackOffset = new Vector3(0f, 10f, 0f);
    float upperBoundX;
    float lowerBoundX;
    float upperBoundZ;
    float lowerBoundZ;
    
    [Header("Hit Detection")]
    [SerializeField] Vector3 hitBoxSize = new Vector3(0.5f, 0.5f, 2f); 
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask obstacleLayer;

    PlayerArmory playerArmory;

    [SerializeField] GameObject missileParticle;
    
    void Start()
    {
        playerArmory = GameObject.Find("Player").GetComponent<PlayerArmory>();

        target = FindClosestEnemy();
        FindRelativeBounds();
        
        if (target == null)
        {
            // Failsafe
            float randomX = Random.Range(-1f, 1f);
            float randomZ = Random.Range(-1f, 1f);
            moveDirection = new Vector3(randomX, 0f, randomZ).normalized;

            if (moveDirection == Vector3.zero) 
            {
                moveDirection = Vector3.forward;
            }
        }
    }

    private void FindRelativeBounds()
    {
        upperBoundX = upperBound + transform.position.x;
        lowerBoundX = lowerBound + transform.position.x;
        upperBoundZ = upperBound + transform.position.z;
        lowerBoundZ = lowerBound + transform.position.z;
    }

    void Update()
    {
        // 1. Boundary Check
        if (transform.position.x > upperBoundX || transform.position.x < lowerBoundX || transform.position.z > upperBoundZ || transform.position.z < lowerBoundZ)
        {
            StartCoroutine(particleEffect());
            Destroy(gameObject);
            return;
        }
        
        // 2. Movement Logic
        if (target != null)
        {
            moveDirection = (target.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(moveDirection) * Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection) * Quaternion.Euler(0, -90, 0);
            }
        }
        
        bool hitSomething = CheckForHit();
        if (hitSomething)
        {
            return; 
        }
    }

    Transform FindClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, maxTargetingRange, enemyLayer);
        
        Transform closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider enemyCollider in enemiesInRange)
        {
            float distanceToEnemy = (enemyCollider.transform.position - currentPosition).sqrMagnitude;
            if (distanceToEnemy < closestDistanceSqr)
            {
                closestDistanceSqr = distanceToEnemy;
                closest = enemyCollider.transform;
            }
        }

        return closest;
    }
    
    bool CheckForHit()
    {
        Collider[] obstacleColliders = Physics.OverlapBox(transform.position, hitBoxSize / 2f, transform.rotation, obstacleLayer);
        if (obstacleColliders.Length > 0)
        {
            Destroy(gameObject);
            return true;
        }
        Collider[] hitColliders = Physics.OverlapBox(transform.position, hitBoxSize / 2f, transform.rotation, enemyLayer);

        foreach (Collider hit in hitColliders)
        {
            EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.recieveDamage(playerArmory.getDamage());
            }
            StartCoroutine(particleEffect());
            Destroy(gameObject);     
            return true; 
        }
        return false;
    }

    IEnumerator particleEffect()
    {
        GameObject particle = Instantiate(missileParticle, transform.position+Vector3.up*1.1f, Quaternion.identity);
        ParticleSystem[] allParticles;
        allParticles = particle.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in allParticles)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(0.5f);
    }
}