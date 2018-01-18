using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchAnimationEvent : MonoBehaviour
{
    //加工动画播放完成
    public void MatchingComplete()
    {
       // Debug.Log("动画完成");
        if (this.GetComponentInParent<MatchingBar>() != null)
            this.GetComponentInParent<MatchingBar>().AnimationComplete(this.gameObject);
        if (this.GetComponentInParent<StudyDrop>() != null)
        {
            this.GetComponentInParent<StudyDrop>().AnimationComplete(this.gameObject);
        }
    }
}
