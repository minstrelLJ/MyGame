using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class GameMainPage : UIBase 
{
    private ComButton btnEnterGame;
    private ComButton btnAccountChange;

    private string accountNumber;
    private string passWord;

    public override void Init()
    {
        btnEnterGame = this.transform.Find("Button (1)").GetComponent<ComButton>();
        btnEnterGame.text = "开始游戏";
        btnEnterGame.Event(() => 
        {
            Debug.Log("btnEnterGame");

            NetworkManager.Instance.SendEnter((data) => 
            {
                UIRoot.ShowPage(Page.SelectRole);
            });
        });

        btnAccountChange = this.transform.Find("Button (2)").GetComponent<ComButton>();
        btnAccountChange.text = "切换账号";
        btnAccountChange.Event(() =>
        {
            Debug.Log("btnAccountChange");
            UIRoot.ShowPage(Page.Login);
        });

        CheckAccount();
    }

    private void CheckAccount()
    {
        accountNumber = PlayerPrefs.GetString("accountNumber");
        passWord = PlayerPrefs.GetString("passWord");

        if (string.IsNullOrEmpty(accountNumber))
        {
            UIRoot.ShowPage(Page.Login);
        }
        else
        {
            NetworkManager.Instance.SendLogin((data) => 
            {
                if (data.error != 0)
                {
                    UIRoot.ShowPage(Page.Login);
                    return;
                }

                CF.ShowHint("欢迎回来" + accountNumber);
                GameManager.Instance.userId = int.Parse(data.list[0]);
            }, accountNumber, passWord);
        }
    }
}
