using System;
using UnityEngine;
public class MatchingBar : MonoBehaviour
{
    //创建和生成加工对象
 //   public Action<CollectBtnClick, GameObject> MatchCompleteAction;
    public GameObject currentObj;
    private CollectBtnClick collectBtnClick;
    private DropItem dropItem;
    public GameObject CreateMatchingMaterial(CollectBtnClick collectBtnClick, DropItem dropItem,DragItem dragitem)
    {
        GameObject objResource = Resources.Load("matching/" + collectBtnClick.material.ToString()) as GameObject;
        if (objResource == null) return null;
        GameObject processBarObj = Instantiate(objResource, Vector3.zero, Quaternion.identity) as GameObject;
        // processBarObj.name = collectBtnClick.material.ToString();
        RectTransform rectans = processBarObj.GetComponent<RectTransform>();
        rectans.SetParent(this.GetComponent<RectTransform>());
        rectans.SetAsLastSibling();
        rectans.localScale = Vector3.one;
        rectans.anchoredPosition = new Vector2(0, 0);
       // Destroy(processBarObj.gameObject, 1);
        //当动画播放完成，销毁刚才生成的对象
      //  dropItem.inUse = false;
       // 初始化数据
       // collectBtnClick.materialPanelIndex = 0;

        this.collectBtnClick = collectBtnClick;
        TextCountManager.Instance.UpdateTextCount(dragitem.gameObject);
        TextCountManager.Instance.UpdateCollectTextCount(dragitem.gameObject, dragitem.collectBtnClick);
        //数据配置,当拖放成功时进行数据配置
        dropItem.inUse = true;
        this.dropItem = dropItem;
      //  CreateParts(collectBtnClick);
        return objResource;
    }
    //生成加工部件,加工栏顶部的对象,部件栏
    public GameObject CreateParts(CollectBtnClick collectBtnClick)
    {
        //+ "(Clone)"
        if (MatchingBarManager.Instance.partDictionary.ContainsKey(collectBtnClick.material.ToString()))
        {
            GameObject temp = null;
            MatchingBarManager.Instance.partDictionary.TryGetValue(collectBtnClick.material.ToString(), out temp);
            TextCountManager.Instance.UpdatePartTextCount(temp, collectBtnClick);
          //  Debug.Log("有了");
            //这里开始对比订单和原料的要求
            OrderManager.Instance.CompareOrder();
            return null;
        }
        GameObject objResouce = null;
        objResouce = Resources.Load("parts/" + collectBtnClick.material.ToString()) as GameObject;
        if (objResouce == null) return null;
        Vector2 pos = new Vector2(collectBtnClick.materialPanelIndex * 121 + -396, 233);

        GameObject processBarObj = Instantiate(objResouce) as GameObject;
        //设置当前部件的名字
        processBarObj.name = collectBtnClick.material.ToString();
        RectTransform rectans = processBarObj.GetComponent<RectTransform>();
        RectTransform parentTrans = GameObject.Find("Canvas/TopPart").GetComponent<RectTransform>();
        rectans.SetParent(parentTrans);
        rectans.SetAsLastSibling();
        rectans.anchoredPosition = pos;
        rectans.localScale = Vector3.one;
        //Resources.UnloadAsset(objResouce);
        if (collectBtnClick.material == Material.XYL) collectBtnClick.materialPanelIndex = 0;
        TextCountManager.Instance.UpdatePartTextCount(processBarObj, collectBtnClick);
        MatchingBarManager.Instance.partDictionary.Add(processBarObj.gameObject.name, processBarObj);
        //从这里开始对比原料和订单的要求
        OrderManager.Instance.CompareOrder();
        return objResouce;
    }
    public void AnimationComplete(GameObject obj)
    {
        Debug.Log("加工动画播放完成");
       // if (this)
        this.dropItem.inUse = false;
        Destroy(obj,1);
        CreateParts(this.collectBtnClick);
        
    }
}
