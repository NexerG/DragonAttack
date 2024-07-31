using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] internal float HP = 100;
    [SerializeField] internal float MaxHP = 100;
    [SerializeField] internal GameManager gameManager;

    [SerializeField] internal List<Sprite> CastleLevelSprites = new List<Sprite>();
    [SerializeField] internal SpriteRenderer CastleSpriteObj;
    [SerializeField] internal SpriteRenderer CastleWearObj;
    [SerializeField] internal SpriteMask mask;
    [SerializeField] internal List<Sprite> CastleWearSprites = new List<Sprite>();

    [SerializeField] internal List<GameObject> units = new List<GameObject>();
    [SerializeField] internal List<Transform> spawnPlaces = new List<Transform>();

    internal List<GameObject> Archers = new List<GameObject>();
    internal List<GameObject> WizardsNormal = new List<GameObject>();
    internal List<GameObject> WizardsIce = new List<GameObject>();
    internal List<GameObject> WizardsFire = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetupCastle(Player data)
    {
        //castle
        CastleSpriteObj.sprite = CastleLevelSprites[data.CastleLevel];
        mask.sprite = CastleSpriteObj.sprite;
        switch (data.CastleLevel)
        {
            case (1):
                CastleSpriteObj.gameObject.transform.localPosition = new Vector3(0, 2.65f, 0);
                mask.gameObject.transform.localPosition = new Vector3(0, 2.65f, 0);
                break;
            case (2):
                CastleSpriteObj.gameObject.transform.localPosition = new Vector3(0, 5.45f, 0);
                mask.gameObject.transform.localPosition = new Vector3(0, 5.45f, 0);
                break;
            case (3):
                CastleSpriteObj.gameObject.transform.localPosition = new Vector3(0, 8.05f, 0);
                mask.gameObject.transform.localPosition = new Vector3(0, 8.05f, 0);
                break;
            default: break;
        }
        //archers
        for (int i = 0; i <= data.Archer && i < 4; i++)
        {
            Archers.Add(Instantiate(units[0], spawnPlaces[i].position, Quaternion.Euler(0, 0, 0)));
            Archers[i].GetComponent<ArcherBehaviour>().manager = gameManager;
            Archers[i].GetComponent<ArcherBehaviour>().Level = data.Archer;
        }
        //normal wizard
        for (int i = 4; i <= data.WizNormal + 3 && i < 7; i++)
        {
            WizardsNormal.Add(Instantiate(units[1], spawnPlaces[i].position, Quaternion.Euler(0, 0, 0)));
            WizardsNormal[i - 4].GetComponent<FireBoltWizardController>().manager = gameManager;
            WizardsNormal[i - 4].GetComponent<FireBoltWizardController>().level = data.WizNormal;
        }
        //fire wizard
        for (int i = 7; i <= data.WizFire + 6 && i < 10; i++)
        {
            WizardsFire.Add(Instantiate(units[2], spawnPlaces[i].position, Quaternion.Euler(0, 0, 0)));
            WizardsFire[i - 7].GetComponent<FireBoltWizardController>().manager = gameManager;
            WizardsFire[i - 7].GetComponent<FireBoltWizardController>().level = data.WizFire;
        }
        //ice wizard
        for (int i = 10; i <= data.WizIce + 9 && i < 13; i++)
        {
            WizardsIce.Add(Instantiate(units[3], spawnPlaces[i].position, Quaternion.Euler(0, 0, 0)));
            WizardsIce[i - 10].GetComponent<FireBoltWizardController>().manager = gameManager;
            WizardsIce[i - 10].GetComponent<FireBoltWizardController>().level = data.WizIce;
        }
    }
    internal void DoDamage(float d)
    {
        HP -= d;
        CheckHP();
        UpdateHealtbar();
    }
    internal void CheckHP()
    {
        if (HP <= 0)
        {
            gameManager.GameOver();
        }
    }
    private void UpdateHealtbar()
    {
        //healthbar update logic
        float x = (100 - HP) / 3;
        x = math.min(x, 1);
        if (HP<75)
        {
            CastleWearObj.sprite = CastleWearSprites[(int)x];
        }
    }
    #region Levelups
    internal void LevelCastle()
    {
        gameManager.data.CastleLevel++;
        CastleSpriteObj.sprite = CastleLevelSprites[gameManager.data.CastleLevel];
        mask.sprite = CastleSpriteObj.sprite;
        switch (gameManager.data.CastleLevel)
        {
            case (1):
                CastleSpriteObj.gameObject.transform.localPosition = new Vector3(0, 2.65f, 0);
                mask.gameObject.transform.localPosition = new Vector3(0, 2.65f, 0);
                break;
            case (2):
                CastleSpriteObj.gameObject.transform.localPosition = new Vector3(0, 5.45f, 0);
                mask.gameObject.transform.localPosition = new Vector3(0, 5.45f, 0);
                break;
            case (3):
                CastleSpriteObj.gameObject.transform.localPosition = new Vector3(0, 8.05f, 0);
                mask.gameObject.transform.localPosition = new Vector3(0, 8.05f, 0);
                break;
            default: break;
        }
        SaveLevels();
    }
    internal void LevelArcher()
    {
        foreach(GameObject x in Archers)
        {
            x.GetComponent<ArcherBehaviour>().LevelUp();
        }
        gameManager.data.Archer++;
        SaveLevels();
    }
    internal void LevelWizard()
    {
        foreach (GameObject x in WizardsNormal)
        {
            x.GetComponent<FireBoltWizardController>().LevelUp();
        }
        gameManager.data.WizNormal++;
        SaveLevels();
    }
    internal void LevelIceWizard()
    {
        foreach (GameObject x in WizardsIce)
        {
            x.GetComponent<FireBoltWizardController>().LevelUp();
        }
        gameManager.data.WizIce++;
        SaveLevels();
    }
    internal void LevelFireWizard()
    {
        foreach (GameObject x in WizardsFire)
        {
            x.GetComponent<FireBoltWizardController>().LevelUp();
        }
        gameManager.data.WizFire++;
        SaveLevels();
    }
    #endregion
    internal void SaveLevels()
    {
        gameManager.save(gameManager.data);
    }
}
