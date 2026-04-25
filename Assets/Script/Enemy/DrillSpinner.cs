using UnityEngine;

public class DrillSpinner : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 360f * Time.deltaTime);
    }
}
