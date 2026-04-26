using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;

    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;

    private bool scriptStatusChangable = true;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!scriptStatusChangable) return;

            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        isPaused = !isPaused;

        gameUI.SetActive(!isPaused);
        pauseUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public bool pauseStatus()
    {
        return isPaused;
    }

    public void setPauseStatus(bool status)
    {
        isPaused = status;
    }

    public void setScriptStatusChangable(bool status)
    {
        scriptStatusChangable = status;
    }

}
