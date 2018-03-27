using System;

public class InfoBase 
{
    public string id;                     // ID
    public string registerTime;     // 注册时间
    public string nickname;         // 昵称
    public string phone;              // 手机
    public string weixin;              // 微信号
    public string address;            // 地址
    public string playername;      // 姓名
    public Sex sex;                      // 性别
    public int age;                      // 年龄
    public string shopName;      // 店铺名称
    public string shopAddress;   // 店铺地址
    public string remarks;          // 备注
    public string imagePath;      // 商铺照片
    public PlayerType playerType;                   // 玩家类型
}

public enum PlayerType
{
    Company = 1,
    Manager,
    Business,
    Player,
}

public enum AuditsType
{
    Adopt = 1,                // 审核中
    NotAdopt,                // 未通过
    HaveAdopt               // 已通过
}

public enum BusinessType
{
    Common = 1
}


/// <summary>
/// 充值类型
/// </summary>
public enum RechargeType
{
    RoomCard = 1,       // 房卡
    Money                    // 钻石
}

/// <summary>
/// 兑换类型
/// </summary>
public enum ExchangeGiftRollType
{
    GiftRoll = 1,               // 礼品卷
}

public class TimePar
{
    public int toggleValue;
    public int year1;
    public int year2;
    public int month1;
    public int month2;
    public int day1;
    public int day2;

    public TimePar()
    {
        toggleValue = 1;
        year1 = DateTime.Now.Year;
        year2 = DateTime.Now.Year;
        month1 = DateTime.Now.Month;
        month2 = DateTime.Now.Month;
        day1 = DateTime.Now.Day;
        day2 = DateTime.Now.Day;
    }
}

[Serializable]
public class ApplyBonusInfo
{
    public ApplyBonusInfo() { }
    public ApplyBonusInfo(JSONObject args)
    {
        time = args["time"].str;
        count = args["count"].f;
    }
    public string time;
    public float count;
}

[Serializable]
public class GetBonusInfo
{
    public GetBonusInfo() { }
    public GetBonusInfo(JSONObject args)
    {
        time = args["time"].str;
        nickname = args["nickname"].str;
        recharge = args["recharge"].f;
        bonus = args["bonus"].f;
    }
    public string time;
    public string nickname;
    public float recharge;
    public float bonus;
}

[Serializable]
public class ApplyGiftRollInfo
{
    public ApplyGiftRollInfo() { }
    public ApplyGiftRollInfo(JSONObject args)
    {
        time = args["time"].str;
        count = args["count"].f;
    }
    public string time;
    public float count;
}

[Serializable]
public class GetGiftRollInfo
{
    public GetGiftRollInfo() { }
    public GetGiftRollInfo(JSONObject args)
    {
        time = args["time"].str;
        count = (int)args["count"].i;
        bonus = args["bonus"].f;
    }
    public string time;
    public int count;
    public float bonus;
}