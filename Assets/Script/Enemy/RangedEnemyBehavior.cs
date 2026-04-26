using UnityEngine;

public class RangedEnemyBehavior : MonoBehaviour
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
    bool canMove = true;
    bool canShoot = false;

    GameObject player;
    PlayerLevel playerLevel;

    Animator anim;

    void Start()
    {
        player = GameObject.Find("Player");
        anim=GetComponent<Animator>();
        playerLevel = player.GetComponent<PlayerLevel>();
    }

    void Update()
    {
        if (player == null) return;
        if (!canMove) return;

        Vector3 vectorToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        if (canShoot)
        {

            if (distanceToPlayer <= stoppingDistance)
            {
                anim.SetBool("shooting", true);
                return;
            }
            else
            {
                anim.SetBool("shooting", false);
                canShoot = false;
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