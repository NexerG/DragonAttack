using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopFunctions : MonoBehaviour
{
    [SerializeField] GameObject manager;
    [SerializeField] List<Image> moneyUI = new List<Image>();

    public void UpgradeArcher()
    {
        if (manager.GetComponent<GameManager>().data.money >99)
        {
            manager.GetComponent<GameManager>().castle.GetComponent<Castle>().LevelArcher();
            manager.GetComponent<GameManager>().data.money -= 100;
            UpdateMoney();
            respawnCharacters();
        }
    }
    public void UpgradeWizard()
    {
        if (manager.GetComponent<GameManager>().data.money > 99)
        {
            manager.GetComponent<GameManager>().castle.GetComponent<Castle>().LevelWizard();
            manager.GetComponent<GameManager>().data.money -= 100;
            UpdateMoney();
            respawnCharacters();
        }
    }
    public void UpgradeFireWizard()
    {
        if (manager.GetComponent<GameManager>().data.money > 99)
        {
            if (manager.GetComponent<GameManager>().data.CastleLevel >= Player.MaxCastle)
                manager.GetComponent<GameManager>().castle.GetComponent<Castle>().LevelFireWizard();
            manager.GetComponent<GameManager>().data.money -= 100;
            UpdateMoney();
            respawnCharacters();
        }
    }
    public void UpgradeIceWizard()
    {
        if (manager.GetComponent<GameManager>().data.money > 99)
        {
            if (manager.GetComponent<GameManager>().data.CastleLevel >= Player.MaxCastle)
                manager.GetComponent<GameManager>().castle.GetComponent<Castle>().LevelIceWizard();
            manager.GetComponent<GameManager>().data.money -= 100;
            UpdateMoney();
            respawnCharacters();
        }
    }
    private void respawnCharacters()
    {
        foreach (GameObject g in manager.GetComponent<GameManager>().castle.GetComponent<Castle>().Archers)
        {
            Destroy(g);
        }
        foreach (GameObject g in manager.GetComponent<GameManager>().castle.GetComponent<Castle>().WizardsNormal)
        {
            Destroy(g);
        }
        foreach (GameObject g in manager.GetComponent<GameManager>().castle.GetComponent<Castle>().WizardsIce)
        {
            Destroy(g);
        }
        foreach (GameObject g in manager.GetComponent<GameManager>().castle.GetComponent<Castle>().WizardsFire)
        {
            Destroy(g);
        }
        manager.GetComponent<GameManager>().castle.GetComponent<Castle>().Archers.Clear();
        manager.GetComponent<GameManager>().castle.GetComponent<Castle>().WizardsNormal.Clear();
        manager.GetComponent<GameManager>().castle.GetComponent<Castle>().WizardsIce.Clear();
        manager.GetComponent<GameManager>().castle.GetComponent<Castle>().WizardsFire.Clear();

        manager.GetComponent<GameManager>().castle.GetComponent<Castle>().SetupCastle(manager.GetComponent<GameManager>().data);
    }
    public void UpgradeCastle()
    {
        if (manager.GetComponent<GameManager>().data.money > 99)
        {
            if (manager.GetComponent<GameManager>().data.CastleLevel < Player.MaxCastle)
                manager.GetComponent<GameManager>().castle.GetComponent<Castle>().LevelCastle();
            manager.GetComponent<GameManager>().data.money -= 100;
            UpdateMoney();
        }
    }
    public void RemoveAds()
    {

    }
    public void RewardsOne()
    {
        if (manager.GetComponent<GameManager>()._rewardedAd != null && manager.GetComponent<GameManager>()._rewardedAd.CanShowAd())
        {
            manager.GetComponent<GameManager>()._rewardedAd.Show((Reward reward) =>
            {
                manager.GetComponent<GameManager>().data.money += 100;
                Debug.Log("Rewarded");
            });
        }
        else
        {
            Debug.LogWarning("No ad prepared. Loading");
            manager.GetComponent<GameManager>().PrepareAd();
            if (manager.GetComponent<GameManager>()._rewardedAd != null && manager.GetComponent<GameManager>()._rewardedAd.CanShowAd())
            {
                manager.GetComponent<GameManager>()._rewardedAd.Show((Reward reward) =>
                {
                    manager.GetComponent<GameManager>().data.money += 100;
                    Debug.Log("Rewarded");
                });
            }
        }
    }
    public void RewardsTwo()
    {
        Debug.Log(manager.GetComponent<GameManager>().data.money);
    }
    public void UpdateMoney()
    {
        int dummyMoney = manager.GetComponent<GameManager>().data.money;
        for (int i = 0; i < moneyUI.Count; i++)
        {
            moneyUI[i].sprite = manager.GetComponent<GameManager>().numbers[dummyMoney % 10];
            dummyMoney /= 10;
        }
    }
}
