using UnityEngine;
using UnityEngine.UI;

public class GameGeneralManager : MonoBehaviour
{
    private float time;
    public Text timerText;
    float enemyKillCount;

    [SerializeField] Image missileIcon;
    [SerializeField] Image flameThrowerIcon;
    [SerializeField] Text killCount;

    [SerializeField] Image hpBar;

    PlayerArmory playerArmory;
    PlayerLevel playerLevel;

    void Start()
    {
        playerArmory= GameObject.Find("Player").GetComponent<PlayerArmory>();
        playerLevel = GameObject.Find("Player").GetComponent<PlayerLevel>();
    }

    void Update()
    {
        time += Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);

        hpBar.fillAmount= playerLevel.getCurrentHp() / playerLevel.getMaxHp();

        checkForIcon();
    }

    void checkForIcon()
    {
        if (playerArmory.hasHomingMissiles) missileIcon.gameObject.SetActive(true);
        if (playerArmory.hasFlameThrower) flameThrowerIcon.gameObject.SetActive(true);
    }

    public void killSignal()
    {
        enemyKillCount++;
        killCount.text = "" + enemyKillCount;
    }
}
