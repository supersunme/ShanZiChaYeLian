
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool dragOnSurface = true;
    private Transform canvasTrans;
    public Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();
    public Vector3 scale = Vector3.one;
    public CollectBtnClick collectBtnClick;
    public string collectBtnClickName;
    //public bool isCominDropItem = false;
    public bool isEqualZero = false;

    public GameObject icon;
    void Start()
    {
        if (GameObject.Find("Canvas").transform != null)
        {
            GameObject temp = GameObject.Find("Canvas");
            this.canvasTrans = temp.transform;
        }
        this.canvasTrans = GameObject.Find("Canvas").transform;
        if (GameObject.Find("Canvas/BottomCollectBtns/BottomBtns/" + this.collectBtnClickName)!=null)
        this.collectBtnClick =
            GameObject.Find("Canvas/BottomCollectBtns/BottomBtns/" + this.collectBtnClickName).GetComponent<CollectBtnClick>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //如果当前拖动的原料的数量等于0
        if (Int32.Parse(this.transform.GetChild(0).GetComponent<Text>().text) <= 0)
        {
            this.isEqualZero = true;
            return;
        }
        #region MyRegion
        /*
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;
        m_DraggingIcons[eventData.pointerId] = new GameObject("icon");

        m_DraggingIcons[eventData.pointerId].transform.SetParent(canvasTrans, false);
        m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();

        var image = m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
        var group = m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        //group.ignoreParentGroups = true;

        image.sprite = GetComponent<Image>().sprite;
        image.SetNativeSize();
        image.GetComponent<RectTransform>().localScale = scale;
        if (dragOnSurface)
        {
            m_DraggingPlanes[eventData.pointerId] = transform as RectTransform;
        }
        else
        {
            m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        }*/
        #endregion
            
        m_DraggingIcons[eventData.pointerId] = Instantiate(this.icon);
        // m_DraggingIcons[eventData.pointerId] = Instantiate(this.icon.gameObject);
        m_DraggingIcons[eventData.pointerId].GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
        m_DraggingIcons[eventData.pointerId].transform.SetParent(canvasTrans);
        m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();
        SetDraggedPosition(eventData);
        this.isEqualZero = false;
    }
    private void SetDraggedPosition(PointerEventData eventData)
    {
        if (dragOnSurface && eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
            m_DraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcons[eventData.pointerId].GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId],
                      eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlanes[eventData.pointerId].rotation;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons[eventData.pointerId] != null)
        {
            SetDraggedPosition(eventData);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (m_DraggingIcons[eventData.pointerId] != null)
        {
            m_DraggingIcons[eventData.pointerId].gameObject.SetActive(false);
            Destroy(m_DraggingIcons[eventData.pointerId].gameObject, 1);
        }

    }
    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null)
            return null;
        var comp = go.GetComponent<T>();
        if (comp != null)
            return comp;
        var t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
