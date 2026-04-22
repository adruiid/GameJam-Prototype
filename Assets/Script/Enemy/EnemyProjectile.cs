using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float upperBound = 50f;
    [SerializeField] float lowerBound = -50;
    [SerializeField] float damage = 10f;
    [SerializeField] Vector3 hitBoxSize = new Vector3(0.5f, 0.5f, 2f);
    
    GameObject player;
    PlayerLevel playerLevel;
    
    float upperBoundX;
    float lowerBoundX;
    float upperBoundZ;
    float lowerBoundZ;
    Vector3 worldDirection;
    float projectileSpeed;
    [SerializeField] LayerMask playerLayer;

    void Start()
    {
        player = GameObject.Find("Player");
        playerLevel = player.GetComponent<PlayerLevel>();
        playerLayer = LayerMask.GetMask("Player");
        FindRelativeBounds();
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
            return;
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
            float currentHp = playerLevel.getCurrentHp();
            playerLevel.setCurrentHp(currentHp - damage);
            Destroy(gameObject);     
            return true; 
        }
        return false;
    }

    public void SetDirection(Vector3 direction, float speed)
    {
        worldDirection = direction;
        projectileSpeed = speed;
        if (worldDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(worldDirection);
        }
    }
}