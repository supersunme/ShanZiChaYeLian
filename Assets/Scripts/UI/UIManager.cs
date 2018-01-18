using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public GameObject endUI;
    public GameObject overUI;
    public Text score;
    public GameObject blackMaskImage;
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void Start()
    {
        _instance = this;
        this.StopTime();
        StartCoroutine(DelayTimeScale());
    }
    public void StartGmae()
    {
        XMLManager.Instance.userDataInfo.programRunTime = System.DateTime.Now.Year + "."
            + System.DateTime.Now.Month + "." + System.DateTime.Now.Day + "." +
                             System.DateTime.Now.Hour + "." + System.DateTime.Now.Minute + "." + System.DateTime.Now.Second;
        Time.timeScale = 1;
        this.StartTime();
        blackMaskImage.SetActive(false);
        this.startUI.SetActive(false);
        endUI.SetActive(false);
        overUI.SetActive(false);
        System.GC.Collect();
    }
    public void EndGame()
    {
        Time.timeScale = 0;
        blackMaskImage.SetActive(true);
        this.startUI.SetActive(false);
        endUI.SetActive(true);
        overUI.SetActive(false);
        System.GC.Collect();
    }
    public void OverEndGame()
    {
        Time.timeScale = 0;
        blackMaskImage.SetActive(true);
        this.startUI.SetActive(false);
        endUI.SetActive(false);
        overUI.SetActive(true);
        this.overUI.transform.GetChild(0).GetComponent<Text>().text = this.score.text;
    }
    public void ReplayGame()
    {
        Time.timeScale = 1;
        //Application.Quit();
        System.GC.Collect();
        SceneManager.LoadScene(1);
        System.GC.Collect();
    }
    public void ExitGame()
    {
        Time.timeScale = 1;
        System.GC.Collect();
        SceneManager.LoadScene(0);
        System.GC.Collect();
    }
    private void StopTime()
    {
        ScoreManager.Instance.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            FindObjectsOfType<CountDown>()[i].enabled = false;
        }
    }
    public void StartTime()
    {
        Time.timeScale = 1;
        ScoreManager.Instance.enabled = true;
        for (int i = 0; i < 3; i++)
        {
            FindObjectsOfType<CountDown>()[i].enabled = true;
        }
    }
    private IEnumerator DelayTimeScale()
    {
        yield return new WaitForSeconds(1.1f);
        Time.timeScale = 0;
    }
    public void BackStudyLevel()
    {
        System.GC.Collect();
        SceneManager.LoadScene(0);
    }
}
