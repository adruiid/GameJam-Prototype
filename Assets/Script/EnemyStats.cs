using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float healthPoint;
    float currentHealth;
    [SerializeField] float exp;
    private ExperienceManager experienceManager;
    void Start()
    {
        experienceManager = GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
        currentHealth = healthPoint;
    }
    void Update()
    {
        if (currentHealth <= 0)
        {
            destroySelf();
        }
    }

    public void recieveDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void destroySelf()
    {
        experienceManager.recieveSignal(exp);
        Destroy(gameObject);
    }
}
