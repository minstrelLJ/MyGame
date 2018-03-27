using System;

[Serializable]
public class RechargeInfo
{
    public RechargeInfo() { }
    public RechargeInfo(JSONObject args)
    {
        nickname = args["nickname"].str;
        rechargeTime = args["rechargeTime"].str;
        price = args["price"].f;
        managerBonus = args["managerBonus"].f;
        businessBonus = args["businessBonus"].f;
        rechargeType = (RechargeType)(int)args["rechargeType"].i;
    }

    public string nickname;                       // 昵称
    public string rechargeTime;                 // 充值时间
    public float price;                                // 充值金额
    public float managerBonus;                // 市场经理佣金
    public float businessBonus;                 // 合作商家佣金
    
        
    public RechargeType rechargeType;    // 充值类型
}