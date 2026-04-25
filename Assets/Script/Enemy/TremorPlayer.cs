using UnityEngine;

public class TremorPlayer : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] float tremorDamage = 20f;
    [SerializeField] float stunDuration = 0.5f;
    [SerializeField] float stunSpeedMultiplier = 0.3f;

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

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            hasHitPlayer = true;

            // Get player components
            PlayerLevel playerLevel = collision.gameObject.GetComponent<PlayerLevel>();
            SwarmPlayerController playerController = collision.gameObject.GetComponent<SwarmPlayerController>();

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
}
