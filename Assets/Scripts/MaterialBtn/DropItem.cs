using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DropItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public DragItem dragItem;
    public CollectBtnClick XYClick;
    //判断当前加工栏是否在使用中
    public bool inUse = false;
    //当前加工栏中加工的道具的数量
    public int posIndex = 0;

    public CollectBtnClick tempXYLCollect;
    public void OnDrop(PointerEventData eventData)
    {
        //如果当前加工栏被占用，不执行任何操作
        if (this.inUse == true) return;
        this.dragItem = GetDropSprite(eventData);
        //如果当前的数量<=0，返回，不执行任何操作
        if (this.dragItem.isEqualZero) return;
        //判断是否部件已经存在，如果部件的数量达到上限值，返回，不执行任何操作
        if (MatchingBarManager.Instance.partDictionary.ContainsKey(this.dragItem.gameObject.name))
        {
            GameObject temp;
            if (MatchingBarManager.Instance.partDictionary.TryGetValue(this.dragItem.gameObject.name, out temp))
            {
                if (Int32.Parse(temp.transform.GetChild(0).GetComponent<Text>().text) >= 80)
                {
                    Debug.Log("到达上限了，不要再加工了");
                    this.inUse = false;
                    return;
                }
            }
        }
        if (this.dragItem.collectBtnClick.material == Material.XY || this.dragItem.collectBtnClick.material == Material.XYL)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.inUse = true;
            Debug.Log("加工的是象牙，需要单独处理");
            return;
        }
        if (TextCountManager.Instance.CheckPartTextCount(this.dragItem.collectBtnClick))
        {
            return;
        }
        this.GetComponent<MatchingBar>().CreateMatchingMaterial(this.dragItem.collectBtnClick, this, this.dragItem);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetDropSprite(eventData) == null) return;
        if (this.inUse == true) return;
        this.dragItem = GetDropSprite(eventData);
       // this.GetComponent<Image>().color = new Color(1, 0, 0, 0.35f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        this.dragItem = GetDropSprite(eventData);
       // this.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.35f);
        //Debug.Log("Exit");
    }
    private DragItem GetDropSprite(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null) return null;
        var dragMe = originalObj.GetComponent<DragItem>();
        if (dragMe == null) return null;
        var srcImage = originalObj.GetComponent<Image>();
        if (srcImage == null) return null;
        return dragMe;
    }
    public void XYLMatchSwitch(string nam)
    {
        if (GameObject.Find("Canvas/BottomCollectBtns/CenterMaterialPart/" + "XY") != null)
        {
            this.dragItem = GameObject.Find("Canvas/BottomCollectBtns/CenterMaterialPart/" + "XY").GetComponent<DragItem>();
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        this.XYClick = this.dragItem.collectBtnClick;
        //XYL单独处理
        if (nam.Equals("XY"))
        {
            this.XYClick.material = Material.XY;
            this.XYClick.materialPanelIndex = 0;
            this.XYClick.factor = 4;
        }
        if (nam.Equals("XYL"))
        {
            this.XYClick.material = Material.XYL;
            this.XYClick.materialPanelIndex = 7;
            this.XYClick.factor = 40;
            if (MatchingBarManager.Instance.partDictionary.ContainsKey(Material.XYL.ToString()))
            {
                GameObject temp;
                if (MatchingBarManager.Instance.partDictionary.TryGetValue(Material.XYL.ToString(), out temp))
                {
                    if (Int32.Parse(temp.transform.GetChild(0).GetComponent<Text>().text) >= 80)
                    {
                        Debug.Log("到达上限了，不要再加工了");
                        this.inUse = false;
                        this.XYClick.materialPanelIndex = 0;
                        if (tempXYLCollect != null)
                        {
                            this.tempXYLCollect.materialPanelIndex = 0;

                        }
                        this.transform.GetChild(0).gameObject.SetActive(false);
                        return;
                    }
                }
            }
        }
        if (TextCountManager.Instance.CheckPartTextCount(XYClick))
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        this.GetComponent<MatchingBar>().CreateMatchingMaterial(XYClick, this, this.dragItem);
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
