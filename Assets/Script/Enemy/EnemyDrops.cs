using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField] GameObject hpOrb;
    [SerializeField] GameObject expOrb;
    [SerializeField] private int hpOrbDropChance;
    [SerializeField] private int expOrbQuantity;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void killSignal()
    {
        
        Vector3 hpOrbDropPosition = transform.position;
        Vector3 expOrbDropPosition = transform.position;
        hpOrbDropPosition.y += 1f;
        hpOrbDropPosition.x += 2f;
        expOrbDropPosition.y += 2f;


        if (Random.Range(0, 100) < hpOrbDropChance)
        {
            Instantiate(hpOrb, hpOrbDropPosition, hpOrb.transform.rotation);
        }

        for(int i=0;i<expOrbQuantity;i++)
        {
            Instantiate(expOrb, expOrbDropPosition, expOrb.transform.rotation);
            expOrbDropPosition.x += 1f;
        }
    }
}
