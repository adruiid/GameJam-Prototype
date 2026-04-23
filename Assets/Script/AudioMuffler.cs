using UnityEngine;

public class AudioMuffler : MonoBehaviour
{
    private ExperienceManager expManager;
    private PauseMenu pauseMenu;
    [SerializeField]private AudioLowPassFilter lowPass;
    void Start()
    {
        pauseMenu = GameObject.Find("Game Manager").GetComponent<PauseMenu>();
        expManager = GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
        lowPass.cutoffFrequency = 22000f;
    }

    
    void Update()
    {
        bool pauseStatus = expManager.levelUpBoxStatus() || pauseMenu.pauseStatus();
        SetPaused(pauseStatus);
    }

    private void SetPaused(bool paused)
    {
        if (paused)
        {
            lowPass.cutoffFrequency = 800f; // muffled
        }
        else
        {
            lowPass.cutoffFrequency = 22000f; // normal
        }
    }
}
