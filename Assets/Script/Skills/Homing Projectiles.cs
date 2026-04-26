using UnityEngine;

public class HomingProjectiles : MonoBehaviour
{
    Transform target;
    Vector3 moveDirection; 
    
    [Header("Movement Settings")]
    public float speed = 10f;
    public float turnSpeed = 5f;
    [SerializeField] float maxTargetingRange = 15f;
    [SerializeField] float upperBound = 50f;
    [SerializeField] float lowerBound = -50f;
    
    [Header("Homing Settings")]
    [SerializeField] float homingDelay = 0.5f; 
    private float timeAlive = 0f;              
    
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
        

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            Vector3 playerRight = player.GetComponent<PlayerAnimatorAndRotate>().playerMesh.transform.right;
            moveDirection = new Vector3(-playerRight.z, 0f, playerRight.x).normalized;
        }
        else
        {
            moveDirection = transform.right;
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
        timeAlive += Time.deltaTime;

        if (transform.position.x > upperBoundX || transform.position.x < lowerBoundX || transform.position.z > upperBoundZ || transform.position.z < lowerBoundZ)
        {
            SpawnParticleEffect();
            Destroy(gameObject);
            return;
        }
        
        if (target != null && timeAlive >= homingDelay)
        {
            Vector3 flatTargetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 desiredDirection = (flatTargetPosition - transform.position).normalized;
            moveDirection = Vector3.RotateTowards(moveDirection, desiredDirection, turnSpeed * Time.deltaTime, 0f).normalized;
        }

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection) * Quaternion.Euler(0, -90, 0);
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
            SpawnParticleEffect();
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
            SpawnParticleEffect();
            Destroy(gameObject);     
            return true; 
        }
        return false;
    }

    void SpawnParticleEffect()
    {
        if (missileParticle != null)
        {
            GameObject particle = Instantiate(missileParticle, transform.position + Vector3.up * 1.1f, Quaternion.identity);
            ParticleSystem[] allParticles = particle.GetComponentsInChildren<ParticleSystem>();
            
            foreach (var ps in allParticles)
            {
                ps.Play();
            }
        }
    }
}