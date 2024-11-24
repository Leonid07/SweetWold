using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    public Button[] buttonBuy;
    void Start()
    {
        for (int i = 0; i < buttonBuy.Length; i++)
        {
            int count = i;
            buttonBuy[count].onClick.AddListener(() => BuySkin(count));
        }
    }

    public void BuySkin(int count)
    {
        if (DataManager.InstanceData.idCount[count] != "")
        {
            if (GameManager.InstanceGame.gold >= Convert.ToInt32(DataManager.InstanceData.idCount[count]))
            {
                GameManager.InstanceGame.gold -= Convert.ToInt32(DataManager.InstanceData.idCount[count]);
                PanelManager.InstancePanel.UplyChange(DataManager.InstanceData.spriteSkinHero[count]);
                DataManager.InstanceData.indexSpriteSkinHero = count;
                DataManager.InstanceData.idCount[count] = "";
                DataManager.InstanceData.textButtonShop[count].text = "";
                DataManager.InstanceData.SaveSkin();
                DataManager.InstanceData.SaveGold();
            }
        }
        else
        {
            PanelManager.InstancePanel.UplyChange(DataManager.InstanceData.spriteSkinHero[count]);
            DataManager.InstanceData.indexSpriteSkinHero = count;
            DataManager.InstanceData.SaveSkin();
        }
    }
}
