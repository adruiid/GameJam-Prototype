using UnityEngine;

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
    void Start()
    {
        playerController = GetComponent<SwarmPlayerController>();
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

    public float getCurrnetHp()
    {
        return currentHealth;
    }

    public void setCurrentHp(float newCurrentHp)
    {
        currentHealth = newCurrentHp;
    }
}
