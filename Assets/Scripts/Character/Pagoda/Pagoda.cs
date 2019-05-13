using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagoda : MonoBehaviour
{
    Animator anim;
    KillText killText; //击杀信息
    Transform killPool; //击杀信息对象池
    Transform cam; //主摄像机
    //初始化
    public virtual void initPagoda()
    {
        enabled = true;
        anim = GetComponentInChildren<Animator>();
        killText = Resources.Load<KillText>("Prefab/UI/KillText");
        killPool = GameObjectPool.Instance.GetPool(killText.name);
        cam = GameObject.Find("Main Camera").transform;
    }
    public Transform k;
    private void Update()
    {
        if (GameMain.instance.gameOver)
        {
            anim.SetBool("Attack", false);
            return;
        }
        GetTarget();
    }
    public float attactRange; //攻击范围
    public float damage; //伤害
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

    //抢人头标记
    public void PlayKill()
    {
        if (killPool.childCount > 0)
            killPool.GetChild(0).GetComponent<KillText>().PlayAnim(cam, transform.position + Vector3.up * 10, killPool);
        else
            Instantiate(killText).PlayAnim(cam, transform.position + Vector3.up * 10, killPool);
    }
}
