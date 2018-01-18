

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DialogManager : MonoBehaviour
{
    #region 当前类所需的实例变量
    public List<Sprite> talkList = new List<Sprite>();
    public Queue<Sprite> talkQueue = new Queue<Sprite>();
    public Image talkImage;
    public Button button;
    private int currentIndex;
    #endregion
    //黑色的遮罩
    public Transform blckMask;

    public GameObject haloImage;

    public GameObject miniHaloImage;

    public GameObject mminiImage;

    public GameObject guideImage;

    public GameObject xyObj;

    public GameObject nextStepObj;
    void Start()
    {
        for (int i = 0; i < talkList.Count; i++)
        {
            talkQueue.Enqueue(talkList[i]);
        }
        talkImage.sprite = talkQueue.Dequeue();
        button.onClick.AddListener(() =>
        {
            ClickNextDialog();
        });
    }
    //点击按钮之后出发下一个对话框
    public void ClickNextDialog()
    {
        if (talkQueue.Count != 0)
        {
            talkImage.sprite = talkQueue.Dequeue();
            CheckQueueIndex(currentIndex++);
        }
        else
        {
            SceneManager.LoadScene("Level_01");
        }
    }
    //判断当前点击对话框的次数，定位到当前对应需要出发的事件
    public GameObject dialogAni;

    public GameObject nextLevelImag;
    public void CheckQueueIndex(int index)
    {
        Debug.Log(index);
        switch (index)
        {
            case 1:
                Debug.Log("点击了第1个按钮");
                break;
            case 2:
                Debug.Log("点击了第2个按钮");
                break;
            case 3:
                Debug.Log("点击了第3个按钮");
                this.blckMask.SetSiblingIndex(this.blckMask.GetSiblingIndex() - 1);
                this.haloImage.gameObject.SetActive(true);
                break;
            case 4:
                Debug.Log("点击了第4个按钮");
                // this.orderObj.gameObject.SetActive(true);
                this.haloImage.gameObject.SetActive(false);
                this.miniHaloImage.gameObject.SetActive(true);
                break;
            case 5:
                this.haloImage.gameObject.SetActive(false);
                this.miniHaloImage.gameObject.SetActive(false);
                mminiImage.gameObject.SetActive(true);

                break;
            case 6:
                mminiImage.gameObject.SetActive(false);
                this.guideImage.gameObject.SetActive(true);
                this.xyObj.SetActive(true);
                this.blckMask.gameObject.SetActive(false);
                nextStepObj.gameObject.SetActive(false);
                break;
            case 7:
                nextStepObj.gameObject.SetActive(false);
                break;
            case 11:
                this.dialogAni.gameObject.SetActive(true);
                this.button.gameObject.SetActive(false);
                nextLevelImag.SetActive(true);
                break;
            default:
                // this.blckMask.SetSiblingIndex(this.blckMask.GetSiblingIndex()+2);
                break;
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level_01");
    }
}