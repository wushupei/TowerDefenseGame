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
    Animator anim;
    Rigidbody rigid;
    [HideInInspector]
    public EnemyState state;
    [HideInInspector]
    public Transform hitPos; //打击点

    Transform eye; //眼睛:用于观测道路和攻击目标
    List<Collider> ways; //记录走过的路

    public override void Init()
    {
        base.Init();

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        gameObject.layer = LayerMask.NameToLayer("Enemy"); //层设置为"Enemy"
        state = EnemyState.forward;
        hitPos = ToolsMethod.Instance.FindChildByName(transform, "HitPos");
        eye = transform.Find("Eye");
        ways = new List<Collider>();
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

        //斜下方打射线检测前方道路
        if (Physics.Raycast(eye.position, Quaternion.AngleAxis(30, transform.right) * transform.forward, out hit, 50, LayerMask.GetMask("Way")))
        {
            //Debug.DrawLine(eye.position, hit.point, Color.blue);
            //发现未走过的道路,获取该道路,朝向该路通往的方向
            if (!ways.Contains(hit.collider))
            {
                ways.Add(hit.collider);
                way = hit.transform;
                wayDir = Quaternion.LookRotation(way.forward);
            }
        }
        else //前方没路了发射球形射线检测周围是否有路
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10, LayerMask.GetMask("Way"));
            for (int i = 0; i < colliders.Length; i++)
            {
                //发现未走过的道路,获取该道路,朝向该路通往的方向
                if (!ways.Contains(colliders[i]))
                {
                    way = colliders[i].transform;
                    wayDir = Quaternion.LookRotation(way.forward);
                    break;
                }
            }
        }
        //获取与脚下道路x轴上偏差值,好让自身走在路中间
        float offset = 0;
        if (way != null)
        {
            Vector3 distance = transform.position - way.position;
            offset = Vector3.Dot(distance, way.right.normalized);
        }
        //面向该路指向的方向前进
        transform.rotation = Quaternion.RotateTowards(transform.rotation, wayDir, speed * 20 * Time.deltaTime);
        transform.Translate(-offset * Time.deltaTime, 0, speed * Time.deltaTime);
    }
    //被击飞方法
    bool isFly; //是否被击飞(处于击飞状态时不能再被击飞)
    public void StrikeFly(float force)
    {
        if (isFly == false) //未被击飞状态下才可以被击飞
        {
            isFly = true;
            rigid.AddForce(Vector3.up * force, ForceMode.Impulse);
            float initSpeed = speed; //初始速度
            speed = 0;
            //0.5秒后恢复
            Util.Instance.AddTimeTask(() =>
            {
                speed = initSpeed;
                isFly = false;
            }, 0.5f);
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
    public override void Death(Pagoda _murderer)
    {
        base.Death(_murderer);

        gameObject.layer = LayerMask.NameToLayer("DeadBody"); //层设置为"DeadBody"
        state = EnemyState.death;
        anim.Play("death");
        Util.Instance.AddTimeTask(DestroySelf, 5); //5秒后尸体消失

        //标记处决者
        _murderer.PlayKill();
    }
    //尸体消失
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    //void OnDrawGizmos() //设置辅助线
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 8);
    //}
}
