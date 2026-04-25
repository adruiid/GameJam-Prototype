using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stoppingDistance = 1.5f;

    [Header("Obstacle Avoidance")]
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float avoidanceRadius = 2f;
    [SerializeField] float avoidanceWeight = 2f;

    [Header("Poke Attack Settings")]
    [SerializeField] float pokeDamage = 15f;
    [SerializeField] float pokeRange = 1.5f;
    [SerializeField] float pokeCooldown = 1f;

    [Header("Slam Attack Settings")]
    [SerializeField] float slamRange = 3f;
    [SerializeField] float slamCooldown = 2f;
    [SerializeField] GameObject tremorPrefab;

    [Header("Smoothing")]
    [SerializeField] float velocitySmoothTime = 0.15f;

    private enum State { Idle, Walking, PokeAttack, SlamAttack, Cooldown }
    private State currentState = State.Idle;

    private int pokeCount = 0;
    private float stateTimer = 0f;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    private Animator animator;
    private bool isAttacking;

    GameObject player;
    PlayerLevel playerLevel;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
            playerLevel = player.GetComponent<PlayerLevel>();
        animator = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        // State machine for boss behavior
        switch (currentState)
        {
            case State.Walking:
                HandleWalkingState(vectorToPlayer, distanceToPlayer);
                break;
            case State.PokeAttack:
                HandlePokeAttackState(vectorToPlayer, distanceToPlayer);
                break;
            case State.SlamAttack:
                HandleSlamAttackState(vectorToPlayer, distanceToPlayer);
                break;
            case State.Cooldown:
                HandleCooldownState(vectorToPlayer, distanceToPlayer);
                break;
            case State.Idle:
                HandleIdleState(vectorToPlayer, distanceToPlayer);
                break;
        }

        ApplyMovement();
    }

    void HandleIdleState(Vector3 vectorToPlayer, float distanceToPlayer)
    {
        animator.SetBool("isPoking", false);
        animator.SetBool("isSlaming", false);
        
        if (distanceToPlayer < stoppingDistance)
        {
            currentState = State.PokeAttack;
            stateTimer = 0f;
        }
        else
        {
            currentState = State.Walking;
        }
    }

    void HandleWalkingState(Vector3 vectorToPlayer, float distanceToPlayer)
    {
        animator.SetBool("isPoking", false);
        animator.SetBool("isSlaming", false);

        if (distanceToPlayer <= stoppingDistance)
        {
            // Reached player, start attack
            currentState = State.PokeAttack;
            stateTimer = 0f;
            currentVelocity = Vector3.zero;
            return;
        }

        // Move toward player
        Vector3 targetDirection = vectorToPlayer.normalized;
        targetDirection.y = 0;

        Vector3 obstacleAvoidance = GetObstacleAvoidanceVector();
        Vector3 finalDirection = (targetDirection + obstacleAvoidance * avoidanceWeight).normalized;
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

    void HandlePokeAttackState(Vector3 vectorToPlayer, float distanceToPlayer)
    {
        animator.SetBool("isSlaming", false);
        animator.SetBool("isPoking", true);
        currentVelocity = Vector3.zero;
        isAttacking = true;

        // Deal damage once when attack triggers
        if (stateTimer == 0f)
        {
            DealPokeDamage();
        }

        stateTimer += Time.deltaTime;

        // Wait for attack animation to finish (adjust based on your animation length)
        if (stateTimer >= pokeCooldown)
        {
            pokeCount++;
            
            // Check if we should do a slam next
            if (pokeCount >= 2)
            {
                currentState = State.SlamAttack;
                stateTimer = 0f;
            }
            else
            {
                currentState = State.Cooldown;
                stateTimer = 0f;
            }
        }
    }

    void HandleSlamAttackState(Vector3 vectorToPlayer, float distanceToPlayer)
    {
        animator.SetBool("isPoking", false);
        animator.SetBool("isSlaming", true);
        currentVelocity = Vector3.zero;
        isAttacking = true;

        // Spawn tremor once when slam triggers
        if (stateTimer == 0f)
        {
            if (player != null && tremorPrefab != null)
            {
                if (distanceToPlayer <= slamRange)
                {
                    Instantiate(tremorPrefab, player.transform.position, Quaternion.identity);
                }
            }
        }

        stateTimer += Time.deltaTime;

        // Wait for slam animation to finish
        if (stateTimer >= slamCooldown)
        {
            pokeCount = 0;
            currentState = State.Cooldown;
            stateTimer = 0f;
        }
    }

    void HandleCooldownState(Vector3 vectorToPlayer, float distanceToPlayer)
    {
        animator.SetBool("isPoking", false);
        animator.SetBool("isSlaming", false);
        isAttacking = false;
        currentVelocity = Vector3.zero;

        stateTimer += Time.deltaTime;

        // Small cooldown before next action
        if (stateTimer >= 0.3f)
        {
            if (distanceToPlayer <= stoppingDistance)
            {
                currentState = State.PokeAttack;
            }
            else
            {
                currentState = State.Walking;
            }
            stateTimer = 0f;
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

    void DealPokeDamage()
    {
        if (player != null && playerLevel != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, pokeRange))
            {
                if (hit.collider.gameObject == player)
                {
                    float currentHp = playerLevel.getCurrentHp();
                    playerLevel.setCurrentHp(currentHp - pokeDamage);
                }
            }
        }
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

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
