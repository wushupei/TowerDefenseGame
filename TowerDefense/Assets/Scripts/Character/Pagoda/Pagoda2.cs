using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagoda2 : Pagoda
{
    public float force; //击飞力度
    float hammerRange; //击飞范围
    ParticleSystem hammerEffect; //击飞特效
    Transform muzzle;//枪口
    Transform effectPool; //特效对象池
    public override void initPagoda()
    {
        base.initPagoda();
        hammerEffect = Resources.Load<ParticleSystem>("Prefab/Bullet/HammerEffect");
        effectPool = GameObjectPool.Instance.GetPool(hammerEffect.name);
        muzzle = ToolsMethod.Instance.FindChildByName(transform, "Muzzle");
    }
    //重写攻击方法(群体攻击)
    public override void PagodaAttack()
    {
        //作用范围始攻击范围的一半
        Collider[] enemys = Physics.OverlapSphere(muzzle.position, hammerRange, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < enemys.Length; i++)
        {
            Enemy enemy = enemys[i].GetComponent<Enemy>();
            enemy.Damage(damage, this);
            PlayKill(enemys[i].transform.position, (-damage).ToString(), Color.blue); 
            enemy.StrikeFly(force); //击飞
        }

        //展示击飞特效
        ParticleSystem effect = effectPool.GetComponentInChildren<ParticleSystem>();
        if (effect != null) //如果对象池中有从对象池中取
            effect.transform.SetParent(null);
        else //对象池中没有则实例化一个
            effect = Instantiate(hammerEffect);
        effect.transform.position = muzzle.position;
        effect.startSize = hammerRange * 5;
        effect.Play();
        Util.Instance.AddTimeTask(() => effect.transform.SetParent(effectPool), 1);
    }
    //根据等级更新数值
    public override void UpdateByGrade()
    {
        switch (grade)
        {
            case 0:
                Refresh(15, 15, 10);
                hammerRange = 6; //升级击飞范围
                break;
            case 1:
                Refresh(30, 22, 12);
                hammerRange = 8;
                break;
            case 2:
                Refresh(45, 37, 15);
                hammerRange = 10;
                break;
            case 3:
                Refresh(0, 60, 20);
                hammerRange = 12;
                break;
        }
    }
}
