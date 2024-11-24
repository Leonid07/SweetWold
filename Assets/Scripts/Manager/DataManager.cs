using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    public Map[] levels;
    public Sprite[] spriteNumber;

    [Header("Обучение")]
    public int valueEd = 0; //  0 не пройдёно 1 пройдено
    public string _idLevelEducation = "_idLevelEducation";

    [Header("Сохранение скинов")]
    public Sprite[] spriteSkinHero;
    public int indexSpriteSkinHero = 0;
    public string idSkinHero = "idSkinHero";

    public Text[] textButtonShop;
    public string[] idCount = new string[] { "", "", "900", "5000", "5000" };
    public string[] idSkin = new string[] { "skin0", "skin1", "skin2", "skin3", "skin4" };

    public Map mapNextLevel;

    public void SaveSkin()
    {
        PlayerPrefs.SetInt(idSkinHero, indexSpriteSkinHero);
        for (int i = 0; i < idSkin.Length; i++)
        {
            PlayerPrefs.SetString(idSkin[i], idCount[i]);
        }
    }
    public void LoadSkin()
    {
        if (PlayerPrefs.HasKey(idSkinHero))
        {
            indexSpriteSkinHero = PlayerPrefs.GetInt(idSkinHero);
            PanelManager.InstancePanel.UplyChange(spriteSkinHero[indexSpriteSkinHero]);
        }
        for (int i = 0; i < idSkin.Length; i++)
        {
            if (PlayerPrefs.HasKey(idSkin[i]))
            {
                idCount[i] = PlayerPrefs.GetString(idSkin[i]);
                textButtonShop[i].text = PlayerPrefs.GetString(idSkin[i]);
            }
        }
    }

    public static DataManager InstanceData { get; private set; }

    private void Awake()
    {
        if (InstanceData != null && InstanceData != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstanceData = this;
            DontDestroyOnLoad(gameObject);
        }
        SetSpriteNumber();
    }

    private void Start()
    {
        PanelManager.InstancePanel.panelMap.SetActive(true);
        SetIndexLevel();
        LoadLevel();
        LoadGold();
        LoadPowerPlayer();
        LoadLevelPlayer();
        LoadFirstUpdate();
        LoadEducation();
        LoadSkin();
        PanelManager.InstancePanel.panelMap.SetActive(false);
    }

    public void SetSpriteNumber()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].imageNumber = spriteNumber[i];
        }
    }

    public void SetIndexLevel()
    {
        int count = 1;
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].indexLevel = count;
            count++;
        }
    }

    public void SaveLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            PlayerPrefs.SetInt(levels[i].idLevel, levels[i].isLoad);
            PlayerPrefs.Save();
        }
    }
    public void LoadLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (PlayerPrefs.HasKey(levels[i].idLevel))
            {
                levels[i].isLoad = PlayerPrefs.GetInt(levels[i].idLevel);
                levels[i].CheckLevel();
            }
        }
    }

    public void SaveFirstUpdate()
    {
        PlayerPrefs.SetInt(PanelManager.InstancePanel.idUpdateCost, PanelManager.InstancePanel.updateCost);
        PlayerPrefs.Save();
    }

    public void LoadFirstUpdate()
    {
        if (PlayerPrefs.HasKey(PanelManager.InstancePanel.idUpdateCost))
        {
            PanelManager.InstancePanel.updateCost = PlayerPrefs.GetInt(PanelManager.InstancePanel.idUpdateCost);
        }
    }

    // сохранение прогресса обучения
    public void SaveEducation()
    {
        PlayerPrefs.SetInt(_idLevelEducation, valueEd);
        PlayerPrefs.Save();
    }

    public void LoadEducation()
    {
        if (PlayerPrefs.HasKey(_idLevelEducation))
        {
            valueEd = PlayerPrefs.GetInt(_idLevelEducation);
        }
    }

    public void SaveGold()
    {
        GameManager.InstanceGame.ApplyGold();
        PlayerPrefs.SetInt(GameManager.InstanceGame.idGold, GameManager.InstanceGame.gold);
        PlayerPrefs.Save();
    }
    public void LoadGold()
    {
        if (PlayerPrefs.HasKey(GameManager.InstanceGame.idGold))
        {
            GameManager.InstanceGame.gold = PlayerPrefs.GetInt(GameManager.InstanceGame.idGold);
            GameManager.InstanceGame.ApplyGold();
        }
    }

    public void SavePowerPlayer()
    {
        PlayerPrefs.SetInt(PanelManager.InstancePanel.idPowerPlayer, PanelManager.InstancePanel.powerPlayer);
        PlayerPrefs.Save();
    }
    public void LoadPowerPlayer()
    {
        if (PlayerPrefs.HasKey(PanelManager.InstancePanel.idPowerPlayer))
        {
            //textPlayerDamageMainMenu
            PanelManager.InstancePanel.powerPlayer = PlayerPrefs.GetInt(PanelManager.InstancePanel.idPowerPlayer);
            PanelManager.InstancePanel.textPlayerDamage.text = PlayerPrefs.GetInt(PanelManager.InstancePanel.idPowerPlayer).ToString();
            PanelManager.InstancePanel.textPlayerDamageMainMenu.text = PlayerPrefs.GetInt(PanelManager.InstancePanel.idPowerPlayer).ToString();
        }
    }
    public void SaveLevelPlayer()
    {
        PlayerPrefs.SetInt(PanelManager.InstancePanel.idLevelPLayer, PanelManager.InstancePanel.levelPLayer);
        PlayerPrefs.Save();
    }
    public void LoadLevelPlayer()
    {
        if (PlayerPrefs.HasKey(PanelManager.InstancePanel.idLevelPLayer))
        {
            //characterLevelMainMenu
            PanelManager.InstancePanel.levelPLayer = PlayerPrefs.GetInt(PanelManager.InstancePanel.idLevelPLayer);
            PanelManager.InstancePanel.characterLevel.text = PlayerPrefs.GetInt(PanelManager.InstancePanel.idLevelPLayer).ToString();
            PanelManager.InstancePanel.characterLevelMainMenu.text = PlayerPrefs.GetInt(PanelManager.InstancePanel.idLevelPLayer).ToString();
        }
    }
}