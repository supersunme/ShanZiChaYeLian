
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

public class OrderInitialization : MonoBehaviour
{
    public Transform parentTans;
    //之前生成的随机数
    public int[] alreadyExistRandoms;

    public GameObject UIManager;
    //private string[] orderNames = {"NONE","DM", "JY", "SQ", "XY", "XY" };
    void Start()
    {
        this.CreateRandomNumber();
        FirstCreateOrder();
    }
    public void FirstCreateOrder()
    {
        int[] temp = CreateRandomNumber();
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Resources.Load("Order/one/" + temp[i]) as GameObject;
            GameObject orderObj = Instantiate(obj);
            orderObj.name = temp[i].ToString();
            orderObj.transform.SetParent(parentTans);
            // orderObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * 338 + 187, -253);
            orderObj.GetComponent<RectTransform>().anchoredPosition=new Vector2(-351,-290);
            StartCoroutine(OrderMove(orderObj.GetComponent<RectTransform>(),i));
            //  orderObj.transform.localScale = Vector3.one;
            //  orderObj.GetComponent<OrderConfig>().posIndex = i;
            orderObj.GetComponent<OrderConfig>().posIndex = i;
            orderObj.transform.localScale = Vector3.one;
            //  OrderManager.Instance.currentOrderDictionary.Add(orderObj.name, orderObj);
            OrderManager.Instance.currentOrderList.Add(orderObj.GetComponent<OrderConfig>());

            OrderManager.Instance.subScriptIndex.Add(temp[i]);
        }
        
    }
    //随机生成三个不重复的随机数
    private int[] CreateRandomNumber()
    {
        int[] temp = new int[3];
        List<int> nums = new List<int>();
        for (int i = 1; i <= 5; i++)
        {
            nums.Add(i);
        }
        for (int i = 0; i < 3; i++)
        {
            int j = Random.Range(0, nums.Count);
            //Debug.Log(nums[j]);
            temp[i] = nums[j];
            nums.Remove(nums[j]);
        }
        this.alreadyExistRandoms = temp;
        return temp;
    }
    //创建新的订单,不与之前出现过的订单重复
    public void CreateNewOrderByStar(Star star)
    {
        switch (star)
        {
            case Star.S1:
                break;
            case Star.S2:
                    break;
            case Star.S3:
                break;
            default:
                break;
        }
    }

    public IEnumerator OrderMove(Transform tans,int index)
    {
        
        yield return new WaitForSeconds(1);
        tans.GetComponent<RectTransform>().DOAnchorPos(new Vector2(index * 338 + 187, -253), 1.2f);
        tans.SetSiblingIndex(tans.GetSiblingIndex()-index);
        tans.GetComponent<CountDown>().enabled = false;
        if (index == 2)
        {
            yield return new WaitForSeconds(2);
            this.UIManager.gameObject.SetActive(true);
            this.UIManager.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        }
    }
}
