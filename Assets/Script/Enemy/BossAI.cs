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

    private int pokeCount = 0;
    private float nextAttackTime;
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private bool attackTriggered = false;

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
        nextAttackTime = 0f;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        Vector3 desiredVelocity = Vector3.zero;

        if (distanceToPlayer <= stoppingDistance)
        {
            isAttacking = true;
            if (Time.time >= nextAttackTime)
            {
                attackTriggered = true;
                ExecuteAttack();
            }
        }
        else
        {
            isAttacking = false;
            attackTriggered = false;
            animator.SetBool("isPoking", false);
            animator.SetBool("isSlaming", false);
            
            Vector3 targetDirection = vectorToPlayer.normalized;
            targetDirection.y = 0;

            Vector3 obstacleAvoidance = GetObstacleAvoidanceVector();
            Vector3 finalDirection = (targetDirection + obstacleAvoidance * avoidanceWeight).normalized;

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

    void ExecuteAttack()
    {
        if (pokeCount >= 2)
        {
            PerformSlam();
            pokeCount = 0;
            nextAttackTime = Time.time + slamCooldown;
        }
        else
        {
            PerformPoke();
            pokeCount++;
            nextAttackTime = Time.time + pokeCooldown;
        }
    }

    void PerformPoke()
    {
        if (attackTriggered)
        {
            animator.SetBool("isSlaming", false);
            animator.SetBool("isPoking", true);
            DealPokeDAamage();
            attackTriggered = false;
        }
    }

    void PerformSlam()
    {
        if (attackTriggered)
        {
            animator.SetBool("isPoking", false);
            animator.SetBool("isSlaming", true);
            
            // Check if player is within slam range
            if (player != null && tremorPrefab != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= slamRange)
                {
                    Instantiate(tremorPrefab, player.transform.position, Quaternion.identity);
                }
            }
            attackTriggered = false;
        }
    }

    void DealPokeDAamage()
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
