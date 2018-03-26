using System;

[Serializable]
public class AuditsInfo
{
    public AuditsInfo() { }
    public AuditsInfo(JSONObject args)
    {
        id = args["id"].str;
        nickname = args["nickname"].str;
        phone = args["phone"].str;
        address = args["address"].str;
        auditsTime = "2018/2/2";
        auditsPlayer = args["auditsPlayer"].str;
        manager = args["manager"].str;
        zone = args["zone"].str;
        businessType = (BusinessType)(int)args["businessType"].i;
        auditsType = (AuditsType)(int)args["auditsType"].i;
    }

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
    /// 地址
    /// </summary>
    public string address;

    /// <summary>
    /// 审核人
    /// </summary>
    public string auditsPlayer;

    /// <summary>
    /// 审核时间
    /// </summary>
    public string auditsTime;

    /// <summary>
    /// 市场经理
    /// </summary>
    public string manager;

    /// <summary>
    /// 地区
    /// </summary>
    public string zone;

    /// <summary>
    /// 商家类型
    /// </summary>
    public BusinessType businessType;

    /// <summary>
    /// 审核状态
    /// </summary>
    public AuditsType auditsType;
}