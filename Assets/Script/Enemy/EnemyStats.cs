using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float healthPoint;
    float currentHealth;
    [SerializeField] float exp;
    private ExperienceManager experienceManager;
    private GameGeneralManager gameGeneralManager;
    private EnemyDrops enemyDrops;

    [SerializeField] GameObject explosionParticle;

    void Start()
    {
        experienceManager = GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
        gameGeneralManager = GameObject.Find("Game Manager").GetComponent<GameGeneralManager>();
        enemyDrops=GetComponent<EnemyDrops>();
        currentHealth = healthPoint;
    }
    void Update()
    {
        if (currentHealth <= 0)
        {
            destroySelf();
            return;
        }
    }

    public void recieveDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void destroySelf()
    {
        StartCoroutine(particleEffect());
        enemyDrops.killSignal();
        gameGeneralManager.killSignal();
        Destroy(gameObject);
    }

    IEnumerator particleEffect()
    {
        GameObject particle = Instantiate(explosionParticle, transform.position+Vector3.up*1.8f, Quaternion.identity);
        ParticleSystem[] allParticles;
        allParticles = particle.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in allParticles)
        {
            ps.Play();
        }
        yield return new WaitForSeconds(0.5f);
    }
}
