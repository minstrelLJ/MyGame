using System;

[Serializable]
public class PersonalInfo
{
    /// <summary>
    /// ID
    /// </summary>
    public string id;

    /// <summary>
    /// 昵称
    /// </summary>
    public string nickname;

    /// <summary>
    /// 手机
    /// </summary>
    public string phone;

    /// <summary>
    /// 微信号
    /// </summary>
    public string weixin;

    /// <summary>
    /// 地址
    /// </summary>
    public string address;

    /// <summary>
    /// 姓名
    /// </summary>
    public string playername;

    /// <summary>
    /// 性别
    /// </summary>
    public Sex sex;

    /// <summary>
    /// 年龄
    /// </summary>
    public int age;

    /// <summary>
    /// 店铺名称
    /// </summary>
    public string shopName;

    /// <summary>
    /// 店铺地址
    /// </summary>
    public string shopAddress;

    /// <summary>
    /// 备注
    /// </summary>
    public string remarks;

    /// <summary>
    /// 商铺照片
    /// </summary>
    public string imagePath;
}

public enum Sex
{
    Secrecy = 1,
    Man,
    Female
}