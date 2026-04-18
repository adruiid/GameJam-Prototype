using UnityEngine;

public class WheelRotator : MonoBehaviour
{   
    float rotation=0f;
    void FixedUpdate()
    {
        rotation += 90 * Time.deltaTime;
        transform.Rotate(rotation, 0, 0);
    }
}
