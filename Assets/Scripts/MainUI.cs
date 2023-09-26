using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    private TextMeshProUGUI goldCounter, goldObjectiveCounter, shopWelcome;
    private PlayerHideout playerHideout;
    private GameObject shopScreen;
    private UpgradeableStatShop sailsStatShop, rudderStatShop, hullStatShop, loaderStatShop, gunpowderStatShop, boostStatShop, armorStatShop;
    private GameObject pauseMenu, winScreen, loseScreen;

    private void Awake()
    {
        playerHideout = FindObjectOfType<PlayerHideout>();
        goldCounter = transform.Find("Txt_PlayerGoldCounter").GetComponent<TextMeshProUGUI>();
        goldObjectiveCounter = transform.Find("Txt_GoldObjectiveCounter").GetComponent<TextMeshProUGUI>();
        shopScreen = transform.Find("Shop").gameObject;
        shopWelcome = shopScreen.transform.Find("Txt_Welcome").GetComponent<TextMeshProUGUI>();

        pauseMenu = transform.Find("PauseMenu").gameObject;
        pauseMenu.SetActive(false);
        winScreen = transform.Find("WinScreen").gameObject;
        winScreen.SetActive(false);
        loseScreen = transform.Find("LoseScreen").gameObject;
        loseScreen.SetActive(false);
    }

    private void Start()
    {
        shopScreen.SetActive(false);

        shopWelcome.text = "Welcome, Captain " + GameManager.Instance.playerName;

        sailsStatShop = shopScreen.transform.Find("SailsStatShop").GetComponent<UpgradeableStatShop>();
        sailsStatShop.SetStat(GameManager.Instance.player.sails);

        rudderStatShop = shopScreen.transform.Find("RudderStatShop").GetComponent<UpgradeableStatShop>();
        rudderStatShop.SetStat(GameManager.Instance.player.rudder);

        hullStatShop = shopScreen.transform.Find("HullStatShop").GetComponent<UpgradeableStatShop>();
        hullStatShop.SetStat(GameManager.Instance.player.hull);

        loaderStatShop = shopScreen.transform.Find("LoaderStatShop").GetComponent<UpgradeableStatShop>();
        loaderStatShop.SetStat(GameManager.Instance.player.loader);

        gunpowderStatShop = shopScreen.transform.Find("GunpowderStatShop").GetComponent<UpgradeableStatShop>();
        gunpowderStatShop.SetStat(GameManager.Instance.player.gunpowder);

        boostStatShop = shopScreen.transform.Find("BoostStatShop").GetComponent<UpgradeableStatShop>();
        boostStatShop.SetStat(GameManager.Instance.player.boostStat);

        armorStatShop = shopScreen.transform.Find("ArmorStatShop").GetComponent<UpgradeableStatShop>();
        armorStatShop.SetStat(GameManager.Instance.player.armor);
    }

    private void Update()
    {
        goldCounter.text = playerHideout.currentGold + " gold";
        if (!GameManager.Instance.isEndlessMode)
        {
            goldObjectiveCounter.text = playerHideout.totalGold + " / " + GameManager.Instance.selectedDifficultySetting.goldTarget + "g";
        }
        else
        {
            goldObjectiveCounter.text = playerHideout.totalGold + "g";
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !(winScreen.activeSelf || loseScreen.activeSelf))
        {
            if (pauseMenu.activeSelf)
            {
                UnPause();

            }
            else
            {
                Pause();
            }
        }
    }

    public void OpenShop()
    {
        shopScreen.SetActive(true);
    }

    public void CloseShop()
    {
        shopScreen.SetActive(false);
    }

    public void Pause() //pause the game
    {

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);


    }
    public void UnPause() //unpause the game
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void Win() //runs when player wins
    {
        Time.timeScale = 0f;
        winScreen.SetActive(true);
    }

    public void Lose() //runs when player loses. also pauses game
    {
        Time.timeScale = 0f;
        loseScreen.SetActive(true);
    }
}
