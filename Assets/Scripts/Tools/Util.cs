using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    private static Util _Instance = null;
    public static Util Instance //单例模式,依附gameObject
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject("Util");
                _Instance = obj.AddComponent<Util>();
            }
            return _Instance;
        }
    }
    public class TimeTask //定时事件类
    {
        public Action callback; //回调函数
        public float delayTime; //延迟长度
        public float destTime; //延迟后的目标时间
        public int count; //重复次数
    }                
    List<TimeTask> timeTaskList = new List<TimeTask>(); //保存所有的定时事件   
    
    //增加定时回调的方法 
    public void AddTimeTask(Action _callback, float _delayTime, int _count = 1)     
    {
        timeTaskList.Add(new TimeTask()
        {
            callback = _callback,
            delayTime = _delayTime,
            destTime = Time.realtimeSinceStartup + _delayTime,
            count = _count
        });
    }
    private void Update()
    {
        for (int i = 0; i < timeTaskList.Count; i++) //实时监测所有定时事件
        {
            TimeTask task = timeTaskList[i];
            if (Time.realtimeSinceStartup >= task.destTime) //时间到了,则执行
            {
                task.callback?.Invoke(); 
                if (task.count == 1) //当次数为1,执行完移除该定时事件
                    timeTaskList.RemoveAt(i);
                else if (task.count > 1) //当次数大于1,执行完次数减1
                    task.count--;
                task.destTime += task.delayTime; //执行完一次后,重新定出下次执行时间
            }
        }
    }
}
