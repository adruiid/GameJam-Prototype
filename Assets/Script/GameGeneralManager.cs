using UnityEngine;
using UnityEngine.UI;

public class GameGeneralManager : MonoBehaviour
{
    private float time;
    public Text timerText;

    void Update()
    {
        time += Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}
