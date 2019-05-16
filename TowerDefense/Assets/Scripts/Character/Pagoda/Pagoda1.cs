using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagoda1 : Pagoda
{
    Transform muzzle;//枪口
    Bullet Arrow; //箭矢
    Transform arrowPool; //箭矢对象池
    //重写初始化
    public override void initPagoda()
    {
        base.initPagoda();
        muzzle = ToolsMethod.Instance.FindChildByName(transform, "Muzzle");
        Arrow = Resources.Load<Bullet>("Prefab/Bullet/Arrow");
        arrowPool = GameObjectPool.Instance.GetPool(Arrow.name);
    }
    //重写攻击方法
    public override void PagodaAttack()
    {
        //如果对象池有,则从对象池取子弹,否则重新实例化
        //设定位置,方向,攻击目标,伤害值,所在对象池
        if (arrowPool.childCount > 0)
            arrowPool.GetChild(0).GetComponent<Bullet>().InitBullet(
                muzzle.position, muzzle.rotation, target, damage, arrowPool, this);
        else
            Instantiate(Arrow).InitBullet(
                muzzle.position, muzzle.rotation, target, damage, arrowPool, this);
    }
    //根据等级更新数值
    public override void UpdateByGrade()
    {
        switch (grade)
        {
            case 0:
                Refresh(10, 10, 30);
                attactRange = 30; //升级攻击范围
                break;
            case 1:
                Refresh(20, 15, 40);
                attactRange = 35;
                break;
            case 2:
                Refresh(30, 25, 50);
                attactRange = 40;
                break;
            case 3:
                Refresh(0, 40, 60);
                attactRange = 45;
                break;
        }
    }
}
