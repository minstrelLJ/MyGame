using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintMsg : MonoBehaviour
{
    private Text textMsg;

    public void Awake()
    {
        textMsg = this.transform.Find("Text").GetComponent<Text>();
    }

    public void SetMsg(string msg)
    {
        textMsg.text = msg;
    }

    public void Close()
    {
        DestroyObject(this.gameObject);
    }
}
