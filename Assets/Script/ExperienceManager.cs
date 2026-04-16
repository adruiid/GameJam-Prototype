using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] Image expBar;
    [SerializeField] Text levelText;
    [SerializeField] Image levelUpBox;
    LevelUpBox levelUpBoxScript;
    private PlayerLevel playerInfo;
    void Start()
    {
        expBar.fillAmount = 0;
        playerInfo = GameObject.Find("Player").GetComponent<PlayerLevel>();
        levelUpBoxScript = GameObject.Find("Game Manager").GetComponent<LevelUpBox>();
    }

    
    void Update()
    {
        expBar.fillAmount = (playerInfo.getExp()/ playerInfo.getNeededExp());
        levelText.text = "Lv. " + playerInfo.getLevel();
    }

    public void recieveSignal(float exp)
    {
        playerInfo.updateExp(exp);
    }

    public void recieveLevelUpSignal()
    {
        levelUpBox.gameObject.SetActive(true);
        levelUpBoxScript.assignNew();
        Time.timeScale = 0f;
    }

    public void recieveLeveledUpSignal()
    {
        levelUpBox.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
