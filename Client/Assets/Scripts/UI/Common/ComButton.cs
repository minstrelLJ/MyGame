using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ComButton : MonoBehaviour
{
    private Button btnButton;
    private Text textBtnText;

    public string text
    {
        set
        {
            if (textBtnText == null) textBtnText = this.transform.Find("Text").GetComponent<Text>();
            textBtnText.text = value;
        }
    }
    public void Event(UnityAction ac)
    {
        if (btnButton == null) btnButton = this.GetComponent<Button>();
        btnButton.onClick.AddListener(ac);
    }
}
