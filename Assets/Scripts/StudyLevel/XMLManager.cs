using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class XMLManager : MonoBehaviour
{
    private string xmlPath;
    private static XMLManager _instance;
    public UserDataInfo userDataInfo;
    public static XMLManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //  userDataInfo.programRunTime = System.DateTime.Now.Year + "." + System.DateTime.Now.Month + "." + System.DateTime.Now.Day + "." +
        //                              System.DateTime.Now.Hour + "." + System.DateTime.Now.Minute + "." + System.DateTime.Now.Second;
    }
    public void CreateXML()
    {
        xmlPath = Application.streamingAssetsPath + "/XML/用户信息.xml";
        if (!File.Exists(xmlPath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("信息");
            xmlDoc.AppendChild(root);
            XmlElement user = xmlDoc.CreateElement("用户");
            //  user.SetAttribute("ID", ID.ToString());
            root.AppendChild(user);
            //场景1节点
            if (userDataInfo.programRunTime != "")
            {
                XmlElement programRun = xmlDoc.CreateElement("开始时间");
                programRun.InnerText = userDataInfo.programRunTime;
                user.AppendChild(programRun);
            }
            if (this.userDataInfo.userScore != "")
            {
                XmlElement programEnd = xmlDoc.CreateElement("当前用户得分");
                programEnd.InnerText = this.userDataInfo.userScore;
                user.AppendChild(programEnd);
            }
            if (this.userDataInfo.userScore != "")
            {
                XmlElement userScore = xmlDoc.CreateElement("结束时间");
                userScore.InnerText = userDataInfo.programEndTime;
                user.AppendChild(userScore);
            }
            xmlDoc.Save(xmlPath);
            InitUserDataInfo();
        }
        else
        {
            string filePath = Application.streamingAssetsPath + "/XML/用户信息.xml";
            if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode node = xmlDoc.SelectSingleNode("信息");
                XmlElement user = xmlDoc.CreateElement("用户");
                node.AppendChild(user);
                if (userDataInfo.programRunTime != "")
                {
                    XmlElement programRun = xmlDoc.CreateElement("开始时间");
                    programRun.InnerText = userDataInfo.programRunTime;
                    user.AppendChild(programRun);
                }
                if (this.userDataInfo.userScore != "")
                {
                    XmlElement programEnd = xmlDoc.CreateElement("当前用户得分");
                    programEnd.InnerText = this.userDataInfo.userScore;
                    user.AppendChild(programEnd);
                }
                if (this.userDataInfo.userScore != "")
                {
                    XmlElement userScore = xmlDoc.CreateElement("结束时间");
                    userScore.InnerText = userDataInfo.programEndTime;
                    user.AppendChild(userScore);
                }
                xmlDoc.Save(xmlPath);
                xmlDoc.Save(filePath);

                InitUserDataInfo();
            }
        }
    }
    void InitUserDataInfo()
    {
        userDataInfo = new UserDataInfo()
        {
            programRunTime = "",
            programEndTime = "",
            userScore = ""
        };
    }
}
