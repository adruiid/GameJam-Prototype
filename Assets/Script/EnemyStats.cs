using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float healthPoint;
    float currentHealth;
    [SerializeField] float exp;
    private ExperienceManager experienceManager;
    private GameGeneralManager gameGeneralManager;
    void Start()
    {
        experienceManager = GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
        gameGeneralManager = GameObject.Find("Game Manager").GetComponent<GameGeneralManager>();
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
        gameGeneralManager.killSignal();
        experienceManager.recieveSignal(exp);
        Destroy(gameObject);
    }
}
