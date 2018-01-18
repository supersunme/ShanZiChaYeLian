using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            return _instance;
        }
    }
    //倒计时文本
    public Text countDownText;
    //得分文本
    public Text scoreText;
    public static IEnumerator coroutine;
    private float t = 60;
    public MySql sql;

    public int TotalMinute = 5;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        //StartCoroutine("StartCountDown");
        coroutine = this.StartCountDowns();
        StartCoroutine(coroutine);
        // ChageText();
    }
    //更新得分，根据用户完成订单的状态
    public void UpdateScoreText(int count)
    {
        int currentScore = Int32.Parse(this.scoreText.text);
        if (currentScore < 0)
        {
            this.scoreText.text = (Int32.Parse(this.scoreText.text) + count).ToString();
            Time.timeScale = 0;
            UIManager.Instance.EndGame();
            return;
        }
        this.scoreText.text = (Int32.Parse(this.scoreText.text) + count).ToString();
    }
    //管理总时间，5分钟倒计时，管理游戏时间
    public IEnumerator StartCountDowns()
    {
        // this.countDownText.text = ("0" + minutes.ToString() + ":" + second.ToString());
        //5分钟的倒计时
        int minutes = TotalMinute;
        //每分钟60秒
        int second = 60;
        do
        {
            minutes--;
            second = 60;
            do
            {
                second--;
                yield return new WaitForSeconds(1);
                if (second < 10)
                {
                    this.countDownText.text = ("0" + minutes.ToString() + ":" + "0" + second.ToString());
                    continue;
                }
                this.countDownText.text = ("0" + minutes.ToString() + ":" + second.ToString());
            }
            while (second > 0);
        } while (minutes > 0);
        this.sql.currentScore = Int32.Parse(this.scoreText.text);
        XMLManager.Instance.userDataInfo.programEndTime = System.DateTime.Now.Year + "." + System.DateTime.Now.Month + "." + System.DateTime.Now.Day + "." +
                          System.DateTime.Now.Hour + "." + System.DateTime.Now.Minute + "." + System.DateTime.Now.Second;
        XMLManager.Instance.userDataInfo.userScore = this.scoreText.text.ToString();
        XMLManager.Instance.CreateXML();
        UIManager.Instance.OverEndGame();

    }
}
