using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 5.0f;
    [SerializeField] float rotationSpeed = 15.0f;
    [SerializeField] float magneticPullStrength = 10.0f;

    [Header("Collision")]
    [SerializeField] float playerHalfHeight = 0.5f;
    [SerializeField] LayerMask metallicLayer;

    private float horizontalInput;
    private float lastMoveDir = 1f;
    private Vector3 targetNormal = Vector3.up;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        // NEW: Update our last known direction only when we are actually pressing a key
        if (horizontalInput != 0)
        {
            lastMoveDir = Mathf.Sign(horizontalInput);
        }
    }

    void FixedUpdate()
    {
        HandleMagneticAttachment();
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 movement = transform.right * horizontalInput * speed;
        Vector3 magneticPull = -targetNormal * magneticPullStrength;
        rb.linearVelocity = movement + magneticPull;
    }

    void HandleMagneticAttachment()
    {
        Vector3 rayOrigin = transform.position;
        float rayLength = playerHalfHeight + 0.2f;

        RaycastHit hit;
        bool isGrounded = false;

        // CHECK 1: Inner Corners 
        // Still requires input because we only want to climb walls we walk INTO
        if (horizontalInput != 0 && CheckMetallicRaycast(transform.right * lastMoveDir, playerHalfHeight + 0.1f, out hit))
        {
            isGrounded = true;
        }
    
        else if (CheckMetallicRaycast(-transform.up, rayLength, out hit))
        {
            isGrounded = true;
        }
        
        else 
        {
           
            Vector3 diagonalBackDown = (-transform.up - (transform.right * lastMoveDir)).normalized;
            
            if (CheckMetallicRaycast(diagonalBackDown, rayLength * 1.5f, out hit))
            {
                isGrounded = true;
            }
            else
            {
    
                Vector3 oppositeDiagonal = (-transform.up - (transform.right * -lastMoveDir)).normalized;
                if (CheckMetallicRaycast(oppositeDiagonal, rayLength * 1.5f, out hit))
                {
                    isGrounded = true;
                }
            }
        }

        if (isGrounded)
        {
            targetNormal = hit.normal;
        }

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, targetNormal) * transform.rotation;
        rb.MoveRotation(Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
    }

    bool CheckMetallicRaycast(Vector3 direction, float distance, out RaycastHit outHit)
    {
        if (Physics.Raycast(transform.position, direction, out outHit, distance, metallicLayer))
        {
            return true;
        }
        return false;
    }
}