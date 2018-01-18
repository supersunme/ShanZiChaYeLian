using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StudyDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public DragItem dragItem;
    public StudyCollectBtn collectBtnClick;
    //判断当前加工栏是否在使用中
    public bool inUse = false;
    //当前加工栏中加工的道具的数量
    public int posIndex = 0;

    public Transform parttans;

    public GameObject currentPart;

    public GameObject assemImage;

    public GameObject okImage;

    public GameObject dragObjStudy;

    public DialogManager dialogmanager;

    public GameObject nextButton;

    public GameObject orderObj;

    void Start()
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        this.dragItem = GetDropSprite(eventData);
        if (this.inUse) return;
        //Debug.Log(this.dragItem.name);
        if (this.dragItem != null)
        {
            if (Int32.Parse(this.dragItem.gameObject.transform.GetChild(0).GetComponent<Text>().text.ToString()) == 0)
            {
                return;
            }
        }
        if (dragObjStudy.activeSelf == true)
        {

            dragObjStudy.SetActive(false);

        }

        CreateMatchingMaterial(collectBtnClick, this, this.dragItem);

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
    }
    public void OnPointerExit(PointerEventData eventData)
    {
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
    public GameObject CreateMatchingMaterial(StudyCollectBtn collectBtnClick, StudyDrop dropItem, DragItem dragitem)
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

        this.inUse = true;
        this.collectBtnClick = collectBtnClick;
        return objResource;
    }

    public GameObject haloFlashImage;
    public GameObject CreateParts(StudyCollectBtn collectBtnClick)
    {
        if (this.parttans.childCount != 0)
        {
            Debug.Log("到这里了");
            this.parttans.GetChild(0).GetChild(0).GetComponent<Text>().text =
                (Int32.Parse(this.parttans.GetChild(0).GetChild(0).GetComponent<Text>().text) + 4).ToString();
            if (Int32.Parse(this.parttans.GetChild(0).GetChild(0).GetComponent<Text>().text) == 20)
            {
                this.dialogmanager.ClickNextDialog();
                this.assemImage.gameObject.SetActive(true);
                this.okImage.SetActive(true);
                haloFlashImage.SetActive(true);
            }
            return null;
        }
        #region MyRegion

        /* if (this.currentPart != null)
        {
            currentPart.transform.GetChild(0).GetComponent<Text>().text =
                (Int32.Parse(currentPart.transform.GetChild(0).GetComponent<Text>().text) + 4).ToString();


            if (Int32.Parse(currentPart.transform.GetChild(0).GetComponent<Text>().text) == 5)
            {
               // this.btnhaloANI.SetActive(false);
                this.dialogmanager.ClickNextDialog();
                this.assemImage.gameObject.SetActive(true);
                this.okImage.SetActive(true);
                haloFlashImage.SetActive(true);
            }
            return null;
        }*/

        #endregion
        GameObject objResouce = null;
        objResouce = Resources.Load("parts/" + collectBtnClick.material.ToString()) as GameObject;
        if (objResouce == null) return null;
        // Vector2 pos = new Vector2(collectBtnClick.materialPanelIndex * 121 + -396, 233);

        GameObject processBarObj = Instantiate(objResouce) as GameObject;
        //设置当前部件的名字
        processBarObj.name = collectBtnClick.material.ToString();
        RectTransform rectans = processBarObj.GetComponent<RectTransform>();
        rectans.transform.SetParent(parttans);
        rectans.SetAsLastSibling();
        // rectans.anchoredPosition = pos;
        rectans.anchoredPosition = new Vector2(-395, 234);
        rectans.localScale = Vector3.one;

        this.currentPart = processBarObj;
        if (collectBtnClick.material == Material.XYL) collectBtnClick.materialPanelIndex = 0;
        if (this.currentPart != null)
        {
            this.currentPart.transform.GetChild(0).GetComponent<Text>().text =
                (Int32.Parse(this.currentPart.transform.GetChild(0).GetComponent<Text>().text) + 4).ToString();
        }
        return objResouce;
    }
    public void AnimationComplete(GameObject obj)
    {
        //Debug.Log("加工动画播放完成");
        this.inUse = false;
        if (this.dragItem != null)
        {
            if ((Int32.Parse(this.dragItem.transform.GetChild(0).GetComponent<Text>().text) == 0))
            {
                this.dragItem.gameObject.SetActive(false);
                return;
            }
            this.dragItem.transform.GetChild(0).GetComponent<Text>().text =
               (Int32.Parse(this.dragItem.transform.GetChild(0).GetComponent<Text>().text) - 1).ToString();
        }
        CreateParts(this.collectBtnClick);
        if (obj != null)
        {
            obj.SetActive(false);
            Destroy(obj, 1);
        }
    }

    public GameObject orderComplete;

    public GameObject blackMask;
    public void OrderComplete()
    {
        this.currentPart.SetActive(false);
        this.orderComplete.gameObject.SetActive(true);
        this.dialogmanager.ClickNextDialog();
        this.nextButton.gameObject.SetActive(true);
        this.orderObj.SetActive(false);
        this.blackMask.gameObject.SetActive(true);
        this.blackMask.transform.SetSiblingIndex(this.blackMask.transform.GetSiblingIndex() + 6);
    }
}
