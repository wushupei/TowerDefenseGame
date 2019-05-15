using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private static GameObjectPool _Instance;
    public static GameObjectPool Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new GameObjectPool();
            return _Instance;
        }
    }
    //用于保存所有对象池
    public Dictionary<string, Transform> poolDict;
    //初始化
    public void Init()
    {
        poolDict = new Dictionary<string, Transform>();
    }
    //获取对象池
    public Transform GetPool(string poolName)
    {
        if (poolDict.ContainsKey(poolName))
            return poolDict[poolName];
        //字典中没有重新创建
        Transform poolObj = new GameObject(poolName + "_Pool").transform;
        poolObj.SetParent(GameObject.Find("PoolManager").transform);
        poolObj.gameObject.SetActive(false);
        poolDict.Add(poolName, poolObj);
        return poolObj;
    }
}
