using UnityEngine;

public class MeleeEnemyBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Separation Settings")]
    [SerializeField] float separationRadius = 2.5f;
    [SerializeField] float separationWeight = 3f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float stoppingDistance = 1.0f;

    [Header("Obstacle Avoidance")]
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float avoidanceRadius = 2f;
    [SerializeField] float avoidanceWeight = 2f;

    [Header("Melee Attack Settings")]
    [SerializeField] float meleeDamage = 10f;
    [SerializeField] float attackCooldown = 1.5f;

    [Header("Smoothing")]
    [SerializeField] float velocitySmoothTime = 0.15f;

    private float nextAttackTime;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    bool canMove = true;
    private bool isAttacking;

    GameObject player;
    PlayerLevel playerLevel;

    private UniversalStatMultiplier statMultiplier;

    void Start()
    {
        player = GameObject.Find("Player");
        playerLevel = player.GetComponent<PlayerLevel>();
        nextAttackTime = 0f;
        lastPosition = transform.position;
        statMultiplier=GameObject.Find("Game Manager").GetComponent<UniversalStatMultiplier>();
    }

    void Update()
    {
        meleeDamage *= statMultiplier.getDamageMultiplier();
        moveSpeed *= statMultiplier.getSpeedMultiplier();

        if (player == null) return;
        if (!canMove) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        Vector3 desiredVelocity = Vector3.zero;

        if (distanceToPlayer <= stoppingDistance)
        {
            isAttacking = true;
            if (Time.time >= nextAttackTime)
            {
                DealDamage();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            isAttacking = false;
            Vector3 targetDirection = vectorToPlayer.normalized;
            targetDirection.y = 0;

            Vector3 separationDirection = GetSeparationVector();
            Vector3 obstacleAvoidance = GetObstacleAvoidanceVector();
            Vector3 finalDirection = (targetDirection + separationDirection * separationWeight + obstacleAvoidance * avoidanceWeight).normalized;

            desiredVelocity = finalDirection * moveSpeed;
        }

        float movedDistance = Vector3.Distance(transform.position, lastPosition);
        bool isStuck = desiredVelocity.magnitude > 0.1f && movedDistance < 0.01f;

        if (isStuck)
        {
            stuckTimer += Time.deltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        float smoothTime = isStuck && stuckTimer > 0.1f ? velocitySmoothTime * 5f : velocitySmoothTime;
        currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, smoothTime);
        
        transform.Translate(currentVelocity * Time.deltaTime, Space.World);
        
        if (currentVelocity.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        lastPosition = transform.position;
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
                float strength = 1f / (distance * distance);
                separationForce += pushAway.normalized * strength;
                neighborsCount++;
            }
        }
        
        if (neighborsCount > 0)
        {
            separationForce /= neighborsCount;
            separationForce = separationForce.normalized;
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

    void DealDamage()
    {
        if (playerLevel != null)
        {
            float currentHp = playerLevel.getCurrentHp();
            playerLevel.setCurrentHp(currentHp - meleeDamage);
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
