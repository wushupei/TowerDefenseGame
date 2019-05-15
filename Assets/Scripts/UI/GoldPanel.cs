using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPanel : MonoBehaviour
{
    public static GoldPanel Instance;
    public Text goldText;
    //初始化
    public void Init()
    {
        Instance = this;
        goldText.text = GameData.Instance.gold.ToString();
    }
    //将金币显示在界面上
}
