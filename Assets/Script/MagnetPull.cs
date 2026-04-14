using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class MagneticPull : MonoBehaviour
{
    [Header("Pull Settings")]
    [SerializeField] float flingSpeed = 25.0f;
    [SerializeField] float maxAimDistance = 50.0f;
    [SerializeField] LayerMask metallicLayer; // <-- Replaced Tag with LayerMask
    
    private LineRenderer lineRenderer;
    private Camera mainCam;
    private Rigidbody rb;
    private PlayerController playerController;

    private Vector3 validTargetPoint;
    private bool isTargetingValidSurface = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        mainCam = Camera.main;

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2; 
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleAiming();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ExecuteFling();
        }
    }

    void HandleAiming()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        // CHANGED: Pass metallicLayer into the Raycast. 
        // It now completely ignores objects that aren't on the metallic layer.
        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, metallicLayer))
        {
            isTargetingValidSurface = true;
            validTargetPoint = hit.point;

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, validTargetPoint);
            return;
        }

        isTargetingValidSurface = false;
        lineRenderer.enabled = false;
    }

    void ExecuteFling()
    {
        lineRenderer.enabled = false;

        if (isTargetingValidSurface)
        {
            Vector3 flightDirection = (validTargetPoint - transform.position).normalized;

            playerController.enabled = false;
            rb.linearVelocity = flightDirection * flingSpeed;

            isTargetingValidSurface = false;
        }
    }

    void OnCollisionEnter(Collision collision)
        {
            // Only trigger if we are currently mid-fling (controller is disabled)
            if (!playerController.enabled)
            {
                // Check if the object we crashed into is actually on the metallic layer
                if (((1 << collision.gameObject.layer) & metallicLayer) != 0)
                {
                    // Grab the angle (normal) of the wall we just smashed into
                    Vector3 surfaceNormal = collision.contacts[0].normal;

                    // Instantly snap the player's rotation so their "up" aligns with the wall's normal.
                    // This makes their "feet" (-transform.up) point perfectly into the wall.
                    transform.rotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;

                    // Hand control back to the boots
                    playerController.enabled = true;
                    rb.linearVelocity = Vector3.zero; 
                }
                else 
                {
                    // Optional: What happens if they fling into a non-metallic wall?
                    // You probably still want to turn the controller back on so they don't get stuck!
                    playerController.enabled = true;
                }
            }
        }
}