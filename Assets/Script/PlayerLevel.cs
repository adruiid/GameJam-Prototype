using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    private PlayerArmory armory;
    private int currentLevel;
    private float experiencePoint;
    private float neededExp;
    void Start()
    {
        armory = GetComponent<PlayerArmory>();
        currentLevel = 1;
        neededExp = 10;
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
        experiencePoint = 0;
        currentLevel += 1;
        neededExp += 5;
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
}
