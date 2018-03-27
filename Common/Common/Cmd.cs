
namespace Common
{
    /// <summary>
    /// 协议
    /// </summary>
    public enum MessageType : int
    {
        none = 0,
        ApplyBonusInfos = 101,                                    // 申请佣金列表
        ApplyGiftRollInfos = 103,                                  // 申请礼品卷列表
        AuditsInfo = 104,                                              // 审核列表
        rechargeOrExchangeGiftRollInfos = 105,            // 获取购买房卡与兑换记录
        ExchangeGiftRollInfo = 106,                              // 兑换礼品卷记录
        TakenOutCash = 108,                                        // 提现记录
        login = 109,                                                       // 用户登录
        MainPlayerInfo = 110,                                       // 获取基本信息

        ManagerList,                                            // 获取市场经理列表
        CooperativeBusinessList,                                            // 获取合作商家列表
        playerList,                                            // 获取玩家列表
        ManagerInfo,                                            // 获取市场经理列表
        CooperativeBusinessInfo,                                            // 获取合作商家列表
        playerInfo,                                            // 获取玩家列表
        SetPersonalInfo,                            // 修改个人信息
        AddManager,                             // 添加市场经理
        SetManagerInfo,                         // 修改经理基本信息
        SetCooperativeBusinessInfo,             // 修改合作商家基本信息
        SetPlayerInfo,                                  // 修改玩家基本信息
        AuditsResult,                                   // 审核结果










        RechargeInfo = 107,                                          // 充值记录

    }
}