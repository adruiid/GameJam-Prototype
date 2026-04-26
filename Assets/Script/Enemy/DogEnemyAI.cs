using System;
using UnityEngine;

public class DogEnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float groundOffset = 0.5f;

    [Header("Separation Settings")]
    [SerializeField] float separationRadius = 2.5f;
    [SerializeField] float separationWeight = 3f;
    [SerializeField] LayerMask enemyLayer;

    [Header("Obstacle Avoidance")]
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float avoidanceRadius = 2f;
    [SerializeField] float avoidanceWeight = 2f;

    [Header("Pounce Attack Settings")]
    [SerializeField] float pounceRange = 5f;
    [SerializeField] float chargeTime = 0.5f;
    [SerializeField] float pounceDamage = 15f;
    [SerializeField] float pounceForce = 20f;
    [SerializeField] float pounceRadius = 2f;
    public bool attackTime = false;
    public bool runningTime = false;

    [Header("Smoothing")]
    [SerializeField] float velocitySmoothTime = 0.15f;

    private enum State { Idle, Walking, Charging, Pouncing, Waiting }
    private State currentState = State.Idle;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private float chargeTimer = 0f;
    private float waitTimer = 0f;
    private Vector3 pounceTarget;

    bool canMove = true;

    GameObject player;
    PlayerLevel playerLevel;

    private UniversalStatMultiplier statMultiplier;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
            playerLevel = player.GetComponent<PlayerLevel>();
        
        Vector3 spawnPos = transform.position;
        spawnPos.y += groundOffset;
        transform.position = spawnPos;
        
        lastPosition = transform.position;

        statMultiplier = GameObject.Find("Game Manager").GetComponent<UniversalStatMultiplier>();
    }

    void Update()
    {
        moveSpeed *= statMultiplier.getSpeedMultiplier();
        pounceDamage *= statMultiplier.getDamageMultiplier();

        if (player == null) return;
        if (!canMove) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        switch (currentState)
        {
            case State.Walking:
                HandleWalkingState(vectorToPlayer, distanceToPlayer);
                break;
            case State.Charging:
                HandleChargingState();
                break;
            case State.Pouncing:
                HandlePouncingState();
                break;
            case State.Waiting:
                HandleWaitingState(vectorToPlayer, distanceToPlayer);
                break;
            case State.Idle:
                HandleIdleState(vectorToPlayer, distanceToPlayer);
                break;
        }

        ApplyMovement();
    }

    void HandleIdleState(Vector3 vectorToPlayer, float distanceToPlayer)
    {
        if (distanceToPlayer < pounceRange)
        {
            currentState = State.Charging;
            chargeTimer = 0f;
            pounceTarget = player.transform.position;
            runningTime = false;
            attackTime = true;
            currentVelocity = Vector3.zero;
        }
        else
        {
            currentState = State.Walking;
        }
    }

    void HandleWalkingState(Vector3 vectorToPlayer, float distanceToPlayer)
    {
        if (distanceToPlayer <= pounceRange)
        {
            currentState = State.Charging;
            chargeTimer = 0f;
            pounceTarget = player.transform.position;
            currentVelocity = Vector3.zero;
            return;
        }
        attackTime = false;
        Vector3 targetDirection = vectorToPlayer.normalized;
        targetDirection.y = 0;
        runningTime = true;
        Vector3 separationDirection = GetSeparationVector();
        Vector3 obstacleAvoidance = GetObstacleAvoidanceVector();
        Vector3 finalDirection = (targetDirection + separationDirection * separationWeight + obstacleAvoidance * avoidanceWeight).normalized;

        Vector3 desiredVelocity = finalDirection * moveSpeed;

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
    }

    void HandleChargingState()
    {
        chargeTimer += Time.deltaTime;
        currentVelocity = Vector3.zero;
        attackTime = true;
        runningTime = false;

        if (chargeTimer >= chargeTime)
        {
            currentState = State.Pouncing;
            pounceTimer = 0f;
        }
    }

    private float pounceTimer = 0f;
    private bool hasHitThisFrame = false;

    void HandlePouncingState()
    {
        pounceTimer += Time.deltaTime;
        Vector3 directionToPounce = (pounceTarget - transform.position).normalized;
        directionToPounce.y = 0;
        currentVelocity = directionToPounce * pounceForce;

        if (!hasHitThisFrame && Vector3.Distance(transform.position, player.transform.position) <= pounceRadius + 0.5f)
        {
            DealDamage();
            hasHitThisFrame = true;
        }

        if (pounceTimer >= 0.3f || Vector3.Distance(transform.position, pounceTarget) < 1f)
        {
            currentState = State.Waiting;
            waitTimer = 0f;
            currentVelocity = Vector3.zero;
            hasHitThisFrame = false;
        }
    }

    void HandleWaitingState(Vector3 vectorToPlayer, float distanceToPlayer)
    {
        waitTimer += Time.deltaTime;
        currentVelocity = Vector3.zero;
        attackTime = false;
        runningTime = false;
        if (waitTimer >= 0.5f)
        {
            if (distanceToPlayer <= pounceRange)
            {
                currentState = State.Charging;
                chargeTimer = 0f;
                pounceTarget = player.transform.position;
            }
            else
            {
                currentState = State.Walking;
            }
        }
    }

    void ApplyMovement()
    {
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
            playerLevel.setCurrentHp(currentHp - pounceDamage);
        }
    }
    public bool IsAttacking()
    {
        return attackTime;
    }
    public bool IsRunning()
    {
        return runningTime;
    }
}
