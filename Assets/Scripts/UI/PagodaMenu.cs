using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagodaMenu : MonoBehaviour
{
    public static PagodaMenu Instanc;
    IconElement[] icons;
    public Camera mainCamera; //主摄像机
    public Transform attRange; //攻击范围显示器
    public bool readyToPlace; //准备放置防御塔
    //初始化
    public void Init()
    {
        Instanc = this;
        icons = GetComponentsInChildren<IconElement>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].Init(mainCamera, attRange);
        }
        readyToPlace = false;
        RefreshMenu();
    }
    //刷新菜单,发现金钱不够的就隐藏该菜单选项
    public void RefreshMenu()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].HideSwitch(GameData.Instance.gold >= icons[i].gold);
        }
    }
}
