using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    private float playerMaxHealth;
    private float currentHealth;
    private float playerSpeed;


    private int currentLevel;
    private float experiencePoint;
    private float neededExp;

    private PlayerArmory armory;
    private ExperienceManager experienceManager;
    private SwarmPlayerController playerController;

    private AudioSource audioSource;
    [SerializeField] AudioClip expPickUpClip;

    [SerializeField] DamageFlash flash;

    private bool canTakeDamage = true;

    [SerializeField] Image speedIcon;
    [SerializeField] Image healthIcon;
    [SerializeField] Image damageIcon;


    void Start()
    {
        playerController = GetComponent<SwarmPlayerController>();
        audioSource = GetComponent<AudioSource>();
        armory = GetComponent<PlayerArmory>();
        experienceManager = GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
        currentLevel = 1;
        neededExp = 100;
        playerMaxHealth = 100;
        playerSpeed = playerController.getSpeed();
        currentHealth = playerMaxHealth;
    }


    void Update()
    {
        if (experiencePoint >= neededExp)
        {
            levelUp();
        }
    }

    private void levelUp()
    {
        experienceManager.recieveLevelUpSignal();
        experiencePoint = 0;
        currentLevel += 1;
        neededExp += 50;
    }

    public float getNeededExp()
    {
        return neededExp;
    }

    public float getExp()
    {
        return experiencePoint;
    }

    public void updateExp(float exp)
    {
        audioSource.PlayOneShot(expPickUpClip);
        experiencePoint += exp;
    }

    public int getLevel()
    {
        return currentLevel;
    }

    public float getSpeed()
    {
        return playerController.getSpeed();
    }

    public void setSpeed(float newSpeed)
    {
        playerController.setSpeed(newSpeed);
    }

    public float getMaxHp()
    {
        return playerMaxHealth;
    }

    public void setMaxHP(float newMaxHP)
    {
        playerMaxHealth = newMaxHP;
    }

    public float getCurrentHp()
    {
        return currentHealth;
    }

    public void setCurrentHp(float newHp)
    {
        if (!canTakeDamage) return;

        flash.Flash();
        currentHealth = newHp;
    }

    public void healSignal(float healingAmount)
    {
        if (currentHealth < playerMaxHealth)
        {
            currentHealth += healingAmount;
            if (currentHealth > playerMaxHealth)
            {
                currentHealth = playerMaxHealth;
            }
        }
    }

    public void temporarySpeedUpgrade()
    {
        StartCoroutine(SpeedBoost(playerSpeed * 2f));
    }

    IEnumerator SpeedBoost(float newSpeed)
    {
        float originalSpeed = playerSpeed;
        playerSpeed = newSpeed;
        float duration = 10f;
        float timer = 0f;

        speedIcon.fillAmount = 1f; // start full

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;          // 0 → 1
            speedIcon.fillAmount = 1f - t;  

            yield return null;
        }

        speedIcon.fillAmount = 0f;
        playerSpeed = originalSpeed;
    }
    public void temporaryDamageUpgrade()
    {
        StartCoroutine(damageBoost(armory.getDamage() * 2f));
    }

    IEnumerator damageBoost(float newDamage)
    {
        float originalDamage = armory.getDamage();
        armory.setDamage(newDamage);
        float duration = 10f;
        float timer = 0f;

        damageIcon.fillAmount = 1f; // start full

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;          // 0 → 1
            damageIcon.fillAmount = 1f - t;

            yield return null;
        }

        damageIcon.fillAmount = 0f;
        armory.setDamage(originalDamage);
    }

    public void temporaryImmunity()
    {
        StartCoroutine(Immunity());
    }

    IEnumerator Immunity()
    {
        canTakeDamage = false;
        float duration = 10f;
        float timer = 0f;

        healthIcon.fillAmount = 1f; // start full

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;          // 0 → 1
            healthIcon.fillAmount = 1f - t;

            yield return null;
        }

        healthIcon.fillAmount = 0f;
        canTakeDamage = true;
    }
}
