using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessInfo : InfoBase 
{
    public BusinessInfo(JSONObject args, bool isSimple = false, bool isMain = false)
    {
        nickname = args["nickname"].str;
        totalBonus = args["totalBonus"].f;
        playerCount = (int)args["playerCount"].i;
        totalExchangeGiftRoll = args["totalExchangeGiftRoll"].f;

        if (isSimple)
        {
            id = args["id"].str;
            registerTime = args["registerTime"].str;
        }
        else
        {
            if (!isMain)
            {
                registerTime = args["registerTime"].str;
                totalHaveApplyGiftRoll = args["totalHaveApplyGiftRoll"].f;
                totalNotApplyGiftRoll = args["totalNotApplyGiftRoll"].f;
            }

            phone = args["phone"].str;
            starLevel = (int)args["starLevel"].i;
            totalRecharge = args["totalRecharge"].f;
            totalCardRecharge = args["totalCardRecharge"].f;
            totalGameRecharge = args["totalGameRecharge"].f;
            totalCardBonus = args["totalCardBonus"].f;
            totalGameBonus = args["totalGameBonus"].f;
            totalHaveApplyBonus = args["totalHaveApplyBonus"].f;
            totalNotApplyBonus = args["totalNotApplyBonus"].f;
            totalHaveExchangeGiftRoll = args["totalHaveExchangeGiftRoll"].f;
            totalNotExchangeGiftRoll = args["totalNotExchangeGiftRoll"].f;
            canTakenOutCashCount = args["canTakenOutCashCount"].f;
            canExchangeCount = args["canExchangeCount"].f;
            auditsType = (AuditsType)args["auditsType"].i;
        }
    }

    public int starLevel;                                  // 星级
    public int playerCount;                             // 玩家总数
    public int roomcardCount;                        // 持有房卡数

    public float totalRecharge;                        // 充值总额
    public float totalCardRecharge;                 // 房卡充值总额
    public float totalGameRecharge;               // 钻石充值总额

    public float totalBonus;                             // 佣金总额
    public float totalCardBonus;                      // 房卡佣金总额
    public float totalGameBonus;                    // 钻石佣金总额
    public float totalHaveApplyBonus;             // 已申请佣金总额
    public float totalNotApplyBonus;               // 未申请佣金总额

    public float totalExchangeGiftRoll;             // 兑换礼品卷总数
    public float totalHaveExchangeGiftRoll;     // 已兑换礼品卷总数
    public float totalNotExchangeGiftRoll;       // 未兑换礼品卷总数
    public float totalHaveApplyGiftRoll;           // 已申请礼品卷总额
    public float totalNotApplyGiftRoll;             // 未申请礼品卷总额

    public float exchangedCount;                    // 历史兑换数
    public float canTakenOutCashCount;        // 可提现金额
    public float canExchangeCount;                // 可兑换礼品卷

    public AuditsType auditsType;                    // 审核状态
}
