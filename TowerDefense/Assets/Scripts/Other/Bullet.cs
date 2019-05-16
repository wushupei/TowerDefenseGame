using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    Enemy target; //攻击目标
    float damage; //伤害值
    Transform pool; //对象池
    Vector3 initPos; //初始位置
    Pagoda pagoda; //防御塔信息

    //初始化
    public void InitBullet(Vector3 position, Quaternion rotation, Enemy _target, float _damage, Transform _pool, Pagoda _pagoda)
    {
        transform.SetParent(null);
        transform.position = position;
        transform.rotation = rotation;
        target = _target;
        damage = _damage;
        pool = _pool;
        initPos = transform.position;
        pagoda = _pagoda;
    }
    private void Update()
    {
        if (transform.parent == null) //没有射中目标,继续飞
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
            if (Vector3.Distance(initPos, transform.position) > 500) //飞出一定范围自动销毁
                DestroySelf();

            if (target != null && target.state != EnemyState.death) //如果目标活着朝向目标
            {
                transform.LookAt(target.hitPos);
                //到达有效范围,调用目标受伤方法,成为目标子物体(插在目标身上)
                if (Vector3.Distance(target.hitPos.position, transform.position) <= 1)
                {
                    target.Damage(damage, pagoda); //拿取防御塔信息
                    pagoda.PlayKill(target.transform.position, (-damage).ToString(), Color.blue);
                    transform.SetParent(target.hitPos);
                }
            }
        }
        else if (target.state == EnemyState.death) //射中后,只要目标一死就销毁
            DestroySelf();
    }
    //销毁自身(进入对象池)
    private void DestroySelf()
    {
        transform.SetParent(pool);
    }
}
