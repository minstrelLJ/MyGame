using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPage : UIBase
{
    private Text textTitle;
    private ComInput inId;
    private ComInput inPw1;
    private ComInput inPw2;
    private ComButton btnRegister;

    public override void Init()
    {
        textTitle = this.transform.Find("Text").GetComponent<Text>();
        textTitle.text = "用户登陆";

        inId = this.transform.Find("ComInput (1)").GetComponent<ComInput>();
        inId.placeholder = "用户名";
        inId.limit = 20;

        inPw1 = this.transform.Find("ComInput (2)").GetComponent<ComInput>();
        inPw1.placeholder = "密码";
        inPw1.limit = 20;
        inPw1.contentType = InputField.ContentType.Password;

        inPw2 = this.transform.Find("ComInput (3)").GetComponent<ComInput>();
        inPw2.placeholder = "确认密码";
        inPw2.limit = 20;
        inPw2.contentType = InputField.ContentType.Password;

        btnRegister = this.transform.Find("Button").GetComponent<ComButton>();
        btnRegister.text = "注册";
        btnRegister.Event(btnRegisterCallBack);

        clickMaskAction = () =>
        {
            UIRoot.ShowPage(Page.LoginPage);
            Hide();
        };
    }

    private void btnRegisterCallBack()
    {
        Debug.Log("btnRegister");

        if (inId.text.Length < 8)
        {
             CF.ShowHint("账号必须为8-20位字符加数字的组合");
            return;
        }

        if (inPw1.text.Length < 8 || inPw2.text.Length < 8)
        {
            CF.ShowHint("密码必须为8-20位字符加数字的组合");
            return;
        }

        if (inPw1.text != inPw2.text)
        {
            CF.ShowHint("两次输入密码不一致");
            return;
        }

        NetworkManager.Instance.SendRegister((data)=>
        {
            UIRoot.ShowPage(Page.LoginPage);
            Hide();
        },inId.text, inPw1.text);
    }
}
