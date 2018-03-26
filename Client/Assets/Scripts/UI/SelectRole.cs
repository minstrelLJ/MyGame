using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRole : UIBase
{
    Transform trRoleInfo;
    ComButton btnEnterGame;

    public override void Init()
    {
        trRoleInfo = this.transform.Find("RoleInfo");

        btnEnterGame = this.transform.Find("RoleInfo/Button").GetComponent<ComButton>();
        btnEnterGame.text = "进入游戏";
        btnEnterGame.Event(EnterGame);
    }
    public override void Refresh()
    {
        GetRole();
    }

    private void GetRole()
    {
        NetworkManager.Instance.SendGetRole((data) =>
        {
            if (data.error  == 1)
            {
                trRoleInfo.gameObject.SetActive(false);
                UIRoot.ShowPage(Page.CreateRole);
            }
            else
            {
                trRoleInfo.gameObject.SetActive(true);
                GameManager.Instance.role = new Data.Role(data);
            }
        });
    }
    private void EnterGame()
    {
        
    }
}
