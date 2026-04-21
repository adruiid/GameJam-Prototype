using UnityEngine;

public class PlayerAnimatorAndRotate : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    private SwarmPlayerController playerController;
    [SerializeField]private GameObject playerMesh;
    void Start()
    {
        anim=GetComponent<Animator>();
        playerController= GetComponent<SwarmPlayerController>();
        rb= GetComponent<Rigidbody>();
    }

    void Update()
    {
        float vectorMovement = playerController.getVectorMovement();
        anim.SetBool("isMoving", vectorMovement > 0f);

        setDirection();

    }

    void setDirection()
    {
        /*
        if(rb.linearVelocity.x>0f)
        {
            playerMesh.transform.rotation = Quaternion.Euler(-90f, 90f, 0f);
        }
        else if(rb.linearVelocity.x<0f)
        {
            playerMesh.transform.rotation = Quaternion.Euler(-90f, -90f, 0f);
        }


        if(rb.linearVelocity.z> 0f)
        {
            playerMesh.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }
        else if (rb.linearVelocity.z < 0f)
        {
            playerMesh.transform.rotation = Quaternion.Euler(-90f, 180f, 0f);
        }

        if(rb.linearVelocity.z>0f && rb.linearVelocity.x>0f)
        {
            playerMesh.transform.rotation = Quaternion.Euler(-90f, 45f, 0f);
        }
        else if (rb.linearVelocity.z > 0f && rb.linearVelocity.x < 0f)
        {
            playerMesh.transform.rotation = Quaternion.Euler(-90f, -45f, 0f);
        }
        else if (rb.linearVelocity.z < 0f && rb.linearVelocity.x > 0f)
        {
            playerMesh.transform.rotation = Quaternion.Euler(-90f, 135f, 0f);
        }
        else if (rb.linearVelocity.z < 0f && rb.linearVelocity.x < 0f)
        {
            playerMesh.transform.rotation = Quaternion.Euler(-90f, -135f, 0f);
        }*/

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            angle=Mathf.Round(angle/45f) * 45f;

            playerMesh.transform.rotation = Quaternion.Euler(-90f, angle, 0f);
        }
    }
}
