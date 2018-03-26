using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CF 
{
    public static void ConnectServer()
    {
        NetworkManager.Instance.Connect();
    }

    public static void ShowHint(string msg)
    {
        GameObject go = ResourceManager.Instance.LoadAsset<GameObject>(UIRoot.uiPrefabRootPath + "HintMsg");
        HintMsg com = GameObject.Instantiate(go).GetComponent<HintMsg>();
        com.transform.SetParent(UIRoot.Instance.popupRoot);
        com.transform.localScale = Vector3.one;
        com.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 165, 0);
        com.SetMsg(msg);
    }

    public static void ShowErrorMessage(int errorCode)
    {
        string msg = ConfigeManager.Instance.GetErrorMessage(errorCode);
        ShowHint(msg);
    }
}
