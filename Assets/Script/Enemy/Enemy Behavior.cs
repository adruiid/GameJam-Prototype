using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{   
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Separation Settings")]
    [SerializeField] float separationRadius = 1.5f;
    [SerializeField] float separationWeight = 1.5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float stoppingDistance = 1.0f;

    [Header("Obstacle Avoidance")]
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float avoidanceRadius = 2f;
    [SerializeField] float avoidanceWeight = 2f;

    [Header("Combat Settings")]
    [SerializeField] float damageAmount = 10f;
    [SerializeField] float attackCooldown = 1f;
    private float nextAttackTime;

    bool canMove = true;
    bool canShoot = false;

    GameObject player;
    PlayerLevel playerLevel;

    void Start()
    {
        player = GameObject.Find("Player");
        playerLevel = player.GetComponent<PlayerLevel>();
    }

    void Update()
    {
        if (player == null) return;
        if (!canMove) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        // If shooting, only move if player is out of range
        if (canShoot)
        {
            if (distanceToPlayer <= stoppingDistance) // Match RangedEnemy's attackRange
            {
                return; // Stop moving, just shoot
            }
            else
            {
                canShoot = false; // Player moved out of range, resume normal behavior
            }
        }

        Vector3 targetDirection = vectorToPlayer.normalized;
        targetDirection.y = 0;

        Vector3 separationDirection = GetSeparationVector();
        Vector3 obstacleAvoidance = GetObstacleAvoidanceVector();
        Vector3 finalDirection = (targetDirection + separationDirection * separationWeight + obstacleAvoidance * avoidanceWeight).normalized;

        // Move towards player
        transform.Translate(finalDirection * moveSpeed * Time.deltaTime, Space.World);
        Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void DealDamage()
    {
        if (playerLevel != null)
        {
            float currentHp = playerLevel.getCurrentHp();
            playerLevel.setCurrentHp(currentHp - damageAmount);
        }
    }

    Vector3 GetSeparationVector()
    {
        Vector3 separationForce = Vector3.zero;
        int neighborsCount = 0;
        Collider[] neighbors = Physics.OverlapSphere(transform.position, separationRadius, enemyLayer);

        foreach (Collider neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;
            Vector3 pushAway = transform.position - neighbor.transform.position;
            float distance = pushAway.magnitude;
            if (distance > 0)
            {
                separationForce += (pushAway.normalized / distance);
                neighborsCount++;
            }
        }
        if (neighborsCount > 0)
        {
            separationForce /= neighborsCount;
        }
        separationForce.y = 0;
        
        return separationForce;
    }

    Vector3 GetObstacleAvoidanceVector()
    {
        Vector3 avoidanceForce = Vector3.zero;
        Collider[] obstacles = Physics.OverlapSphere(transform.position, avoidanceRadius, obstacleLayer);

        foreach (Collider obstacle in obstacles)
        {
            Vector3 pushAway = transform.position - obstacle.transform.position;
            float distance = pushAway.magnitude;
            if (distance > 0)
            {
                avoidanceForce += (pushAway.normalized / distance);
            }
        }
        avoidanceForce.y = 0;
        
        return avoidanceForce.normalized;
    }

    public void SetCanShoot(bool shooting)
    {
        canShoot = shooting;
    }
}