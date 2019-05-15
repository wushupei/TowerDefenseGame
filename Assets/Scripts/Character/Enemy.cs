using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState //状态机
{
    forward,
    attack,
    death
}
public class Enemy : Character
{
    [HideInInspector]
    public Transform enemyPool; //对象池
    Animator anim;
    Rigidbody rigid;
    [HideInInspector]
    public Transform hitPos; //打击点
    Transform eye; //眼睛:用于观测道路和攻击目标
    [HideInInspector]
    public EnemyState state;
    List<Collider> ways; //记录走过的路

    public int gold;

    public override void Init()
    {
        base.Init();

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        hitPos = ToolsMethod.Instance.FindChildByName(transform, "HitPos");
        eye = transform.Find("Eye");
        ResetEnemy();
    }
    //重置敌人状态
    public void ResetEnemy()
    {
        surHp = totalHp; //血量恢复
        redHp.localScale = Vector3.one;
        gameObject.layer = LayerMask.NameToLayer("Enemy"); //层设置为"Enemy"
        state = EnemyState.forward;
        ways = new List<Collider>(); //走过的路重置
    }
    private void Update()
    {
        hpObj.rotation = mainCamera.rotation; //血条始终面向镜头

        if (GameMain.instance.gameOver)
            anim.Play("idle");
        else if (state == EnemyState.forward)
            EnemyForward();
    }
    //前进方法
    public int view; //视野
    Quaternion wayDir;
    Transform way;
    public float speed;
    MainCity target;
    bool isFly; //是否被击飞(击飞状态时不能再被击飞)
    bool isGround; //是否落地
    private void EnemyForward()
    {
        RaycastHit hit;
        //看见攻击目标则攻击
        if (Physics.Raycast(eye.position, transform.forward, out hit, view, LayerMask.GetMask("City")))
        {
            state = EnemyState.attack;
            anim.Play("attack");
            target = hit.collider.GetComponent<MainCity>();
        }
        if (Physics.Raycast(eye.position, Vector3.down, out hit, 4.1f, LayerMask.GetMask("Way")))
        {
            way = hit.transform;
            wayDir = Quaternion.LookRotation(way.forward);

            //获取与道路的偏移量
            Vector3 distance = transform.position - way.position;
            float offset = Vector3.Dot(distance, way.right.normalized);

            //往道路方向旋转和移动
            transform.rotation = Quaternion.RotateTowards(transform.rotation, wayDir, speed * 20 * Time.deltaTime);
            transform.Translate(-offset * Time.deltaTime, 0, speed * Time.deltaTime);
            isGround = true;
        }
        else
            isGround = false;
    }
    //被击飞方法(不可叠加)
    public void StrikeFly(float force)
    {
        //未被击飞状态下才可以被击飞,且在地上,才能被击飞
        if (isFly == false && isGround == true)
        {
            isFly = true;
            rigid.AddForce(Vector3.up * force, ForceMode.Impulse);
            Util.Instance.AddTimeTask(() => isFly = false, 0.3f);
        }
    }
    //攻击方法(放在攻击动画事件中)
    public float damage; //伤害
    private void EnemyAttack()
    {
        if (target != null)
            target.Damage(damage, null);
    }

    //死亡方法
    public override void Death(Pagoda murderer)
    {
        base.Death(murderer);

        gameObject.layer = LayerMask.NameToLayer("DeadBody"); //层设置为"DeadBody"
        state = EnemyState.death;
        anim.Play("death");
        Util.Instance.AddTimeTask(() => transform.SetParent(enemyPool), 5); //5秒后返回对象池
        GameData.Instance.AddGold(gold); //获得金币奖励
        PagodaMenu.Instanc.RefreshMenu(); //刷新菜单

        //标记处决者
        murderer.PlayKill(murderer.transform.position, "击杀", Color.red);
    }
    //void OnDrawGizmos() //设置辅助线
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 8);
    //}
}
