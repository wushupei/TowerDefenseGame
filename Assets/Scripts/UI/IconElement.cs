using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    string pagodaName; //防御塔名,用来加载防御塔 
    Camera mainCamera; //主摄像
    Transform attRange; //攻击范围显示器
    Material ria; //范围显示器的材质球
    LayerMask layer; //射线可照射层
    //初始化
    public void Init(Camera _mainCamera, Transform _attRange)
    {
        pagodaName = GetComponent<Image>().sprite.name; //自身图片的名字就是对应防御塔名字
        mainCamera = _mainCamera;
        attRange = _attRange;
        ria = attRange.GetComponent<MeshRenderer>().material;
        layer = LayerMask.GetMask("Ground") | LayerMask.GetMask("Way") | LayerMask.GetMask("Pagoda");
    }
    


    Pagoda pagodaObj; //防御塔实例
    //点击头像实例化防御塔
    public void OnPointerDown(PointerEventData eventData)
    {
        //加载防御塔模型
        pagodaObj = Instantiate(Resources.Load<Pagoda>("Prefab/Chara/PagodaChara/" + pagodaName));
        //启用攻击范围显示器并将防御塔攻击方位反映在尺寸上
        attRange.gameObject.SetActive(true);
        attRange.localScale = new Vector3(pagodaObj.attactRange * 2, 10, pagodaObj.attactRange * 2);
        GetComponent<Image>().color = new Color(0, 1, 0); //头像变色
    }
    bool isPlace; //是否可放置
    Transform terrain; //可放置地形
    //抬起鼠标放置或删除防御塔
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPlace) //可放置时
        {
            //放置在该地形并成为地形子物体,然后初始化
            pagodaObj.transform.position = terrain.position;
            pagodaObj.transform.SetParent(terrain); 
            pagodaObj.initPagoda();
        }
        else //不可放置则销毁
            Destroy(pagodaObj.gameObject);

        attRange.gameObject.SetActive(false); //禁用范围显示器
        pagodaObj = null;
        GetComponent<Image>().color = new Color(1, 1, 1); //头像变色
    }
    void Update()
    {
        //如果防御塔实例化,则找寻可以放置的位置
        if (pagodaObj != null)
        {
            //摄像机向鼠标位置发射线
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, layer))
            {
                pagodaObj.transform.position = hit.point; //防御塔模型根据鼠标移动
                attRange.position = hit.point; //范围显示器根据鼠标移动
                int index = hit.collider.gameObject.layer; //获取照射到物体的层
                //如果是可以放置的地形,并且该地形上没有其它防御塔,就可以放置
                if (LayerMask.LayerToName(index) == "Pagoda" && hit.collider.transform.childCount == 0)
                {
                    isPlace = true;
                    terrain = hit.collider.transform;
                    ria.color = new Color(0, 1, 0, 0.3f);
                }
                else
                {
                    isPlace = false;
                    ria.color = new Color(1, 0, 0, 0.3f);
                }
            }
        }
    }
}
