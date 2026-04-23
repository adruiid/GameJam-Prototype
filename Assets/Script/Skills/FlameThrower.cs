using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField] private float tickRate = 0.2f;
    [SerializeField] LayerMask enemyLayer;
    private BoxCollider boxCollider;

    private float timer;
    void Start()
    {
        boxCollider=GetComponent<BoxCollider>();
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
}
