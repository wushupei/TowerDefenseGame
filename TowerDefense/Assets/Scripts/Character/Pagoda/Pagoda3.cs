using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pagoda3 : Pagoda
{
    float critChance; //暴击率
    //重写攻击方法
    public override void PagodaAttack()
    {
        if (target != null)
        {
            //代入暴击率,计算最终伤害(暴击是双倍伤害)
            int crit = (int)(critChance * 100);
            if (Random.Range(0, 100) < crit)
            {
                target.Damage(damage * 2, this);
                PlayKill(target.transform.position, (-damage * 2).ToString(), Color.red); //暴击提示
            }
            else
            {
                target.Damage(damage, this);
                PlayKill(target.transform.position, (-damage).ToString(), Color.blue);
            }
        }
    }
    //根据等级更新数值
    public override void UpdateByGrade()
    {
        switch (grade)
        {
            case 0:
                Refresh(20, 20, 50);
                critChance = 0.2f; //升级暴击率
                break;
            case 1:
                Refresh(40, 30, 65);
                critChance = 0.3f;
                break;
            case 2:
                Refresh(60, 50, 80);
                critChance = 0.4f;
                break;
            case 3:
                Refresh(0, 80, 100);
                critChance = 0.5f;
                break;
        }
    }
}
