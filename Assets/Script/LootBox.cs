
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : MonoBehaviour
{
    [SerializeField] GameObject closedChest;
    [SerializeField] GameObject openChest;
    [SerializeField] Image icon;
    [SerializeField] Transform upgradeIconPos;
    [SerializeField] UpgradeContainers[] upgradesList;
    [SerializeField] GameObject parentCanvas;
    [SerializeField] Text upgradeText;



    [SerializeField] GameObject lootBoxMenu;
    [SerializeField] GameObject gameUI;

    private bool lootBoxMenuActive;
    private PauseMenu pauseMenu;

    private PlayerLevel playerLevel;

    private void Start()
    {
        lootBoxMenuActive = false;
        pauseMenu = GameObject.Find("Game Manager").GetComponent<PauseMenu>();
        playerLevel = GameObject.Find("Player").GetComponent<PlayerLevel>();
    }



    public void activateLootboxMenu()
    {
        gameUI.SetActive(false);
        lootBoxMenu.SetActive(true);
        pauseMenu.setPauseStatus(true);
        Time.timeScale = 0f;
        lootBoxMenuActive = true;
    }

    public void lootBoxDoneSignal()
    {
        upgradeText.gameObject.SetActive(false);
        closedChest.SetActive(true);
        openChest.SetActive(false);
        lootBoxMenu.SetActive(false);
        gameUI.SetActive(true);
        pauseMenu.setPauseStatus(false);
        Time.timeScale = 1f;
        lootBoxMenuActive = false;
    }

    public bool isActive()
    {
        return lootBoxMenuActive;
    }



    public void OnChestClicked()
    {
        closedChest.SetActive(false);
        openChest.SetActive(true);

        StartCoroutine(AssignNewUpgrade());
    }


    IEnumerator AssignNewUpgrade()
    {
        Image iconNew=Instantiate(icon, upgradeIconPos.position, Quaternion.identity, parentCanvas.transform);

        Animator animator = iconNew.GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        UpgradeContainers newUpgrade = upgradesList[Random.Range(0, upgradesList.Length)];
        playerUpgrade(newUpgrade.id);
        iconNew.sprite = newUpgrade.icon;

        yield return new WaitForSecondsRealtime(0.32f);
        upgradeText.text = newUpgrade.upgradeName;
        upgradeText.gameObject.SetActive(true);

        float timer = 0f;
        float duration = 2f;

        while (timer < duration)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(iconNew.gameObject);
                lootBoxDoneSignal();
                yield break;
            }

            timer += Time.unscaledDeltaTime;
        }


        yield return new WaitForSecondsRealtime(2f);
        Destroy(iconNew.gameObject);
        lootBoxDoneSignal();
    }

    public bool lootBoxActiveStatus()
    {
        return lootBoxMenuActive;
    }

    private void playerUpgrade(int upgradeID)
    {
        if (upgradeID == 11)
        {
            playerLevel.temporarySpeedUpgrade();
        } else if (upgradeID == 12)
        {
            playerLevel.temporaryImmunity();
        } else
        {
            playerLevel.temporaryDamageUpgrade();
        }
    }

}
