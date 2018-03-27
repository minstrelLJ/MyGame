using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyInfo : InfoBase 
{
    public CompanyInfo(JSONObject args, bool isMain = false)
    {
        nickname = args["nickname"].str;
        phone = args["phone"].str;

        roomCardCount = (int)args["roomCardCount"].i;
        managerCount = (int)args["managerCount"].i;
        businessCount = (int)args["businessCount"].i;
        playerCount = (int)args["playerCount"].i;

        totalCardRecharge = args["totalCardRecharge"].f;
        totalGameRecharge = args["totalGameRecharge"].f;
        totalNotTakenOutCash = args["totalNotTakenOutCash"].f;
        totalHaveTakenOutCash = args["totalHaveTakenOutCash"].f;
        totalHaveExchangeGiftRoll = args["totalHaveExchangeGiftRoll"].f;
        totalNotExchangeGiftRoll = args["totalNotExchangeGiftRoll"].f;
    }

    public int starLevel;                                  // 星级
    public int roomCardCount;                        // 持有房卡数
    public int managerCount;                         // 市场经理数
    public int businessCount;                          // 合作商家数
    public int playerCount;                             // 玩家总数

    public float totalCardRecharge;                 // 房卡充值总额
    public float totalGameRecharge;               // 钻石充值总额
    public float totalHaveTakenOutCash;         // 已提取现金
    public float totalNotTakenOutCash;           // 未提取现金
    public float totalHaveExchangeGiftRoll;     // 已兑换礼品卷总数
    public float totalNotExchangeGiftRoll;       // 未兑换礼品卷总数

    public float haveExchangeGiftRoll;            // 已兑换礼品卷
    public float notExchangeGiftRoll;              // 未兑换礼品卷
}

public class ExchangeGiftRoll
{
    public ExchangeGiftRoll(JSONObject args)
    {
        haveExchangeGiftRoll = args["haveExchangeGiftRoll"].f;
        notExchangeGiftRoll = args["notExchangeGiftRoll"].f;
    }

    public float haveExchangeGiftRoll;
    public float notExchangeGiftRoll;
}