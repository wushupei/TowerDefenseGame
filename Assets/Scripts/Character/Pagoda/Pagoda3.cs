using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagoda3 : Pagoda
{
    //重写攻击方法
    public override void PagodaAttack()
    {
        if (target != null)
            target.Damage(damage,this);
    }
}
