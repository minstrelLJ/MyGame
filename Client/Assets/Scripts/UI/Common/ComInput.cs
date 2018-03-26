using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComInput : MonoBehaviour
{
    private InputField inInput;
    private Text textPlaceholder;

    public string text
    {
        get { return inInput.text; }
        set
        {
            if (inInput == null) inInput = this.transform.Find("InputField").GetComponent<InputField>();
            inInput.text = value;
        }
    }
    public string placeholder
    {
        set
        {
            if (textPlaceholder == null) textPlaceholder = this.transform.Find("InputField/Placeholder").GetComponent<Text>();
            textPlaceholder.text = value;
        }
    }
    public int limit
    {
        set
        {
            if (inInput == null) inInput = this.transform.Find("InputField").GetComponent<InputField>();
            inInput.characterLimit = value;
        }
    }
    public InputField.ContentType contentType
    {
        set
        {
            if (inInput == null) inInput = this.transform.Find("InputField").GetComponent<InputField>();
            inInput.contentType = value;
        }
    }
}
