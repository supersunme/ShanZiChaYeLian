using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class OrderCompleteAni : MonoBehaviour
{

    public GameObject xyImage;

    public GameObject blackImage;

    public void AnimationComplete()
    {
        if (this.xyImage != null)
            this.xyImage.SetActive(true);
        StartCoroutine(this.DelayHide());
    }

    IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(4);
        if (this.xyImage != null)
            this.xyImage.gameObject.SetActive(false);
        if (this.blackImage.gameObject != null)
            this.blackImage.gameObject.SetActive(false);
    }
}
