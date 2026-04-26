using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private bool isOver;

    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject overUI;

    PauseMenu pauseMenu;

    private PlayerLevel playerLevel;

    void Start()
    {
        playerLevel=GameObject.Find("Player").GetComponent<PlayerLevel>();
        pauseMenu = GetComponent<PauseMenu>();
    }


    void Update()
    {
       if(playerLevel.getCurrentHp() <= 0 && !isOver)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        pauseMenu.setPauseStatus(true);
        isOver = true;
        gameUI.SetActive(false);
        overUI.SetActive(true);
        pauseMenu.setScriptStatusChangable(false);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
