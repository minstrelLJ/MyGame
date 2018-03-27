using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInfo : InfoBase 
{
    public ManagerInfo(JSONObject args, bool isSimple = false, bool isMain = false)
    {
        nickname = args["nickname"].str;
        businessCount = (int)args["businessCount"].i;
        playerCount = (int)args["playerCount"].i;

        if (isSimple)
        {
            id = args["id"].str;
            totalBonus = args["rebate"].f;
            registerTime = args["registerTime"].str;
        }
        else
        {
            if (isMain)
            {
                getBonusMsgCount = (int)args["getBonusMsgCount"].i;
                applyBonusMsgCount = (int)args["applyBonusMsgCount"].i;

                totalRecharge = args["totalRecharge"].f;
                totalCardRecharge = args["totalCardRecharge"].f;
                totalGameRecharge = args["totalGameRecharge"].f;

                totalCardBonus = args["totalCardBonus"].f;
                totalGameBonus = args["totalGameBonus"].f;
                haveApplyBonus = args["haveApplyBonus"].f;
                notApplyBonus = args["notApplyBonus"].f;
            }
            else
            {
                registerTime = args["registerTime"].str;
                weixin = args["weixin"].str;
                playername = args["playername"].str;
                sex = (Sex)args["sex"].i;
                age = (int)args["age"].i;
                remarks = args["remarks"].str;

                notReceiveBonus = args["notReceiveBonus"].f;
                haveReceiveBonus = args["haveReceiveBonus"].f;
            }

            phone = args["phone"].str;
            starLevel = (int)args["starLevel"].i;
            totalBonus = args["totalBonus"].f;
        }
    }

    public int starLevel;                                  // 星级
    public int playerCount;                             // 玩家数量
    public int businessCount;                         // 合作商家数量

    public float totalRecharge;                         // 充值总额
    public float totalCardRecharge;                 // 房卡充值总额
    public float totalGameRecharge;               // 钻石充值总额
    public float totalBonus;                             // 佣金总额
    public float totalCardBonus;                      // 房卡佣金总额
    public float totalGameBonus;                    // 钻石佣金总额

    public float haveApplyBonus;                    // 已申请佣金
    public float notApplyBonus;                      // 未申请佣金

    public float haveReceiveBonus;                  // 已领取佣金
    public float notReceiveBonus;                    // 未领取佣金

    public float canTakenOutCashCount;        // 可提现金额
    public float canExchangeCount;                // 可兑换礼品卷

    public int getBonusMsgCount;                  // 获取佣金信息数量
    public int applyBonusMsgCount;              // 申请佣金信息数量

    // 领取礼品卷记录
    public GetGiftRollInfo[] getGiftRollList;
}
