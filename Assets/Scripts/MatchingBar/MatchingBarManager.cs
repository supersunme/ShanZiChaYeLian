using System.Collections.Generic;
using UnityEngine;
public class MatchingBarManager : MonoBehaviour
{
    private static MatchingBarManager _Instance;
    public static MatchingBarManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    void Awake()
    {
        _Instance = this;
    }
    public Dictionary<string, GameObject> partDictionary = new Dictionary<string, GameObject>();
    //当有新的部件产生的时候，将字典中的所有部件，与当前的每一张订单进行对比，比对当前所拥有的原料是否满足订单的要求。
    //这个函数的触发是在每次加工部件的时候
    //将字典中的存在的部件转化为part对象
}
