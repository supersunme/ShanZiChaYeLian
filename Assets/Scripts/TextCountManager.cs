using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextCountManager : MonoBehaviour
{
    private static TextCountManager _instance;
    public static TextCountManager Instance
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
    //更新原料文本的数量,原料部分文本的更新
    public void UpdateTextCount(GameObject obj)
    {
        Text txt = obj.transform.GetChild(0).GetComponent<Text>();
        int currentCount = Int32.Parse(txt.text);
        int laterCount = currentCount - 1;
        txt.text = laterCount.ToString();
        if (laterCount <= 0)
        {
            txt.text = laterCount.ToString();
            txt.text = 0.ToString();
            StartCoroutine(Delay(obj));
          //  obj.gameObject.SetActive(false);
          //  Destroy(obj.gameObject, 0.2f);
        }
    }
    //更新部件文本的数量
    public void UpdatePartTextCount(GameObject obj, CollectBtnClick collectClick)
    {
        Text txt = obj.transform.GetChild(0).GetComponent<Text>();
        int currentCount = Int32.Parse(txt.text);
        if (currentCount >= 80)
        {
           // collectClick.materialPanelIndex = 0;
            return;
        }
        int laterCount = currentCount + collectClick.factor;
        txt.text = laterCount.ToString();
    }
    //更新采集按钮的文本数量
    public void UpdateCollectTextCount(GameObject obj, CollectBtnClick collectBtnClick)
    {
        Text txt = obj.transform.GetChild(0).GetComponent<Text>();
        int currentCount = Int32.Parse(txt.text);
        int laterCount = currentCount;
        collectBtnClick.currentCount = laterCount;
        collectBtnClick.tempCount = currentCount;
    }
    //判断部件栏是否达到最大值
    public bool CheckPartTextCount(CollectBtnClick collectBtnClick)
    {
        bool isOver = false;
        if (MatchingBarManager.Instance.partDictionary.ContainsKey(collectBtnClick.material.ToString()))
        {
            GameObject temp = null;
            MatchingBarManager.Instance.partDictionary.TryGetValue(collectBtnClick.material.ToString(), out temp);
            // TextCountManager.Instance.UpdatePartTextCount(temp, collectBtnClick);
            Text txt = temp.transform.GetChild(0).GetComponent<Text>();
            int count = Int32.Parse(txt.text);
            if (count >= 80)
            {

                isOver = true;
            }
        }
        return isOver;
    }
    //点击拼装的按钮之后，重新更新部件的文本数量
    public void UpdatePartTextCountAssemLater(OrderConfig orderConfig)
    {
        foreach (KeyValuePair<string, int> order in orderConfig.orderNumberDict)
        {
            //重点注意，Dictionary在迭代的过程中是一个只读的变量，不允许修改变量的值
            int orderInt = 0;
            GameObject temp = null;
            foreach (KeyValuePair<string, GameObject> p in MatchingBarManager.Instance.partDictionary)
            {
                GameObject obj = p.Value;
                if (p.Key.Equals(order.Key))
                {
                    orderInt = order.Value;
                    p.Value.transform.GetChild(0).GetComponent<Text>().text = (Int32.Parse(obj.transform.GetChild(0).GetComponent<Text>().text) - orderInt).ToString();
                }
            }
        }
    }

    IEnumerator Delay(GameObject obj)
    {
        yield return new WaitForSeconds(1);
        obj.SetActive(false);
    }
}
