using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField] GameObject hpOrb;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void killSignal()
    {
        if(Random.Range(0, 100) < 30)
        {
            Instantiate(hpOrb, transform.position, hpOrb.transform.rotation);
        }
    }
}
