using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDelay : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Image>().raycastTarget = true;
        StartCoroutine(this.Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        this.GetComponent<Image>().raycastTarget = false;
    }
}
