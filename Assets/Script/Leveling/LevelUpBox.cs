using Unity.Burst;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    [SerializeField] Text button1Desc;
    [SerializeField] Text button2Desc;
    [SerializeField] Text button3Desc;


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
        if (upgradeList == null || upgradeList.Length == 0)
        {
            Debug.LogError("LevelUpBox.assignNew: upgradeList is null or empty.");
            ClearButtons();
            return;
        }

        List<int> valid = new List<int>();
        for (int i = 0; i < upgradeList.Length; i++)
        {
            if (upgradeList[i] != null && upgradeList[i] != nullUpgrade) valid.Add(i);
        }

        if (valid.Count == 0)
        {
            Debug.LogWarning("LevelUpBox.assignNew: no valid upgrades available (all null or nullUpgrade).");
            ClearButtons();
            return;
        }

        idx1 = idx2 = idx3 = -1;
        int picks = Mathf.Min(3, valid.Count);
        for (int p = 0; p < picks; p++)
        {
            int r = Random.Range(0, valid.Count);
            int chosen = valid[r];
            valid.RemoveAt(r);
            if (p == 0) idx1 = chosen;
            else if (p == 1) idx2 = chosen;
            else if (p == 2) idx3 = chosen;
        }

        SetButtonToUpgrade(button1Text, button1Image, idx1);
        SetButtonToUpgrade(button2Text, button2Image, idx2);
        SetButtonToUpgrade(button3Text, button3Image, idx3);

        if (playerLevel != null)
        {
            if (maxHealthIndicator != null) maxHealthIndicator.text = "Max Health: " + playerLevel.getMaxHp();
            if (levelIndicator != null) levelIndicator.text = "Level: " + playerLevel.getLevel();
            if (speedIndicator != null) speedIndicator.text = "Speed: " + playerLevel.getSpeed() / 100f;
        }
        else
        {
            Debug.LogWarning("LevelUpBox.assignNew: playerLevel is null.");
        }

        if (playerArmory != null)
        {
            if (damageIndicator != null) damageIndicator.text = "Damage: " + playerArmory.getDamage();
        }
        else
        {
            Debug.LogWarning("LevelUpBox.assignNew: playerArmory is null.");
        }
    }

    private void ClearButtons()
    {
        SetButtonToUpgrade(button1Text, button1Image, -1);
        SetButtonToUpgrade(button2Text, button2Image, -1);
        SetButtonToUpgrade(button3Text, button3Image, -1);
    }

    private void SetButtonToUpgrade(Text txt, Image img, int idx)
    {
        if (txt != null)
        {
            if (idx < 0 || upgradeList == null || idx >= upgradeList.Length || upgradeList[idx] == null || upgradeList[idx] == nullUpgrade)
                txt.text = "None";
            else
                txt.text = string.IsNullOrEmpty(upgradeList[idx].upgradeName) ? "Unnamed Upgrade" : upgradeList[idx].upgradeName;
        }

        if (img != null)
        {
            if (idx < 0 || upgradeList == null || idx >= upgradeList.Length || upgradeList[idx] == null || upgradeList[idx] == nullUpgrade)
                img.sprite = null;
            else
                img.sprite = upgradeList[idx].icon;
        }
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
