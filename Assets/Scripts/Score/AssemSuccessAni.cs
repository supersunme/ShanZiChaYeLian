using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemSuccessAni : MonoBehaviour
{
    private void OnEnable()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(this.DelayHide());
    }

    private void OnDisable()
    {
        StopCoroutine(this.DelayHide());
    }

    public IEnumerator DelayHide()
    {
        yield return new WaitForSeconds(4);
        this.gameObject.SetActive(false);
    }

    public void AnimationComplete()
    {
        Debug.Log("动画播放完成");
    }
}
