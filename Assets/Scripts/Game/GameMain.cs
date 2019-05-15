using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 主调函数
/// </summary>
public class GameMain : MonoBehaviour
{
    public static GameMain instance;

    [SerializeField]
    private PlayerController controller;
    [SerializeField]
    private EnemySystem eSys;
    [SerializeField]
    private PagodaMenu pagodaMenu;
    [SerializeField]
    private GoldPanel mGoldPanel;
    [SerializeField]
    private Button mAgainBut; //重新开始按钮
    [SerializeField]
    private UpgradePanel mUpgradePanel;
    [HideInInspector]
    public bool gameOver;
    public Texture2D cursorTexture;  //鼠标图片
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
        GameObjectPool.Instance.Init();
        eSys.Init();
        pagodaMenu.Init();
        mGoldPanel.Init();
        mUpgradePanel.Init();
        mAgainBut.gameObject.SetActive(false);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto); //设置鼠标图片
    }

    public int InitGold;
    //初始化游戏数据
    void InitGameData()
    {
        GameData.Instance.gold = InitGold;
    }
    //游戏结束
    public void GameOver()
    {
        gameOver = true;
        mAgainBut.gameObject.SetActive(true);
    }

    //重新开始游戏(挂按钮事件中)
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
