using UnityEngine;

public class TremorPlayer : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] float tremorDamage = 20f;
    [SerializeField] float stunDuration = 0.5f;
    [SerializeField] float stunSpeedMultiplier = 0.3f;
    
    [Header("Trigger Settings")]
    [SerializeField] float requiredDelay = 1.0f; // How long player must stand inside to get hit

    private float timeInside = 0f;
    private bool hasHitPlayer = false;

    void Start()
    {
    }

    void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

    // 1. Triggered exactly when the player steps in
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            timeInside = 0f; // Start the clock at 0
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            timeInside += Time.deltaTime;
            if (timeInside >= requiredDelay)
            {
                hasHitPlayer = true;
                ApplyEffects(collision.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            timeInside = 0f;
        }
    }

    private void ApplyEffects(GameObject playerObject)
    {
        // Get player components
        PlayerLevel playerLevel = playerObject.GetComponent<PlayerLevel>();
        SwarmPlayerController playerController = playerObject.GetComponent<SwarmPlayerController>();

        // Deal damage
        if (playerLevel != null)
        {
            float currentHp = playerLevel.getCurrentHp();
            playerLevel.setCurrentHp(currentHp - tremorDamage);
        }

        // Apply stun
        if (playerController != null)
        {
            playerController.ApplyStun(stunDuration, stunSpeedMultiplier);
        }
    }
}