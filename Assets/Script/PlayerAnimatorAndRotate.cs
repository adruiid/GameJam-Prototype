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
        anim.SetBool("isMoving", vectorMovement > 0.1f);

        setDirection();

    }

    void setDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        Vector3 direction = mouseWorld - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            playerMesh.transform.rotation = Quaternion.Euler(-90f, angle, 0f);
        }
    }
}
