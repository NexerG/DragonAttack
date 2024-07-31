using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIfunctions : MonoBehaviour
{
    [SerializeField] GameObject UI_Main;
    [SerializeField] GameObject UI_Shop;
    [SerializeField] GameObject UI_Game;

    [SerializeField] GameObject UI_UpgradeCastle;
    [SerializeField] GameObject UI_UpgradeCastleText;

    [SerializeField] GameObject UI_UpgradeFireWizard;
    [SerializeField] GameObject UI_UpgradeFireText;
    [SerializeField] Sprite FireWizardSprite;

    [SerializeField] GameObject UI_UpgradeIceWizard;
    [SerializeField] GameObject UI_UpgradeIceWizardText;
    [SerializeField] Sprite IceWizardSprite;

    [SerializeField] Sprite LockSprite;

    public void OpenShop()
    {
        UI_Main.SetActive(false);
        UI_Game.SetActive(false);
        UI_Shop.SetActive(true);

        if(gameObject.GetComponent<GameManager>().data.CastleLevel >= Player.MaxCastle)
        {
            UI_UpgradeCastle.GetComponent<Image>().sprite = LockSprite;
            UI_UpgradeCastleText.GetComponent<TextMeshProUGUI>().text = "Castle\nMaxed";
        }

        if (gameObject.GetComponent<GameManager>().data.CastleLevel < Player.MaxCastle)
        {
            UI_UpgradeFireWizard.GetComponent<Image>().sprite = LockSprite;
            UI_UpgradeFireText.GetComponent<TextMeshProUGUI>().text = "Locked";

            UI_UpgradeIceWizard.GetComponent<Image>().sprite = LockSprite;
            UI_UpgradeIceWizardText.GetComponent<TextMeshProUGUI>().text = "Locked";
        }
        else
        {
            UI_UpgradeFireWizard.GetComponent<Image>().sprite = FireWizardSprite;
            UI_UpgradeFireText.GetComponent<TextMeshProUGUI>().text = "";

            UI_UpgradeIceWizard.GetComponent<Image>().sprite = IceWizardSprite;
            UI_UpgradeIceWizardText.GetComponent<TextMeshProUGUI>().text = "";
        }

        UI_Shop.GetComponent<ShopFunctions>().UpdateMoney();
        Time.timeScale = 0.5f;
    }
    public void Play()
    {
        UI_Main.SetActive(false);
        UI_Shop.SetActive(false);
        UI_Game.SetActive(true);
        if (!gameObject.GetComponent<GameManager>().isRound)
        {
            Debug.Log(!gameObject.GetComponent<GameManager>().isRound);
            gameObject.GetComponent<GameManager>().startStage();
        }
        Time.timeScale = 1f;
    }
    public void Pause()
    {
        UI_Main.SetActive(true);
        UI_Game.SetActive(false);
        Time.timeScale = 0f;
    }
    public void Volume()
    {
        //do volume
        if (!gameObject.GetComponent<GameManager>().isAudioPlaying)
            gameObject.GetComponent<GameManager>().PlayStopAudio(!gameObject.GetComponent<GameManager>().isAudioPlaying);
        else
        {
            gameObject.GetComponent<GameManager>().StopMusic();
            gameObject.GetComponent<GameManager>().isAudioPlaying = false;
        }
    }
    public void removeAds()
    {
        //do that
    }
}
