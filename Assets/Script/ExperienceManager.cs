using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] Image expBar;
    private PlayerLevel playerInfo;
    void Start()
    {
        expBar.fillAmount = 0;
        playerInfo = GameObject.Find("Player").GetComponent<PlayerLevel>();
    }

    
    void Update()
    {
        expBar.fillAmount = (playerInfo.getExp()/ playerInfo.getNeededExp());
    }

    public void recieveSignal(float exp)
    {
        playerInfo.updateExp(exp);
    }
}
