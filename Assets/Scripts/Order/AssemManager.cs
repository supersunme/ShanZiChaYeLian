using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class AssemManager : MonoBehaviour
{
    private static AssemManager _instance;
    public static AssemManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public Image assemCompleteImage;

    public Image shanziCompleteImage;

    public Image blackMaskImage;
    void Start()
    {
        _instance = this;
    }
    public void CreateAssemSuccessSanzi(Sprite sprite)
    {
        this.blackMaskImage.gameObject.SetActive(true);
        assemCompleteImage.gameObject.SetActive(true);

        this.shanziCompleteImage.sprite = sprite;
        StartCoroutine(ShanziDelayShow());      
    }

    IEnumerator ShanziDelayShow()
    {
        yield return new WaitForSeconds(1.5f);

        this.shanziCompleteImage.SetNativeSize();
        this.shanziCompleteImage.gameObject.SetActive(true);  
        
        yield return StartCoroutine(this.delayHide());
    }
    IEnumerator delayHide()
    {
        yield return new WaitForSeconds(3);
        this.shanziCompleteImage.gameObject.SetActive(false);
        this.blackMaskImage.gameObject.SetActive(false);
        this.assemCompleteImage.gameObject.SetActive(false);
    }
}
