using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] Image expBar;
    [SerializeField] Text levelText;
    [SerializeField] GameObject levelUpBox;
    [SerializeField] PauseMenu pauseMenu;
    LevelUpBox levelUpBoxScript;
    private PlayerLevel playerInfo;
    private bool levelUpBoxActive;

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
        if (pauseMenu.pauseStatus())
        {
            return;
        }

        Time.timeScale = levelUpBoxActive ? 0f : 1f;
    }

    public void recieveSignal(float exp)
    {
        playerInfo.updateExp(exp);
    }

    public void recieveLevelUpSignal()
    {
        levelUpBox.gameObject.SetActive(true);
        levelUpBoxActive = true;
        levelUpBoxScript.assignNew();
    }

    public void recieveLeveledUpSignal()
    {
        levelUpBox.gameObject.SetActive(false);
        levelUpBoxActive = false;
        Time.timeScale = 1f;
    }

    public bool levelUpBoxStatus()
    {
        return levelUpBoxActive;
    }
}
