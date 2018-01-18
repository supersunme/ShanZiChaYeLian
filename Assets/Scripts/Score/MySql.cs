using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class MySql : MonoBehaviour
{
    /// <summary>
    /// 记录数据库中比当前用户分数低的总用户数
    /// </summary>
    private int lessCount = 0;
    /// <summary>
    /// 记录参与游戏的所有玩家
    /// </summary>
    private int currentMax = 0;
    /// <summary>
    /// 当前用户的得分情况
    /// </summary>
    public int currentScore = 0;

    public Text overCount;

    private SQLiteHelper sql;

    private SqliteDataReader reader;
    void Awake()
    {
        LoomManager.Loom.QueueOnMainThread(
            () =>
                {
                    if (!File.Exists(Application.streamingAssetsPath + "/userscore.db"))
                    {
                        this.InitSql();
                        Debug.Log("不存在数据库文件，现在重新创建");
                    }
                });
        // this.sql=new SQLiteHelper("data source=userscore.db");
    }
    /// <summary>
    ///创建并初始化一个本地数据库
    /// </summary>
    void InitSql()
    {
        this.sql = new SQLiteHelper("data source=" + Application.streamingAssetsPath + "/userscore.db");
        this.sql.CreateTable("star", new string[] { "ID", "Score", }, new string[] { "INTEGER", "INTEGER" });

        this.sql.InsertValues("star", new[] { "1", "20" });
        this.sql.InsertValues("star", new[] { "2", "10" });
        this.sql.CloseConnection();
    }
    void Start()
    {
        this.sql = new SQLiteHelper("data source=" + Application.streamingAssetsPath + "/userscore.db");
        StartCoroutine("QueryDataIenumator");
    }
    IEnumerator QueryDataIenumator()
    {
        this.QueryData();
        yield return StartCoroutine("GetMaxColIenumator");
    }
    IEnumerator GetMaxColIenumator()
    {
        this.overCount.text =((int)(GetMaxColAndPrecent()*100)).ToString() + "%";
        //yield return StartCoroutine("InsertCurrentData");
        this.InsertData();
        yield return new WaitForEndOfFrame();
        this.reader.Dispose();
        this.reader.Close();
        this.sql.CloseConnection();
        yield return null;
    }
    void QueryData()
    {
        var lessc = 0;
        // SQLiteHelper sql = new SQLiteHelper("data source=" + Application.streamingAssetsPath + "/userscore.db");
        this.reader = sql.ReadTable(
             "star",
             new[] { "Score" },
             new[] { "Score" },
             new string[] { "<=" },
             new[] { this.currentScore.ToString() });
        while (reader.Read())
        {
            lessc++;
        }
        this.lessCount = lessc;
    }
    /// <summary>
    /// 将当前用户的数据插入本地数据库中
    /// </summary>
    void InsertData()
    {
        // SQLiteHelper sql = new SQLiteHelper("data source=" + Application.streamingAssetsPath + "/userscore.db");
        // sql.InsertValues("star",new []{"1",""})
        this.sql.InsertValues("star", new string[] { (this.currentMax + 1).ToString(), this.currentScore.ToString() });
       // sql.CloseConnection();
    }
    /// <summary>
    ///获取当前表中的最大行
    /// </summary>
    float GetMaxColAndPrecent()
    {
        var count = 0;
        this.reader = sql.ReadFullTable("star");
        while (reader.Read())
        {
            count++;
        }
        // yield return reader.Read();
        this.currentMax = count;
        var temp = ((float)this.lessCount / (float)this.currentMax);
        return temp;
    }
}
