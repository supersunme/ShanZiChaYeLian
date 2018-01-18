using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// 订单拼装类
/// </summary>
public class AssemOrder : MonoBehaviour
{
    private static AssemOrder _instance;
    public static AssemOrder Instance
    {
        get
        {
            return _instance;
        }
    }
    void Start()
    {
        _instance = this;
    }
    //如果有订单满足条件,改变玩家的得分，并销毁当前订单，还需要移除原料中消耗的数量，如果数量小于等于0，则销毁该对象，并从字典中移除相应的键值
    public void StartAssemOrder(GameObject orderObject)
    {
        if (orderObject != null)
        {
            if (orderObject.transform.GetChild(0) != null)
            {
                orderObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (orderObject.transform.GetChild(1) != null)
            {
                orderObject.transform.GetChild(1).gameObject.SetActive(true);
                Debug.Log("当前订单满足需求，可以拼装");
            }
            if (orderObject.transform.GetChild(2) != null)
            {
                orderObject.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
    //原料种类或者数量不满足订单的条件，将相应的条件去除
    public void EndAssemOrder(GameObject orderObject)
    {
        if (orderObject != null)
        {
            if (orderObject.transform.GetChild(0) != null)
            {
                orderObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (orderObject.transform.GetChild(1) != null)
            {
                orderObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            if (orderObject.transform.GetChild(2) != null)
            {
                orderObject.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }
}
