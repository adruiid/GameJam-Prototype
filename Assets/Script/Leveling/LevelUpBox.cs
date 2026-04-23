using Unity.Burst;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpBox : MonoBehaviour
{
    PlayerArmory playerArmory;
    PlayerLevel playerLevel;

    ExperienceManager experienceManager;

    [SerializeField] Text maxHealthIndicator;
    [SerializeField] Text levelIndicator;
    [SerializeField] Text damageIndicator;
    [SerializeField] Text speedIndicator;

    [SerializeField] Text button1Text;
    [SerializeField] Text button2Text;
    [SerializeField] Text button3Text;
    [SerializeField] Image button1Image;
    [SerializeField] Image button2Image;
    [SerializeField] Image button3Image;

    int idx1, idx2, idx3;
    string upgrade1;
    string upgrade2;
    string upgrade3;

    [SerializeField]UpgradeContainers[] upgradeList;
    [SerializeField] GameObject homingMissilePrefab;
    HomingProjectiles homingSpeed;

    private int homingLevel;
    [SerializeField] UpgradeContainers upgradeHoming;

    private int cogWheelLevel;
    [SerializeField] UpgradeContainers upgradeCogWheel;

    private int mineLevel;
    [SerializeField] UpgradeContainers upgradeMine;

    private int flameThrowerLevel;
    [SerializeField] UpgradeContainers upgradeFlameThrower;



    [SerializeField] UpgradeContainers nullUpgrade;

    void Start()
    {
        playerArmory = GameObject.Find("Player").GetComponent<PlayerArmory>();
        playerLevel = GameObject.Find("Player").GetComponent<PlayerLevel>();
        experienceManager = GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
        homingSpeed= homingMissilePrefab.GetComponent<HomingProjectiles>();
    }

    
    void Update()
    {
        if (playerArmory.hasHomingMissiles)
        {
            upgradeList[0] = upgradeHoming;
        }

        if (playerArmory.hasCogWheel)
        {
            upgradeList[5] = upgradeCogWheel;
        }

        if (playerArmory.hasMines)
        {
            upgradeList[6] = upgradeMine;
        }

        if(playerArmory.hasFlameThrower)
        {
            upgradeList[2] = upgradeFlameThrower;
        }
    }

    public void assignNew()
    {
        while (true)
        {
            idx1 = Random.Range(0, upgradeList.Length);
            if (!(upgradeList[idx1] == nullUpgrade)) break;
        }
        
        while (true)
        {
            idx2 = Random.Range(0, upgradeList.Length);
            if (upgradeList[idx2] == nullUpgrade) continue;
            if (idx2 != idx1) break;
        }
        while (true)
        {
            idx3 = Random.Range(0, upgradeList.Length);
            if (upgradeList[idx3] == nullUpgrade) continue;
            if (idx3 != idx1 && idx3 != idx2) break;
        }


        button1Text.text = upgradeList[idx1].upgradeName;
        button2Text.text = upgradeList[idx2].upgradeName;
        button3Text.text = upgradeList[idx3].upgradeName;
        button1Image.sprite = upgradeList[idx1].icon;
        button2Image.sprite= upgradeList[idx2].icon;
        button3Image.sprite = upgradeList[idx3].icon;

        maxHealthIndicator.text= "Max Health: " + playerLevel.getMaxHp();
        levelIndicator.text = "Level: "+ playerLevel.getLevel();
        damageIndicator.text = "Damage: " + playerArmory.getDamage();
        speedIndicator.text = "Speed: " + playerLevel.getSpeed()/100f;

    }

    public void callButtonOne()
    {
        callFunction(idx1);
    }

    public void callButtonTwo()
    {
        callFunction(idx2);
    }

    public void callButtonThree()
    {
        callFunction(idx3);
    }

    public void callFunction(int idxFunc)
    {
        if (idxFunc == 0) activateHomingMissile();
        else if (idxFunc == 1) increaseDamage();
        else if (idxFunc == 2) activateFlameThrower();
        else if (idxFunc == 3) increaseSpeed();
        else if (idxFunc == 4) increaseMaxHealth();
        else if (idxFunc == 5) activateCogWheel();
        else if (idxFunc == 6) activateMine();

        experienceManager.recieveLeveledUpSignal();
    }

    public void activateHomingMissile()
    {
        if (playerArmory.hasHomingMissiles)
        {
            homingLevel++;
            upgradeHomingMissile();
            return;
        }

        playerArmory.hasHomingMissiles = true;
        homingLevel = 1;
    }

    private void upgradeHomingMissile()
    {
        if (homingLevel == 2) playerArmory.setHomingCooldown(0.7f);
        else if (homingLevel == 3) playerArmory.setHomingCooldown(0.5f);
        else if (homingLevel == 4)
        {
            playerArmory.setHomingCooldown(0.3f);
            upgradeList[0] = nullUpgrade;
        }
    }

    public void increaseDamage()
    {
        playerArmory.setDamage(playerArmory.getDamage() * 2);
    }

    public void activateFlameThrower()
    {
        if (playerArmory.hasFlameThrower)
        {
            flameThrowerLevel++;
            UpgradeFlameThrower();
            return;
        }

        flameThrowerLevel = 1;
        playerArmory.hasFlameThrower = true;
    }

    private void UpgradeFlameThrower()
    {
        if (flameThrowerLevel == 2)
        {

        } else if(flameThrowerLevel == 3)
        {
            upgradeList[2] = nullUpgrade;
        }
    }

    public void activateCogWheel()
    {
        if (playerArmory.hasCogWheel)
        {
            cogWheelLevel++;
            UpgradeCogWheel();
            return;
        }
        cogWheelLevel = 1;
        playerArmory.hasCogWheel = true;
    }

    private void UpgradeCogWheel()
    {
        if (cogWheelLevel == 3)
        {
            upgradeList[5] = nullUpgrade;
        }
        playerArmory.setCogWheelLevel(cogWheelLevel);
    }

    public void increaseSpeed()
    {
        playerLevel.setSpeed(playerLevel.getSpeed() + 100f);
    }

    public void increaseMaxHealth()
    {
        playerLevel.setMaxHP(playerLevel.getMaxHp() + 10f);
    }

    public void activateMine()
    {
        if(playerArmory.hasMines)
        {
            mineLevel++;
            UpgradeMine();
            return;
        }

        mineLevel = 1;
        playerArmory.hasMines = true;
    }

    private void UpgradeMine()
    {
        if (mineLevel == 2)
        {
            playerArmory.setMineLevel(2);
        }

        if (mineLevel == 3)
        {
            playerArmory.setMineLevel(3);
            upgradeList[6] = nullUpgrade;
        }
    }

}
