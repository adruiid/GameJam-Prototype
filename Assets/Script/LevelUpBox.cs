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
    [SerializeField] Text homingIndicator;
    [SerializeField] Text flameThrowerIndicator;

    [SerializeField] Text button1Text;
    [SerializeField] Text button2Text;
    [SerializeField] Image button1Image;
    [SerializeField] Image button2Image;

    int idx1, idx2;
    string upgrade1;
    string upgrade2;

    [SerializeField]UpgradeContainers[] upgradeList;
    [SerializeField] GameObject homingMissilePrefab;
    HomingProjectiles homingSpeed;

    bool hasHoming;
    bool hasFlameThrower;
    void Start()
    {
        playerArmory = GameObject.Find("Player").GetComponent<PlayerArmory>();
        playerLevel = GameObject.Find("Player").GetComponent<PlayerLevel>();
        experienceManager = GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
        homingSpeed= homingMissilePrefab.GetComponent<HomingProjectiles>();
    }

    
    void Update()
    {

        if (hasHoming) homingIndicator.gameObject.SetActive(true);
        if (hasFlameThrower) flameThrowerIndicator.gameObject.SetActive(true);
    }

    public void assignNew()
    {
        idx1 = Random.Range(0, upgradeList.Length);
        while (true)
        {
            idx2 = Random.Range(0, upgradeList.Length);
            if (idx2 != idx1) break;
        }
        button1Text.text = upgradeList[idx1].upgradeName;
        button2Text.text = upgradeList[idx2].upgradeName;
        button1Image.sprite = upgradeList[idx1].icon;
        button2Image.sprite= upgradeList[idx2].icon;

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

    public void callFunction(int idxFunc)
    {
        if (idxFunc == 0) activateHomingMissile();
        else if (idxFunc == 1) increaseDamage();
        else if (idxFunc == 2) activateFlameThrower();
        else if (idxFunc == 3) increaseSpeed();
        else if (idxFunc == 4) increaseMaxHealth();

        experienceManager.recieveLeveledUpSignal();
    }

    public void activateHomingMissile()
    {
        if (playerArmory.hasHomingMissiles)
        {
            upgradeHomingMissile();
            return;
        }

        playerArmory.hasHomingMissiles = true;
        hasHoming = true;
    }

    public void upgradeHomingMissile()
    {
        homingSpeed.speed += 5f;
    }

    public void increaseDamage()
    {
        playerArmory.setDamage(playerArmory.getDamage() * 2);
    }

    public void activateFlameThrower()
    {
        hasFlameThrower = true;
        playerArmory.hasFlameThrower = true;
    }

    public void increaseSpeed()
    {
        playerLevel.setSpeed(playerLevel.getSpeed() + 100f);
    }

    public void increaseMaxHealth()
    {
        playerLevel.setMaxHP(playerLevel.getMaxHp() + 10f);
    }


}
