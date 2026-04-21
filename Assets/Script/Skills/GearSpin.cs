using UnityEngine;

public class GearSpin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f; // degrees per second
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
