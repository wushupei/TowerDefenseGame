using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public static EnemySystem Instanc;
    //根据名称保存所有敌人
    Dictionary<string, Enemy> enemyDict = new Dictionary<string, Enemy>();
    public void Init()
    {
        Instanc = this;
        //保存所有种类敌人,可以根据名字获取
        Enemy[] enemys = Resources.LoadAll<Enemy>("Prefab/Chara/EnemyChara");
        for (int i = 0; i < enemys.Length; i++)
        {
            if (!enemyDict.ContainsKey(enemys[i].name))
            {
                enemyDict.Add(enemys[i].name, enemys[i]);
                enemys[i].enemyPool = GameObjectPool.Instance.GetPool(enemys[i].name);
            }
        }
    }
    //生成敌人,参数中设置敌人种类,生成间隔,生成数量(默认为1)
    public void CreateEnemy(string name, float delay, int count = 1)
    {
        if (GameMain.instance.gameOver == false)
        {

            //使用定时器,生成敌人
            Util.Instance.AddTimeTask(() =>
          {   //如果对象池没有则实例化
              Enemy enemy = enemyDict[name].enemyPool.GetComponentInChildren<Enemy>();
              if (enemy != null)
              {
                  enemy.transform.SetParent(null);
                  enemy.ResetEnemy(); //从对象池拿需要重置状态
              }
              else
              {
                  enemy = Instantiate(enemyDict[name]);
                  enemy.Init(); //实例化后要初始化
              }
              enemy.transform.position = transform.position;
              enemy.transform.rotation = transform.rotation;
          },
            delay, count);
        }
    }
    //点击按钮生成一组敌人(按钮事件)
    public void ClickButtonDispatchTroops()
    {
        CreateEnemy("Zombie1", 1, 10);
        //Util.Instance.AddTimeTask(() => CreateEnemy("Zombie2", 0.5f, 15), 0);
    }
}
