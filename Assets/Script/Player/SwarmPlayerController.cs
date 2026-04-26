using UnityEngine;
using System.Collections;

public class SwarmPlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    [SerializeField] float speed = 10f; // NOTE: You may need to tweak this value in the Inspector now!
    Rigidbody rb;

    private float vectorMovement;
    private float originalSpeed;
    private bool isStunned = false;
    
    private Vector3 moveDirection; // Store the normalized direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // 1. Create a single vector out of the inputs
        Vector3 inputVector = new Vector3(horizontalInput, 0f, verticalInput);
        
        // 2. NORMALIZE IT! This stops the diagonal speed boost.
        moveDirection = inputVector.normalized;
        
        // Update your vectorMovement using the newly normalized vector (useful if you use this for animations)
        vectorMovement = moveDirection.magnitude; 
    }

    private void FixedUpdate()
    {
        // 3. Set velocity without Time.deltaTime. 
        // Velocity is already "units per second", so no time conversion is needed here.
        rb.linearVelocity = moveDirection * speed;
    }

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