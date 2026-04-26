using UnityEngine;

public class ParticleDies : MonoBehaviour
{

    void Start()
    {
        Invoke("ParticleSuicide", 1f);
    }
    


    void ParticleSuicide()
    {
        Destroy(gameObject);
    }
}
