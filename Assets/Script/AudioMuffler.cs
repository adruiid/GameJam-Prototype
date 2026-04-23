using UnityEngine;

public class AudioMuffler : MonoBehaviour
{
    private ExperienceManager expManager;
    [SerializeField]private AudioLowPassFilter lowPass;
    void Start()
    {
        expManager= GameObject.Find("Game Manager").GetComponent<ExperienceManager>();
        lowPass.cutoffFrequency = 22000f;
    }

    
    void Update()
    {
        SetPaused(expManager.levelUpBoxStatus());
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
