using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject settingsUI;
    [SerializeField] GameObject creditsUI;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openSettingsUI()
    {
        mainMenuUI.SetActive(false);
        settingsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void openCreditsUI()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void returnToMainMenu()
    {
        mainMenuUI.SetActive(true);
        creditsUI.SetActive(false);
        settingsUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
