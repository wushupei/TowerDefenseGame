using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;
    public EnemyState state;
    Transform eye; //眼睛:用于观测道路和攻击目标
    List<Collider> ways; //记录走过的路(不走回头路)


    public int view; //视野
    Quaternion wayDir; //前进方向
    MainCity target; //主城
    Transform way; //正在走的路
    public float speed; 
    //前进方法
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

        //斜下方30°打射线检测前方道路
        if (Physics.Raycast(eye.position, Quaternion.AngleAxis(30, transform.right)
            * transform.forward, out hit, 50, LayerMask.GetMask("Way")))
        {
            Debug.DrawLine(eye.position, hit.point, Color.blue);
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
            Collider[] colliders = Physics.OverlapSphere(transform.position, 8, LayerMask.GetMask("Way"));
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
}
