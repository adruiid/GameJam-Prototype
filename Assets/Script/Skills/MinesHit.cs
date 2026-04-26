using Unity.VisualScripting;
using UnityEngine;

public class MinesHit : MonoBehaviour
{
    [Header("Hit Detection")]
    [SerializeField] Vector3 hitBoxSize = new Vector3(0.5f, 0.5f, 2f); 
    [SerializeField] LayerMask enemyLayer;

    PlayerArmory playerArmory;

    [SerializeField] GameObject mineHitParticle;
    void Start()
    {
        playerArmory = GameObject.Find("Player").GetComponent<PlayerArmory>();
        Destroy(gameObject, 5f);
    }

    void Update()
    {
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
            SpawnParticleEffect();
            Destroy(gameObject);     
            return true; 
        }
        return false;
    }

    void SpawnParticleEffect()
    {
        if (mineHitParticle != null)
        {
            GameObject particle = Instantiate(mineHitParticle, transform.position + Vector3.up * 1.1f, Quaternion.identity);
            ParticleSystem[] allParticles = particle.GetComponentsInChildren<ParticleSystem>();

            foreach (var ps in allParticles)
            {
                ps.Play();
            }
        }
    }
}