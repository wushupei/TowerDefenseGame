using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagodaMenu : MonoBehaviour
{
    public Camera mainCamera; //主摄像机
    public Transform attRange; //攻击范围显示器
    public void Init()
    {
        IconElement[] icons = GetComponentsInChildren<IconElement>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].Init(mainCamera, attRange);
        }
    }
}
