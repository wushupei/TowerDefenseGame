using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagoda2 : Pagoda
{
    public float force; //击飞力度
    Transform muzzle;//枪口
    ParticleSystem hammerEffect; //击飞特效
    Transform effectPool; //特效对象池
    public override void initPagoda()
    {
        base.initPagoda();
        muzzle = ToolsMethod.Instance.FindChildByName(transform, "Muzzle");
        hammerEffect = Resources.Load<ParticleSystem>("Prefab/Bullet/HammerEffect");
        effectPool = GameObjectPool.Instance.GetPool(hammerEffect.name);
    }
    //重写攻击方法(群体攻击)
    public override void PagodaAttack()
    {
        //作用范围始攻击范围的一半
        Collider[] enemys = Physics.OverlapSphere(muzzle.position, attactRange / 2, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < enemys.Length; i++)
        {
            Enemy enemy = enemys[i].GetComponent<Enemy>();
            enemy.Damage(damage, this);
            enemy.StrikeFly(force); //击飞
        }

        //击飞特效
        if (effectPool.childCount > 0)
        {
            Transform effect = effectPool.GetChild(0);
            effect.SetParent(null);
            effect.position = muzzle.position;
            effect.GetComponent<ParticleSystem>().Play();
            Util.Instance.AddTimeTask(() => effect.SetParent(effectPool), 1);
        }
        else
        {
            ParticleSystem effect = Instantiate(hammerEffect, muzzle.position, Quaternion.identity);
            effect.Play();
            Util.Instance.AddTimeTask(() => effect.transform.SetParent(effectPool), 1);
        }
    }
}
