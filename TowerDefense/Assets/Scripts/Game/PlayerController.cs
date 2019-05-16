using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    public float moveSpeed, zoomSpeed;
    public UpgradePanel upgradePanel;
    Transform hitTerrain;
    private void Update()
    {
        CameraMove(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        CameraZoom();

        if (Input.GetMouseButtonDown(0))
            ShowUpgradePanel();

        //启用后一直跟随之前的刚才的地形,不受屏幕拖动影响
        if (upgradePanel.gameObject.activeInHierarchy)
            upgradePanel.transform.position = mainCamera.WorldToScreenPoint(hitTerrain.position + Vector3.up * 5);
    }
    public void CameraMove(float x, float z) //镜头在一定范围移动
    {
        Vector3 pos = mainCamera.transform.position;
        pos.x = Mathf.Clamp(pos.x, -100, 100);
        pos.z = Mathf.Clamp(pos.z, -240, -160);
        mainCamera.transform.position = pos;
        mainCamera.transform.Translate(x * moveSpeed * Time.deltaTime, 0, z * moveSpeed * Time.deltaTime, Space.World);
    }
    public void CameraZoom() //镜头在一定范围缩放
    {
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 5, 30);
        mainCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
    }
    //显示升级信息
    public void ShowUpgradePanel()
    {
        if (PagodaMenu.Instanc.readyToPlace == false)
        {
            if (upgradePanel.gameObject.activeInHierarchy && Vector3.Distance(upgradePanel.transform.position, Input.mousePosition) >= 300)
                CloseUpgradePanel();
            //从摄像机发射射线,检测防御塔地形
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, LayerMask.GetMask("Pagoda")))
            {
                //照射到的地形上有防御塔,显示升级信息               
                if (hit.collider.transform.childCount > 0)
                {
                    hitTerrain = hit.collider.transform;
                    //得到防御塔信息
                    Pagoda pagoda = hitTerrain.GetComponentInChildren<Pagoda>();
                    //显示升级信息
                    upgradePanel.ShowPagodaInfo(pagoda);
                }
            }
        }
        else
            upgradePanel.gameObject.SetActive(false);
    }
    //关闭升级信息和范围显示器
    public void CloseUpgradePanel()
    {
        upgradePanel.gameObject.SetActive(false);
        upgradePanel.attRange.gameObject.SetActive(false);
    }
}