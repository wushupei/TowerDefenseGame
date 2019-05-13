using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPanel : MonoBehaviour
{
    public static GoldPanel Instance;
    [SerializeField]
    private Text mGold;
    //初始化
    public void Init()
    {
        Instance = this;
        ShowGold();
    }
    //将金币显示在界面上
    public void ShowGold()
    {
        mGold.text = GameData.Instance.gold.ToString();
    }
}
