using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField] GameObject hpOrb;
    [SerializeField] GameObject expOrb;
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


        if (Random.Range(0, 100) < 30)
        {
            Instantiate(hpOrb, hpOrbDropPosition, hpOrb.transform.rotation);
        }

        Instantiate(expOrb, expOrbDropPosition, hpOrb.transform.rotation);
    }
}
