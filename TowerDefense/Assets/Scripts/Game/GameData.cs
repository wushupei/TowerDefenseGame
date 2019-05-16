using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private static GameData _Instance;
    public static GameData Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new GameData();
            return _Instance;
        }
    }
    //游戏金钱
    public int gold;
    public void AddGold(int _gold)
    {
        gold += _gold;
        GoldPanel.Instance.goldText.text = gold.ToString();
        UpgradePanel.Instanc.RefreshInfo();
    }
    public void SubGold(int _gold)
    {
        gold -= _gold;
        GoldPanel.Instance.goldText.text = gold.ToString();
    }
}
