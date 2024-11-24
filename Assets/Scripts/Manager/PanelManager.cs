using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public AnimUnlockLevel animUnlockLevel;

    [Header("Кнопки в главном меню")]
    public Button buttonReward;
    public Button buttonPersonal;
    public Button buttonOptions;

    [Header("Пенели из главного меню")]
    public GameObject panelReward;
    public GameObject panelPersonal;
    public GameObject panelOptions;

    [Header("кнопки закрытия окон")]
    public Button buttonRewardClose;
    public Button buttonPersonalClose;
    public Button buttonOptionsClose;

    [Space(20)]
    [Header("Кнопки улучшения персонажа")]
    public Button buttonPlayerUpdate;
    public Button buttonPlayerPanelClose;
    public Button buttonUpdate;

    [Header("Текстовые панели в улучшении")]
    public Text textBeforeUpdate;
    public Text textAfterUpdate;
    public Text textPriceOnButton;
    public Text textPlayerDamage;
    public Text textPlayerDamageMainMenu;////// изменения

    public int damage = 350;
    public int updateCost = 150;
    public string idUpdateCost = "_costUpdate";

    public int powerPlayer;
    public string idPowerPlayer = "power";

    public int levelPLayer = 1;
    public string idLevelPLayer = "level_";

    public int countFirstUpdate;
    public double growthFactor = 1.5;

    [Header("Панель улучшения персонажа")]
    public GameObject panelUpdate;

    [Header("Персонаж")]
    public GameObject buttonStart;

    public GameObject[] panelIsActive;

    [Header("Банель проигрыша")]
    public GameObject panelGameover;
    public UIPanelFade _UIPanelFade;
    public Button buttonBackToMainMenu;
    public Button buttonInereasedLevel;

    [Header("Персонаж")]
    public Image characterMain;
    public Image characterUpgrade;

    [Header("Текст левел персонажа")]
    public Text characterLevel;
    public Text characterLevelMainMenu;/// изменения

    [Header("Панель выйгрыша")]
    public UIPanelFade panelWin;
    public Button buttonBackMainMenu;
    public Button buttonGoToLevelUpPanel;

    [Header("Переход в магазин и дорожку уровней")]
    public Button buttonShop;
    public Button buttonBackShop;
    public GameObject panelShop;

    public Button buttonMap;
    public Button buttonBackMap;
    public GameObject panelMap;

    public static PanelManager InstancePanel { get; private set; }

    private void Awake()
    {
        if (InstancePanel != null && InstancePanel != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InstancePanel = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        buttonReward.onClick.AddListener(() => ActivePanel(panelReward));
        buttonPersonal.onClick.AddListener(() => ActivePanel(panelPersonal));
        buttonOptions.onClick.AddListener(() => ActivePanel(panelOptions));

        buttonRewardClose.onClick.AddListener(() => ClosePanel(panelReward));
        buttonPersonalClose.onClick.AddListener(() => ClosePanel(panelPersonal));
        buttonOptionsClose.onClick.AddListener(() => ClosePanel(panelOptions));

        buttonPlayerUpdate.onClick.AddListener(() => { PanelUpdateActive(panelUpdate); SetValueForUpdate(); });
        buttonPlayerPanelClose.onClick.AddListener(() => PanelUpdateDisActive(panelUpdate));

        buttonUpdate.onClick.AddListener(() => { UpdatePlayer(); });

        buttonBackToMainMenu.onClick.AddListener(() => { SetActivePanel(); SetDisActiveButtonStart(); });

        buttonInereasedLevel.onClick.AddListener(() => { ActivePanel(panelPersonal); SetDisActiveButtonStart(); });

        buttonBackMainMenu.onClick.AddListener(() => { 
            SetDisActiveButtonStartPanelWin();
            Debug.Log(DataManager.InstanceData.mapNextLevel.mapNextLevel.isLoad);
            if (DataManager.InstanceData.mapNextLevel.mapNextLevel.isLoad == 0)
            {
                AnimOpenLevel();
                DataManager.InstanceData.mapNextLevel.OpenLevel();
            }
        });
        buttonGoToLevelUpPanel.onClick.AddListener(() => { ActivePanel(panelPersonal); SetDisActiveButtonStartPanelWin(); });

        buttonShop.onClick.AddListener(() => ActivePanel(panelShop));
        buttonMap.onClick.AddListener(() => ActivePanel(panelMap));

        buttonBackShop.onClick.AddListener(() => ClosePanel(panelShop));
        buttonBackMap.onClick.AddListener(() => ClosePanel(panelMap));

    }

    public void AnimOpenLevel()
    {
        animUnlockLevel.gameObject.SetActive(true);
        animUnlockLevel.StartAnimUnlockLevel();
    }

    public void PanelUpdateActive(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void PanelUpdateDisActive(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void ActivePanel(GameObject panel)
    {
        animRightPanel.StartAnimation(panel, true);
        animLeftPanel.StartAnimation(panel, true);
    }
    public void ClosePanel(GameObject panel)
    {
        animRightPanel.StartAnimation(panel, false);
        animLeftPanel.StartAnimation(panel, false);
    }
    [Space(10)]
    public AnimationPAnel animRightPanel;
    public AnimationPAnel animLeftPanel;

    public void SetActivePanel(bool lose = false)
    {
        if (lose == false)
        {
            for (int i = 0; i < panelIsActive.Length; i++)
            {

                animRightPanel.StartAnimation(panelIsActive[i], true);
                animLeftPanel.StartAnimation(panelIsActive[i], true);
            }
        }
        else
        {
            for (int i = 0; i < panelIsActive.Length; i++)
            {
                animRightPanel.StartAnimationUnLockLevel(panelIsActive[i]);
                animLeftPanel.StartAnimationUnLockLevel(panelIsActive[i]);
            }
        }
    }
    public void SetDisActivePanel()
    {
        for (int i = 0; i < panelIsActive.Length; i++)
        {
            animRightPanel.StartAnimation(panelIsActive[i], false);
            animLeftPanel.StartAnimation(panelIsActive[i], false);
        }
    }
    public void SetActiveButtonStart()
    {
        animRightPanel.StartAnimation(buttonStart, true);
        animLeftPanel.StartAnimation(buttonStart, true);
    }
    public void SetDisActiveButtonStart()
    {
        _UIPanelFade.FadeOut();
        animRightPanel.StartAnimation(buttonStart, false);
        animLeftPanel.StartAnimation(buttonStart, false);
        buttonStart.SetActive(false);
        SetActivePanel(false);
    }

    public void SetDisActiveButtonStartPanelWin()
    {
        panelWin.FadeOut();
        buttonStart.SetActive(false);
        buttonStart.SetActive(false);
        for (int i = 0; i < panelIsActive.Length; i++)
        {
            panelIsActive[i].SetActive(true);
        }
    }

    public void SetValueForUpdate()
    {

        textBeforeUpdate.text = powerPlayer.ToString();

        levelPLayer++;
        int calculatedDamage = Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer - 1));
        levelPLayer--;
        int calculatedPrice = Convert.ToInt32(updateCost * Math.Pow(growthFactor, levelPLayer - 1));
        textAfterUpdate.text = $"{calculatedDamage}";
        textPriceOnButton.text = $"{calculatedPrice}";
    }

    public void UpdatePlayer()
    {
        if (countFirstUpdate <= GameManager.InstanceGame.gold)
        {
            countFirstUpdate = Convert.ToInt32(updateCost * Math.Pow(growthFactor, levelPLayer - 1));

            GameManager.InstanceGame.gold -= countFirstUpdate;
            powerPlayer = Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer));
            textAfterUpdate.text = $"{Convert.ToInt32(damage * Math.Pow(growthFactor, levelPLayer - 1))}";
            Debug.Log($"powerPlayer  {powerPlayer}");
            textPlayerDamage.text = powerPlayer.ToString();
            textPlayerDamageMainMenu.text = powerPlayer.ToString();

            levelPLayer++;
            SetValueForUpdate();

            characterLevel.text = levelPLayer.ToString();
            characterLevelMainMenu.text = levelPLayer.ToString();// изменение
            DataManager.InstanceData.SaveLevelPlayer();
            DataManager.InstanceData.SaveGold();
            DataManager.InstanceData.SavePowerPlayer();
        }
    }

    public void UplyChange(Sprite sprite)
    {
        characterMain.sprite = sprite;
        characterUpgrade.sprite = sprite;
    }
}