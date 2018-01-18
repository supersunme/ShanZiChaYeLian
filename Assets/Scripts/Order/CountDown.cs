using System;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Image fliiImage;
    public float countDownTimer = 60;
    void Start()
    {
        this.fliiImage.DOFillAmount(0, countDownTimer).OnUpdate(
            () =>
                {
                    if (this.fliiImage.fillAmount < 0.25f)
                    {
                        this.fliiImage.DOColor(new Color(1, 0, 0, 1), 2);

                    }
                    if (this.fliiImage.fillAmount < 0.1f)
                    {
                        this.fliiImage.transform.parent.GetComponent<Image>().DOColor(new Color(1, 0, 0, 1), 5);
                    }
                }).OnComplete(() =>
        {
            int temp = 0;
            Star star = this.GetComponent<OrderConfig>().star;
            if (star == Star.S1)
            {
                temp = -1;
            }
            else if (star == Star.S2)
            {
                temp = -2;
            }
            else if (star == Star.S3)
            {
                temp = -3;
            }
            OrderManager.Instance.currentOrderList.Remove(this.GetComponent<OrderConfig>());
            //此处是订单没有被完成，相应的需要扣除分数
            ScoreManager.Instance.UpdateScoreText(temp);
            OrderManager.Instance.CreateNewOrder(this.GetComponent<OrderConfig>().posIndex, false);
            Debug.Log("到时间了");
            this.gameObject.SetActive(false);
        });
    }
}
