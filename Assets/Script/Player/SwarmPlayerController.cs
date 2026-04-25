using UnityEngine;
using System.Collections;

public class SwarmPlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    [SerializeField] float speed = 10f;
    //[SerializeField] GameObject projectilePrefab;
    Rigidbody rb;

    private float vectorMovement;
    private float originalSpeed;
    private bool isStunned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        vectorMovement = Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity= (Vector3.right * horizontalInput * Time.deltaTime * speed+ Vector3.forward * verticalInput * Time.deltaTime * speed);
    }
    // void OnClick()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
    //     {
    //         Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
    //     }
    // }

    public float getSpeed()
    {
        return speed;
    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public float getVectorMovement()
    {
        return vectorMovement;
    }

    public void ApplyStun(float stunDuration, float speedMultiplier)
    {
        if (!isStunned)
        {
            originalSpeed = speed;
            isStunned = true;
            speed *= speedMultiplier;
            StartCoroutine(RemoveStun(stunDuration));
        }
    }

    IEnumerator RemoveStun(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        isStunned = false;
    }
}
