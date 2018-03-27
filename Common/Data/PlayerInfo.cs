using System;
using System.Collections.Generic;

[Serializable]
public class PlayerInfo : InfoBase
{
    public PlayerInfo() { }
    public PlayerInfo(JSONObject args, bool isSimple = false)
    {
        nickname = args["nickname"].str;
        registerTime = args["registerTime"].str;

        if (isSimple)
        {
            id = args["id"].str;
            totalRechargeCount = args["totalRechargeCount"].f;
            totalExchangeCount = args["totalExchangeCount"].f;
        }
        else
        {
            phone = args["phone"].str;
            playername = args["playername"].str;
            sex = (Sex)args["sex"].i;
            age = (int)args["age"].i;
            address = args["address"].str;
            remarks = args["remarks"].str;

            lastLoginTime = args["lastLoginTime"].str;
            totalCardRechargeCount = args["totalCardRechargeCount"].f;
            totalGameRechargeCount = args["totalGameRechargeCount"].f;
            totalManagerBonus = args["totalManagerBonus"].f;
            totalBusinessBonus = args["totalBusinessBonus"].f;
            totalExchangeCount = args["totalExchangeCount"].f;
        }
    }

    // 玩家数据
    public string lastLoginTime;                      // 最后登录时间
    public float totalCardRechargeCount;       // 房卡充值总数
    public float totalGameRechargeCount;     // 钻石充值总数
    public float totalManagerBonus;              // 市场经理佣金总数
    public float totalBusinessBonus;               // 合作商家佣金总数

    public float totalRechargeCount;              // 充值总额
    public float totalExchangeCount;              // 兑换总额
    public float totalBonus;                            // 佣金总额
}