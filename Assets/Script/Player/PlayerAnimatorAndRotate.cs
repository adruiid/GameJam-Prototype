using UnityEngine;

public class PlayerAnimatorAndRotate : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    private SwarmPlayerController playerController;
    public GameObject playerMesh;
    [SerializeField] float deadzoneRadius = 100f;
    private PauseMenu pauseMenu;
    private ExperienceManager experienceManager;
    
    private Quaternion lastValidRotation;
    
    void Start()
    {
        anim=GetComponent<Animator>();
        playerController= GetComponent<SwarmPlayerController>();
        rb= GetComponent<Rigidbody>();
        lastValidRotation = playerMesh.transform.rotation;
        pauseMenu = GameObject.Find("Game Manager").GetComponent<PauseMenu>();
        experienceManager = GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
    }

    void Update()
    {
        if(experienceManager.levelUpBoxStatus() || pauseMenu.pauseStatus())
        {
            return;
        }

        float vectorMovement = playerController.getVectorMovement();
        anim.SetBool("isMoving", vectorMovement > 0.1f);

        setDirection();

    }

    void setDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector2 offsetFromCenter = mousePos - screenCenter;
        
        // Check if mouse is outside deadzone
        if (offsetFromCenter.magnitude > deadzoneRadius)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            Vector3 direction = mouseWorld - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                lastValidRotation = Quaternion.Euler(-90f, angle, 0f);
            }
        }
        
        playerMesh.transform.rotation = lastValidRotation;
    }
}
