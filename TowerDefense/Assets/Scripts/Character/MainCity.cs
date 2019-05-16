using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCity : Character
{
    Animation anim;
    void Start()
    {
        Init();
    }
    private void Update()
    {
        //血条始终面向镜头
        hpObj.rotation = mainCamera.rotation;
    }
    public override void Init()
    {
        base.Init();
        anim = GetComponent<Animation>();
    }
    public override void Death(Pagoda murderer) //重写死亡方法
    {
        base.Death(murderer);
        anim.enabled = false;
        GameMain.instance.GameOver();
    }
}
