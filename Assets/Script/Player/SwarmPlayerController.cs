using UnityEngine;

public class SwarmPlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    [SerializeField] float speed = 10f;
    //[SerializeField] GameObject projectilePrefab;
    Rigidbody rb;

    private float vectorMovement;
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
}
