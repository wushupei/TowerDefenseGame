using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public static UpgradePanel Instanc;
    public Transform attRange; //攻击范围显示器
    [SerializeField]
    private GameObject[] stars;
    [SerializeField]
    private Button gradebut; //升级按钮
    [SerializeField]
    private Text sellGold; //出售价格
    [SerializeField]
    private Text upgradeGold; //出售价格
    Pagoda pagoda;
    //初始化
    public void Init()
    {
        Instanc = this;
        gameObject.SetActive(false);
    }
    //显示防御塔信息
    public void ShowPagodaInfo(Pagoda _pagoda)
    {
        gameObject.SetActive(true);
        pagoda = _pagoda;
        RefreshInfo();
        ShowattRange();
    }
    //根据等级刷新信息   
    public void RefreshInfo()
    {
        if (pagoda != null)
        {    //显示星星
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(i < pagoda.grade);
            }
            sellGold.text = pagoda.sellGold.ToString(); //显示出售数字
            upgradeGold.text = pagoda.upgradeGold.ToString(); //显示升级数字

            //如果满级或钱不够
            Text butDes = gradebut.GetComponentInChildren<Text>();
            if (pagoda.grade >= 3 || GameData.Instance.gold < pagoda.upgradeGold)
            {
                //按钮为红色,不能点击
                butDes.color = Color.red;
                gradebut.enabled = false;

                if (pagoda.grade >= 3) //如果是满级
                {
                    //数字隐藏
                    upgradeGold.enabled = false;
                }
                else //不是满级,那就是钱不够
                {
                    //数字显示,为红色
                    upgradeGold.enabled = true;
                    upgradeGold.color = Color.red;
                }
            }
            else //未满级且钱够
            {
                //数字显示,为黄色
                upgradeGold.enabled = true;
                upgradeGold.color = Color.yellow;
                //按钮为绿色,可以点击
                butDes.color = Color.green;
                gradebut.enabled = true;
            }
        }
    }
    //显示攻击范围
    void ShowattRange()
    {
        attRange.gameObject.SetActive(true);
        attRange.localScale = new Vector3(pagoda.attactRange * 2, 1, pagoda.attactRange * 2);
        attRange.position = pagoda.transform.position;
    }
    //出售防御塔(按钮事件)
    public void PagodaSell()
    {
        pagoda.SellPagoda();
        gameObject.SetActive(false);
        attRange.gameObject.SetActive(false);
        PagodaMenu.Instanc.RefreshMenu();
    }
    //升级防御塔(按钮事件)
    public void PagodaGrade()
    {
        pagoda.Upgrade(); //升级
        RefreshInfo();
        //升级攻击范围
        attRange.localScale = new Vector3(pagoda.attactRange * 2, 1, pagoda.attactRange * 2);
        PagodaMenu.Instanc.RefreshMenu();
    }
}
