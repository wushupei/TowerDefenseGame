using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillText : MonoBehaviour
{
    public float upSpeed; //上升速度
    Transform cam;

    private void Update()
    {
        transform.rotation = cam.rotation;
        transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
    }
    //播放击杀信息的动画
    public void PlayAnim(Transform _cam, Vector3 pos, Transform pool, string des, Color color)
    {
        cam = _cam;
        transform.SetParent(null);
        transform.position = pos;
        Util.Instance.AddTimeTask(() => transform.SetParent(pool), 1);
        TextMesh textmesh = GetComponent<TextMesh>();
        textmesh.text = des;
        textmesh.color = color;
    }
}
