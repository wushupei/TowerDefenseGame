using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pagoda : MonoBehaviour
{
    Animator anim;
    KillText killText; //击杀信息
    protected Transform killPool; //击杀信息对象池
    Transform cam; //主摄像机
    [HideInInspector]
    public Transform pagodaPool; //对象池


    public int grade; //当前等级
    public int gold; //修建价格
    [HideInInspector]
    public int upgradeGold; //升级价格
    [HideInInspector]
    public int sellGold; //出售价格
    //初始化
    public virtual void initPagoda()
    {
        enabled = true;
        anim = GetComponentInChildren<Animator>();
        killText = Resources.Load<KillText>("Prefab/UI/KillText");
        killPool = GameObjectPool.Instance.GetPool(killText.name);
        cam = GameObject.Find("Main Camera").transform;
        UpdateByGrade();
    }
    private void Update()
    {
        if (GameMain.instance.gameOver)
        {
            anim.SetBool("Attack", false);
            return;
        }
        GetTarget();
    }
    public float damage; //伤害
    public float attactRange; //攻击范围
    protected Enemy target; //攻击目标
    //获取攻击目标
    void GetTarget()
    {
        if (target == null) //攻击目标为空时,用球形射线找寻攻击目标
        {
            Collider[] enemys = Physics.OverlapSphere(transform.position, attactRange, LayerMask.GetMask("Enemy"));
            if (enemys.Length == 0)
                anim.SetBool("Attack", false);
            //发现敌人,设为目标,进行攻击(播放攻击动画)
            for (int i = 0; i < enemys.Length;)
            {
                target = enemys[i].GetComponent<Enemy>();
                anim.SetBool("Attack", true);
                break;
            }
        }
        else
        {
            //面向攻击目标
            Vector3 pos = target.transform.position;
            Quaternion dir = Quaternion.LookRotation(new Vector3(pos.x, transform.position.y, pos.z) - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, dir, 0.1f);
            //攻击目标离开攻击范围或死亡,重新获取攻击目标
            if (Vector3.Distance(target.transform.position, transform.position) >= attactRange || target.state == EnemyState.death)
                target = null;
        }
    }
    //攻击方法(放在攻击动画事件中)
    public virtual void PagodaAttack()
    {
    }
    //出售防御塔
    public void SellPagoda()
    {
        GameData.Instance.AddGold(sellGold);
        //返回对象池,等级清0,刷新数据
        transform.SetParent(pagodaPool);        
        grade = 0;
        UpdateByGrade();
    }
    //升级方法
    public void Upgrade()
    {
        //未满级且金钱足够才能升级
        if (grade < 3 && GameData.Instance.gold >= upgradeGold)
        {
            GameData.Instance.SubGold(upgradeGold);
            grade++;
            UpdateByGrade(); //升级后更新数值
        }
    }
    //根据等级更新数值
    public virtual void UpdateByGrade()
    {
    }
    //升级属性
    public void Refresh(int _upgradeGold, int _sellGold, float _damage)
    {
        upgradeGold = _upgradeGold; //升级升级价格
        sellGold = _sellGold; //升级出售价格        
        damage = _damage; //升级攻击力
    }
    //抢人头标记
    public void PlayKill(Vector3 pos, string des, Color color)
    {
        if (killPool.childCount > 0)
            killPool.GetChild(0).GetComponent<KillText>().PlayAnim(cam, pos + Vector3.up * 10, killPool, des, color);
        else
            Instantiate(killText).PlayAnim(cam, pos + Vector3.up * 10, killPool, des, color);
    }
}
