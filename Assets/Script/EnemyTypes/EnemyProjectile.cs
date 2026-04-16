using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float upperBound = 50f;
    [SerializeField] float lowerBound = -50;
    [SerializeField] float damage = 10f;
    [SerializeField] Vector3 hitBoxSize = new Vector3(0.5f, 0.5f, 2f);
    GameObject player;
    float upperBoundX;
    float lowerBoundX;
    float upperBoundZ;
    float lowerBoundZ;
    Vector3 worldDirection;
    [SerializeField] LayerMask playerLayer;

    void Start()
    {
        player = GameObject.Find("Player");
        playerLayer = LayerMask.GetMask("Player");
        FindRelativeBounds();
        AimAtPlayer();
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
        transform.position += worldDirection * projectileSpeed * Time.deltaTime;
        if (transform.position.x > upperBoundX || transform.position.x < lowerBoundX || transform.position.z > upperBoundZ || transform.position.z < lowerBoundZ)
        {
            Destroy(gameObject);
        }
        bool hitSomething = CheckForHit();
        if (hitSomething)
        {
            return; 
        }
    }
    bool CheckForHit()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, hitBoxSize / 2f, transform.rotation, playerLayer);

        foreach (Collider hit in hitColliders)
        { 
                //player damage logic here
                Destroy(gameObject);     
                return true; 
            
        }
        return false;
    }
    private void AimAtPlayer()
    {
        if (player == null) return;
        worldDirection = player.transform.position - transform.position;
        worldDirection.y = 0;
        worldDirection.Normalize();
        if (worldDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(worldDirection);
        }
    }


}
