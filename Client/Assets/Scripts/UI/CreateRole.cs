using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRole : UIBase
{
    ComInput inName;
    ComButton btnCreate;

    public override void Init()
    {
        inName = this.transform.Find("InputField").GetComponent<ComInput>();
        inName.placeholder = "请输入角色名称";
        inName.limit = 7;

        btnCreate = this.transform.Find("Button").GetComponent<ComButton>();
        btnCreate.text = "创建角色";
        btnCreate.Event(Create);
    }

    private void Create()
    {
        NetworkManager.Instance.SendCreateRole((data) => 
        {
            if (data.error > 0)
            {
                CF.ShowErrorMessage(data.error);
                return;
            }

            Hide();
        }, inName.text);
    }
}
