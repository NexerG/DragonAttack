using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

[Serializable] public class Player
{
    public int CastleLevel = 0;
    public int Archer = 0;
    public int WizNormal = 0;
    public int WizFire = 0;
    public int WizIce = 0;
    public int money = 0;
    public int wave = 1;

    public const int MaxCastle = 2;
}

public class GameManager : MonoBehaviour
{
    #region manager
    [SerializeField] private GameObject DeathUI;
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject MainUI;

    [SerializeField] private int TotalBudget;
    private int CurrentBudget;

    [SerializeField] private List<GameObject> EnemyTypes = new List<GameObject>();
    private int EnemyIndex = 0;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float RateOffset = 0.5f;

    [SerializeField] internal GameObject castle;
    [SerializeField] private GameObject AttackPoint;

    private float nextSpawn = 0f;
    internal List<GameObject> enemies = new List<GameObject>();
    internal int gamespeed = 1;

    [SerializeField] List<Image> moneyUI = new List<Image>();
    [SerializeField] internal List<Image> WaveUI = new List<Image>();
    [SerializeField] internal List<Sprite> numbers = new List<Sprite>();

    [SerializeField] internal AudioSource music;
    internal bool isAudioPlaying=true;
    #endregion

    internal bool isRound = false;
    internal Player data;

    #region ad
    internal RewardedAd _rewardedAd;
    private string _adUnitID = "ca-app-pub-3940256099942544/5224354917";
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        PrepareAd();

        Time.timeScale = 0;

        data = load();
        applyLevels();
        addMoney(0);

        SetWaveCounter();
    }
    // Update is called once per frame
    void Update()
    {
        if (isRound)
            if (Time.time > nextSpawn)
            {
                float offset = UnityEngine.Random.Range(-RateOffset, RateOffset);
                nextSpawn = Time.time + spawnRate + offset;

                float percentage = UnityEngine.Random.Range(2f, 3f);
                Spawner(percentage);
            }
    }
    private void CalculateBudget()
    {
        TotalBudget = data.wave * 10;
        CurrentBudget = TotalBudget;
    }
    private void Spawner(float percentage)
    {
        float spending = TotalBudget * percentage / 100;
        if (spending < 1)
        {
            GameObject enemy;
            if (spending < CurrentBudget)
            {
                CurrentBudget = (int)(CurrentBudget - Mathf.Floor(spending) - 1);
                while (spending > 0)
                {
                    float y = UnityEngine.Random.Range(2f, 8.5f);
                    int z = UnityEngine.Random.Range(0, EnemyTypes.Count);
                    enemy = Instantiate(EnemyTypes[z], new Vector3(11.5f, y, 0f), Quaternion.Euler(0, 0, 0));
                    switch (z)
                    {
                        case 0:
                            enemy.GetComponent<SimpleEnemyController>().AttackPoint = AttackPoint;
                            enemy.GetComponent<SimpleEnemyController>().manager = gameObject.GetComponent<GameManager>();
                            enemy.GetComponent<SimpleEnemyController>().index = EnemyIndex;
                            EnemyIndex++;
                            break;
                        case 1:
                            enemy.GetComponent<HealingEnemyController>().AttackPoint = AttackPoint;
                            enemy.GetComponent<HealingEnemyController>().manager = gameObject.GetComponent<GameManager>();
                            enemy.GetComponent<HealingEnemyController>().index = EnemyIndex;
                            EnemyIndex++;
                            break;
                        default: break;
                    }
                    enemies.Add(enemy);
                    spending--;
                }
            }
            else
            {
                while (CurrentBudget > 0)
                {
                    float y = UnityEngine.Random.Range(2f, 4f);
                    int z = UnityEngine.Random.Range(0, EnemyTypes.Count);
                    enemy = Instantiate(EnemyTypes[z], new Vector3(11.5f, y, 0f), Quaternion.Euler(0, 0, 0));
                    switch (z)
                    {
                        case 0:
                            enemy.GetComponent<SimpleEnemyController>().AttackPoint = AttackPoint;
                            enemy.GetComponent<SimpleEnemyController>().manager = gameObject.GetComponent<GameManager>();
                            enemy.GetComponent<SimpleEnemyController>().index = EnemyIndex;
                            EnemyIndex++;
                            break;
                        case 1:
                            enemy.GetComponent<HealingEnemyController>().AttackPoint = AttackPoint;
                            enemy.GetComponent<HealingEnemyController>().manager = gameObject.GetComponent<GameManager>();
                            enemy.GetComponent<HealingEnemyController>().index = EnemyIndex;
                            EnemyIndex++;
                            break;
                        default: break;
                    }
                    enemies.Add(enemy);
                    CurrentBudget--;
                }
            }
        }
        else
        {
            GameObject enemy;
            float y = UnityEngine.Random.Range(2f, 4f);
            int z = UnityEngine.Random.Range(0, EnemyTypes.Count);
            enemy = Instantiate(EnemyTypes[z], new Vector3(11.5f, y, 0f), Quaternion.Euler(0, 0, 0));
            switch (z)
            {
                case 0:
                    enemy.GetComponent<SimpleEnemyController>().AttackPoint = AttackPoint;
                    enemy.GetComponent<SimpleEnemyController>().manager = gameObject.GetComponent<GameManager>();
                    enemy.GetComponent<SimpleEnemyController>().index = EnemyIndex;
                    EnemyIndex++;
                    break;
                case 1:
                    enemy.GetComponent<HealingEnemyController>().AttackPoint = AttackPoint;
                    enemy.GetComponent<HealingEnemyController>().manager = gameObject.GetComponent<GameManager>();
                    enemy.GetComponent<HealingEnemyController>().index = EnemyIndex;
                    EnemyIndex++;
                    break;
                default: break;
            }
            enemies.Add(enemy);
            CurrentBudget--;
        }
        if (CurrentBudget <= 0 && enemies.Count <= 0)
        {
            FinishSpawning();
        }
    }
    private void FinishSpawning()
    {
        isRound = false;
    }
    internal void EndRound()
    {
        if (enemies.Count == 0)
        {
            EnemyIndex = 0;
            data.wave++;

            save(data);
            castle.GetComponent<Castle>().HP = castle.GetComponent<Castle>().MaxHP;
            startStage();
        }
    }
    internal void startStage()
    {
        isRound = true;
        CalculateBudget();
        SetWaveCounter();
        if(isAudioPlaying)
            StartMusic();
    }
    internal void GameOver()
    {
        isRound = false;

        ShopUI.SetActive(false);
        MainUI.SetActive(false);
        GameUI.SetActive(false);
        DeathUI.SetActive(true);

        foreach (GameObject g in enemies)
        {
            Destroy(g);
            enemies.Remove(g);
        }
        StopMusic();
    }
    internal void save(Player data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/playerData.json", jsonData);
    }
    internal Player load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.json"))
        {
            string jsonData = File.ReadAllText(Application.persistentDataPath + "/playerData.json");
            return JsonUtility.FromJson<Player>(jsonData);
        }
        else
        {
            Debug.LogError("Save file not found");
            data = new Player();
            save(data);
            return data;
        }
    }
    internal void applyLevels()
    {
        castle.GetComponent<Castle>().SetupCastle(data);
    }
    internal void PrepareAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        var adRequest = new AdRequest();
        RewardedAd.Load(_adUnitID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load. With Error: " + error);
                return;
            }
            _rewardedAd = ad;
            Debug.Log("Ad loaded");
        });
    }
    internal void addMoney(int money)
    {
        data.money += money;
        save(data);

        int dummyMoney = data.money;
        for (int i = 0; i < moneyUI.Count; i++)
        {
            moneyUI[i].sprite = numbers[dummyMoney % 10];
            dummyMoney /= 10;
        }
    }
    private void SetWaveCounter()
    {
        int x = data.wave;
        for (int i = 0; i < WaveUI.Count / 2; i++)
        {
            WaveUI[i].sprite = numbers[x % 10];
            WaveUI[i + 3].sprite = numbers[x % 10];
            x /= 10;
        }
    }
    internal void StartMusic()
    {
        music.Play();
    }
    internal void StopMusic()
    {
        music.Stop();
    }

    internal void PlayStopAudio(bool b)
    {
        if(b)
        {
            StartMusic();
            isAudioPlaying = true;
        }
    }
}