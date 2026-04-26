using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    private PlayerArmory playerStat;
    [SerializeField] private float damagePerTick;
    [SerializeField] private float tickRate = 0.2f;
    [SerializeField] LayerMask enemyLayer;
    private BoxCollider boxCollider;

    private float timer;
    void Start()
    {
        playerStat=GameObject.Find("Player").GetComponent<PlayerArmory>();
        damagePerTick = playerStat.getFlameThrowerDamagePerTick();
        boxCollider =GetComponent<BoxCollider>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tickRate)
        {
            CheckForHit();
            timer = 0f;
        }

    }

    void CheckForHit()
    {
        Collider[] hits = Physics.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.extents, transform.rotation, enemyLayer);

        foreach (Collider hit in hits)
        {
            EnemyStats enemyStats = hit.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.recieveDamage(damagePerTick);
            }
        }
    }

    public void setDamagePerTick(float damagePerTick)
    {
        this.damagePerTick = damagePerTick;
    }
}
