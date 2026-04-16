using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{   
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Separation Settings")]
    [SerializeField] float separationRadius = 1.5f;
    [SerializeField] float separationWeight = 1.5f;
    [SerializeField] LayerMask enemyLayer;

    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player == null) return;
        Vector3 targetDirection = (player.transform.position - transform.position).normalized;
        Vector3 separationDirection = GetSeparationVector();
        Vector3 finalDirection = (targetDirection + separationDirection * separationWeight).normalized;
        transform.Translate(finalDirection * moveSpeed * Time.deltaTime, Space.World);
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