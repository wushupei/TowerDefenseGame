using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 主调函数
/// </summary>
public class GameMain : MonoBehaviour
{
    public static GameMain instance;

    [SerializeField]
    private MainController controller;
    [SerializeField]
    private EnemySystem eSys;
    [SerializeField]
    private PagodaMenu pagodaMenu;
    [SerializeField]
    private GoldPanel mGoldPanel;
    [HideInInspector]
    public bool gameOver;
    void Start()
    {
        InitGame();
    }
    //初始化游戏
    void InitGame()
    {
        InitGameData();
        instance = this; //单例
        gameOver = false;
        controller.Init();
        eSys.Init();
        pagodaMenu.Init();
        mGoldPanel.Init();
    }
    //初始化游戏数据
    void InitGameData()
    {
        GameData.Instance.gold = 200;
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //        DispatchTroops();
    //}
}
