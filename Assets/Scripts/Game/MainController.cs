using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public Transform cam;
    Camera MainCamera;
    public float moveSpeed, zoomSpeed;
    public void Init()
    {
        MainCamera = cam.GetComponentInChildren<Camera>();
    }
    private void Update()
    {
        CameraMove(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        CameraZoom();
    }
    public void CameraMove(float x, float z) //镜头在一定范围移动
    {
        Vector3 pos = cam.position;
        pos.x = Mathf.Clamp(pos.x, -100, 100);
        pos.z = Mathf.Clamp(pos.z, -40, 40);
        cam.position = pos;
        cam.Translate(x * moveSpeed * Time.deltaTime, 0, z * moveSpeed * Time.deltaTime, Space.World);
    }
    public void CameraZoom() //镜头在一定范围缩放
    {
        MainCamera.fieldOfView = Mathf.Clamp(MainCamera.fieldOfView, 5, 30);
        MainCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
    }
}
