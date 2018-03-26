using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Common;
using Data;

public class ConfigeManager : Singleton<ConfigeManager>
{
    Dictionary<int, string> errorDic;

    public void Init()
    {
        errorDic = new Dictionary<int, string>();
        ConfigeManager.Instance.GetErrorConfige();
    }

    private void GetErrorConfige()
    {
        string content = ResourceManager.Instance.LoadText("Configs/ErrorConfig");
        List<ErrorMessage>  errorList = JsonConvert.DeserializeObject<List<ErrorMessage>>(content);
        foreach (var item in errorList)
        {
            errorDic[(int)item.ID] = item.Message;
        }
    }

    public string GetErrorMessage(int errorCode)
    {
        string msg;
        if (ConfigeManager.Instance.errorDic.TryGetValue(errorCode, out msg))
        {
            return msg;
        }
        else
        {
            msg = "未知错误 " + errorCode;
        }
        return msg;
    }
}
