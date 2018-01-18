using System;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class OrderManager : MonoBehaviour
{
    //单例模式
    private static OrderManager _instance;
    public static OrderManager Instance
    {
        get
        {
            return _instance;
        }
    }
    //字典类型，用于存储当前场景中存在的订单
    //  public Dictionary<string, GameObject> currentOrderDictionary = new Dictionary<string, GameObject>();
    public List<OrderConfig> currentOrderList = new List<OrderConfig>();
    //当前已经完成的订单的数量
    public int alreadyCompleteOrderCount = 0;
    //  public Part part;
    //创建一个变量，用于存储用户当前已经完成的订单的总数
    public int alreadyCompleteCount = 0;
    //public List<OrderConfig> appeaseOrderList=new List<OrderConfig>();
    //创建一个数组，用于存储已经生成过的订单的下标
    public List<int> subScriptIndex;
    //用户做了完成了3张以上的订单
    private bool isOver = false;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
    }
    //用来比较当前所拥有的的部件是否满足当前存在的订单
    public void CompareOrder()
    {
        //Debug.Log("开始对比");
        for (int i = 0; i < currentOrderList.Count; i++)
        {
            var isExistAndAppease = 0;
            foreach (var k in this.currentOrderList[i].orderNumberDict)
            {
                foreach (var o in MatchingBarManager.Instance.partDictionary)
                {
                    if (k.Key.Equals(o.Key)
                        && Int32.Parse(o.Value.transform.GetChild(0).gameObject.GetComponent<Text>().text) >= k.Value)
                    {
                        isExistAndAppease++;
                        if (isExistAndAppease == this.currentOrderList[i].needPartCount)
                        {
                            Debug.Log("完全满足当前订单的需求" + currentOrderList[i].gameObject.name);
                            //appeaseOrderList.Add(currentOrderList[i].gameObject.GetComponent<OrderConfig>());
                            AssemOrder.Instance.StartAssemOrder(this.currentOrderList[i].gameObject);
                            isExistAndAppease = 0;
                        }
                    }
                }
            }
        }
    }
    //点击拼装之后，再次将所剩原料和当前面板中的订单进行比较，看是否满足需求
    //每次生成新的订单之后，需要再次将新生成的订单和已经有的原材料进行对比，如果满足条件，将订单的拼装按钮显示为可以点击的状态
    public void AssemLaterCompare()
    {
        for (int i = 0; i < currentOrderList.Count; i++)
        {
            var isExistAndAppease = 0;
            foreach (var k in this.currentOrderList[i].orderNumberDict)
            {
                if (k.Key == null)
                {
                    AssemOrder.Instance.EndAssemOrder(this.currentOrderList[i].gameObject);
                }
                foreach (var o in MatchingBarManager.Instance.partDictionary)
                {
                    if (o.Key == null)
                    {
                        AssemOrder.Instance.EndAssemOrder(this.currentOrderList[i].gameObject);
                    }
                    if (k.Key.Equals(o.Key) && Int32.Parse(o.Value.transform.GetChild(0).gameObject.GetComponent<Text>().text) < k.Value)
                    {
                        Debug.Log("不满足，关闭");
                        AssemOrder.Instance.EndAssemOrder(this.currentOrderList[i].gameObject);
                    }
                }
            }
        }
    }
    //当订单被销毁之后，重新创建新的订单,从Resources文件夹中读取对应的Prefab，Instantiate()到相应的位置,判断被销毁的订单是否是用户完成的
    public void CreateNewOrder(int posIndex, bool isAssemComplete)
    {
        string path = null;
        if (isAssemComplete == true)
        {
            alreadyCompleteCount++;
        }
        switch (this.alreadyCompleteCount)
        {
            case 0:
                path = "Order/one/";
                break;
            case 1:
                path = "Order/one/";
                break;
            case 2:
                path = "Order/one/";
                break;
            case 3:
                path = "Order/two/";
                break;
            case 4:
                path = "Order/two/";
                break;
            case 5:
                path = "Order/three/";
                break;
            default:
                isOver = true;
                alreadyCompleteCount = this.MakeRandom();
                path = "Order/three/";
                break;
        }
        int temp = Random.Range(1, 5);
        GameObject obj = null;
        if (this.isOver == false)
        {
            obj = Resources.Load(path + temp.ToString()) as GameObject;
        }
        else
        {
            obj = Resources.Load(path + this.alreadyCompleteCount.ToString()) as GameObject;
            this.isOver = false;
        }
        GameObject orderObj = Instantiate(obj);
        orderObj.name = temp.ToString();
        orderObj.transform.SetParent(this.transform);
        orderObj.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //   orderObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posIndex * 338 + 187, -253);

       // SetStartPosAndTweenPos(orderObj.GetComponent<RectTransform>(), posIndex);
        StartCoroutine(SetStartPosAndTweenPos(orderObj.GetComponent<RectTransform>(), posIndex));
        orderObj.transform.localScale = Vector3.one;
        orderObj.GetComponent<OrderConfig>().posIndex = posIndex;
        OrderManager.Instance.currentOrderList.Add(orderObj.GetComponent<OrderConfig>());
        this.CompareOrder();
    }
    //创建一个随机数，判断哪些订单是已经生成的,排除已经生成过的订单，从剩下的订单中随机生成
    //判断当前订单面板中有哪些订单,当完成的订单到达一定的数量上限之后
    //在[1,2,3,4,5]之间生成一个正整数，出现1,2,3的概率分别为百分之五
    //出现4的概率为百分之三十五，出现5的概率为百分之四十五
    public int MakeRandom()
    {
        int[] seeds = { 1, 2, 3, 4, 5 };

        List<int> seed = new List<int>(18)
                           {
                               1,2,3,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5
                           };
        int temp = Random.Range(0, 18);
        Debug.Log(seed[temp]);
        return seed[temp];
    }
    //设置订单的出现动画
    private IEnumerator SetStartPosAndTweenPos(RectTransform tran, int index)
    {
        yield return new WaitForSeconds(1);
        tran.anchoredPosition = new Vector2(index * 338 + 187, -253 + index + 300);
        tran.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 0.6f);
        tran.DOAnchorPos(new Vector2(index * 338 + 187, -253), 0.6f);
    }
}
