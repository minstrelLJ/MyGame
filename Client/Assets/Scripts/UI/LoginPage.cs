using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPage : UIBase 
{
    private Text textTitle;
    private ComInput inId;
    private ComInput inPw;
    private ComButton btnLogin;
    private ComButton btnRegister;

	public override void Init () 
    {
        textTitle = this.transform.Find("Text").GetComponent<Text>();
        textTitle.text = "用户登陆";

        inId = this.transform.Find("ComInput (1)").GetComponent<ComInput>();
        inId.placeholder = "用户名";
        inId.limit = 20; 

        inPw = this.transform.Find("ComInput (2)").GetComponent<ComInput>();
        inPw.placeholder = "密码";
        inPw.limit = 20;
        inPw.contentType = InputField.ContentType.Password;

        btnLogin = this.transform.Find("btns/Button (1)").GetComponent<ComButton>();
        btnLogin.text = "登录";
        btnLogin.Event(btnLoginCallBack);

        btnRegister = this.transform.Find("btns/Button (2)").GetComponent<ComButton>();
        btnRegister.text = "注册";
        btnRegister.Event(() => 
        { 
            Hide();
            UIRoot.ShowPage(Page.Register);
        });
	}

    private void btnLoginCallBack()
    {
        if (inId.text.Length < 8)
        {
            CF.ShowHint("账号必须为8-20位字符加数字的组合");
            return;
        }

        if (inPw.text.Length < 8)
        {
            CF.ShowHint("密码必须为8-20位字符加数字的组合");
            return;
        }

        NetworkManager.Instance.SendLogin((data) =>
        {
            GameManager.Instance.userId = int.Parse(data.list[0]);
            Hide();

            PlayerPrefs.SetString("accountNumber", inId.text);
            PlayerPrefs.SetString("passWord", inPw.text);
        }, inId.text, inPw.text);
    }
}
