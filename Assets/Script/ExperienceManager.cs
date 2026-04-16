using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] Image expBar;
    [SerializeField] Text levelText;
    private PlayerLevel playerInfo;
    void Start()
    {
        expBar.fillAmount = 0;
        playerInfo = GameObject.Find("Player").GetComponent<PlayerLevel>();
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
}
