using UnityEngine;

public class HomingProjectiles : MonoBehaviour
{
    Transform target;
    Vector3 moveDirection; 
    [Header("Movement Settings")]
    [SerializeField] float speed = 10f;
    [SerializeField] float maxTargetingRange = 15f;
    [SerializeField] float upperBound = 50f;
    [SerializeField] float lowerBound = -50f;
    float upperBoundX;
    float lowerBoundX;
    float upperBoundZ;
    float lowerBoundZ;
    [Header("Hit Detection")]
    [SerializeField] Vector3 hitBoxSize = new Vector3(0.5f, 0.5f, 2f); 
    [SerializeField] LayerMask enemyLayer;

    PlayerArmory playerArmory;
    void Start()
    {
        playerArmory = GameObject.Find("Player").GetComponent<PlayerArmory>();

        target = FindClosestEnemy();
        FindRelativeBounds();
        if (target == null)
        {
            float randomX = Random.Range(-1f, 1f);
            float randomZ = Random.Range(-1f, 1f);
            moveDirection = new Vector3(randomX, 0f, randomZ).normalized;

            if (moveDirection == Vector3.zero) // Justt a failsafe
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
        if (transform.position.x > upperBoundX || transform.position.x < lowerBoundX || transform.position.z > upperBoundZ || transform.position.z < lowerBoundZ)
        {
            Destroy(gameObject);
            return;
        }
        if (target != null)
        {
            moveDirection = (target.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.LookAt(target);
        }
        else
        {
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        
        float closestDistanceSqr = maxTargetingRange * maxTargetingRange;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = (enemy.transform.position - currentPosition).sqrMagnitude;
            if (distanceToEnemy < closestDistanceSqr)
            {
                closestDistanceSqr = distanceToEnemy;
                closest = enemy.transform;
            }
        }

        return closest;
    }
    bool CheckForHit()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, hitBoxSize / 2f, transform.rotation, enemyLayer);

        foreach (Collider hit in hitColliders)
        {
            EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
            enemyStats.recieveDamage(playerArmory.getDamage());
            Destroy(gameObject);     
            return true; 
        }
        return false;
    }
}