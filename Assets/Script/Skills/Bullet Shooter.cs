using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float upperBound = 50f;
    [SerializeField] float lowerBound = -50;
    [SerializeField] Vector3 hitBoxSize = new Vector3(0.5f, 0.5f, 2f);
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject bulletParticle;
    
    float upperBoundX;
    float lowerBoundX;
    float upperBoundZ;
    float lowerBoundZ;
    Vector3 worldDirection = Vector3.forward;
    PlayerArmory playerArmory;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        playerArmory = player.GetComponent<PlayerArmory>();
        FindRelativeBounds();
        SetDirectionFromPlayer();
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
            StartCoroutine(particleEffect());
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
        Collider[] hitColliders = Physics.OverlapBox(transform.position, hitBoxSize / 2f, transform.rotation, enemyLayer);

        foreach (Collider hit in hitColliders)
        {
            EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
            enemyStats.recieveDamage(playerArmory.getDamage());
            StartCoroutine(particleEffect());
            Destroy(gameObject);     
            return true; 
        }
        return false;
    }
    
    private void SetDirectionFromPlayer()
    {
        Vector3 playerRight = player.GetComponent<PlayerAnimatorAndRotate>().playerMesh.transform.right;
        Vector3 directionOnPlane = new Vector3(-playerRight.z, 0f, playerRight.x).normalized;
        
        if (directionOnPlane != Vector3.zero)
        {
            worldDirection = directionOnPlane;
            transform.rotation = Quaternion.LookRotation(worldDirection);
        }
    }

    IEnumerator particleEffect()
    {
        GameObject particle= Instantiate(bulletParticle, transform.position, Quaternion.identity);
        ParticleSystem[] allParticles;
        allParticles = particle.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in allParticles)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(0.5f);
    }


}
