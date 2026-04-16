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
    Vector3 targetDirection = Vector3.zero;

    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player == null) return;
        Vector3 vectorToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = vectorToPlayer.magnitude;

        if (distanceToPlayer > stoppingDistance)
        {
            targetDirection = vectorToPlayer.normalized;
        }

        Vector3 separationDirection = GetSeparationVector();
        Vector3 finalDirection = (targetDirection + separationDirection * separationWeight).normalized;

        if (finalDirection != Vector3.zero)
        {
            transform.Translate(finalDirection * moveSpeed * Time.deltaTime, Space.World);
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
}