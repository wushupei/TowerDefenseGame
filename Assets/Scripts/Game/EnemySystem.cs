using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    //根据名称保存所有敌人
    Dictionary<string, Enemy> enemyDict = new Dictionary<string, Enemy>();
    public void Init()
    {
        //保存所有种类敌人,可以根据名字获取
        Enemy[] enemys = Resources.LoadAll<Enemy>("Prefab/Chara/EnemyChara");
        for (int i = 0; i < enemys.Length; i++)
        {
            if (!enemyDict.ContainsKey(enemys[i].name))
                enemyDict.Add(enemys[i].name, enemys[i]);
        }
    }
    //生成敌人,参数中设置敌人种类,生成间隔,生成数量(默认为1)
    public void CreateEnemy(string name, float delay, int count = 1)
    {
        if (GameMain.instance.gameOver == false)
            //使用定时器,生成敌人
            Util.Instance.AddTimeTask(() => Instantiate(
            enemyDict[name], transform.position, transform.rotation).Init(),
            delay, count);
    }
    //点击按钮生成敌人(挂在按钮事件中)
    public void ClickButtonDispatchTroops()
    {
        CreateEnemy("Zombie1", 1, 10);
        Util.Instance.AddTimeTask(() => CreateEnemy("Zombie2", 1, 1), 1);
    }
}
