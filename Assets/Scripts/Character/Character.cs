using UnityEngine;

public class Character : MonoBehaviour
{
    public float totalHp = 100; //总血量
    float surHp; //剩余血量
    protected Transform hpObj; //黄血条
    protected Transform redHp; //血条红条
    protected Transform mainCamera; //主摄像机

    public virtual void Init() //初始化
    {
        surHp = totalHp;
        hpObj = transform.Find("BloodStrip");
        redHp = hpObj.Find("Hp");
        mainCamera = GameObject.Find("Main Camera").transform;
    }
    public void Damage(float damage,Pagoda murderer) //受伤方法,参数为受到的伤害值和给予最后一击的防御塔
    {
        if (surHp > damage) //当前血量大于受伤血量,正常扣血
        {
            surHp -= damage;
            //受伤后开始显示血条
            if (surHp < totalHp)
                hpObj.gameObject.SetActive(true);
            Vector3 hpScale = redHp.localScale;
            hpScale.x = surHp / totalHp;
            redHp.localScale = hpScale;
        }
        else //当前血量不够,调用死亡方法          
            Death(murderer);
    }
    public virtual void Death(Pagoda murderer) //死亡方法
    {
        surHp = 0;
        hpObj.gameObject.SetActive(false); //血条不再显示
    }
}
