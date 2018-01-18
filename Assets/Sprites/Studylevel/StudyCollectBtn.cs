using System;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using DG.Tweening.Core;

using UnityEngine;
using UnityEngine.UI;

public class StudyCollectBtn : MonoBehaviour
{
    public Image xyflashImage;
    //公有变量区域
    [Header("气泡预制")]
    public GameObject bubblePrefab;

    [Header("原料预制")]
    public GameObject materialPrefab;

    public GameObject materialPrefabM;

    [HideInInspector]
    public GameObject materialObj;

    public Transform materialTransform;

    public int materialPanelIndex;

    public Vector2 materialStartPos;

    [Header("原料数量控制")]
    public int currentCount = 0;

    private int countLimit = 5;
    public int tempCount = 0;
    //点击当前按钮需要加工和采集的原料类型
    public Material material;
    //私有变量区域
    private Button button;
    //DotweenPath的父级对象
    private ABSAnimationComponent dotweenPathABS;

    [Header("相应的比例系数")]
    public int factor = 0;

    public GameObject guideImage;

    public GameObject dragStudyImage;

    public GameObject studyDrop;

    public CountDown countDown;

    public DialogManager dialogManager;
    void Start()
    {
        this.button = this.GetComponent<Button>();
        this.button.onClick.AddListener(
            () =>
                {
                    if (this.xyflashImage.gameObject.activeSelf == false)
                    {
                        this.xyflashImage.gameObject.SetActive(true);
                    }
                    if (this.currentCount < this.countLimit)
                    {
                        if (this.tempCount < this.countLimit)
                        {
                            this.CreateBubble();
                        }
                    }
                    if (this.tempCount < 5) tempCount++;
                    if (this.guideImage != null && this.guideImage.activeSelf == true)
                    {
                        this.guideImage.gameObject.SetActive(false);
                    }
                });
    }
    //生成气泡，并创建动画，触发动画播放完成事件
    public void CreateBubble()
    {
        if (this.bubblePrefab != null)
        {
            if (this.countDown.enabled == false)
            {
                this.countDown.enabled = true;
            }
            GameObject bubble = Instantiate(this.bubblePrefab, this.transform) as GameObject;
            bubble.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            dotweenPathABS = bubble.GetComponent<DOTweenPath>() as ABSAnimationComponent;
            this.dotweenPathABS.onComplete.AddListener(
                () =>
                    {
                        DOTweenPath dotweenPath = bubble.GetComponent<DOTweenPath>();
                        Vector2 endPos = new Vector2(materialPanelIndex * 123 + -397, -25);
                        if (this.materialObj == null)
                        {
                            //this.materialObj = CreateMaterial(materialStartPos, endPos, false);
                            this.materialObj = CreateMaterial(new Vector2(190, -332), endPos, false);
                        }
                        else
                        {
                            //CreateMaterial(materialStartPos, endPos, true);
                            CreateMaterial(new Vector2(190, -332), endPos, true);
                        }
                        Destroy(bubble);
                    });
        }
    }
    //创建原料，更新原料的数量
    public GameObject btnhaloANI;
    private GameObject CreateMaterial(Vector2 startPos, Vector2 endpos, bool isExist)
    {
        GameObject temp = null;
        if (this.materialObj != null) temp = this.materialPrefabM;
        if (this.materialObj == null) temp = this.materialPrefab;
        GameObject materialObjTmp = Instantiate(temp, this.materialTransform);
        materialObjTmp.GetComponent<RectTransform>().anchoredPosition = startPos;
        materialObjTmp.transform.GetChild(0).gameObject.SetActive(false);

        materialObjTmp.GetComponent<RectTransform>().DOAnchorPos(endpos, 1).OnComplete(
            () =>
                {
                    if (isExist == false)
                    {
                        //这个对象是第一次生成的对象
                        materialObj.transform.GetChild(0).gameObject.SetActive(true);
                        this.currentCount++;
                        //materialObjTmp.transform.GetChild(0).gameObject.SetActive(true);
                        materialObj.transform.GetChild(0).GetComponent<Text>().text = this.currentCount.ToString();
                        this.dialogManager.ClickNextDialog();
                    }
                    else
                    {
                        if (this.materialObj.activeSelf == false) this.materialObj.gameObject.SetActive(true);
                        this.currentCount++;
                        materialObj.transform.GetChild(0).GetComponent<Text>().text = this.currentCount.ToString();
                        Destroy(materialObjTmp);

                        if (Int32.Parse(materialObj.transform.GetChild(0).GetComponent<Text>().text) >= 5)
                        {
                            Debug.Log("点击了此处");
                            this.btnhaloANI.SetActive(false);
                            this.dialogManager.ClickNextDialog();
                            this.dragStudyImage.gameObject.SetActive(true);
                            this.studyDrop.GetComponent<StudyDrop>().enabled = true;
                        }

                    }
                });
        return materialObjTmp;
    }
}
