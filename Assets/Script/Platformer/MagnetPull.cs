using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class MagneticPull : MonoBehaviour
{
    [Header("Pull Settings")]
    [SerializeField] float flingSpeed = 25.0f;
    [SerializeField] float maxAimDistance = 50.0f;
    [SerializeField] LayerMask metallicLayer;
    
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
            if (!playerController.enabled)
            {
                if (((1 << collision.gameObject.layer) & metallicLayer) != 0)
                {
                    Vector3 surfaceNormal = collision.contacts[0].normal;
                    transform.rotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;

                    // Hand control back to the boots
                    playerController.enabled = true;
                    rb.linearVelocity = Vector3.zero; 
                }
                else 
                {
                    playerController.enabled = true;
                }
            }
        }
}