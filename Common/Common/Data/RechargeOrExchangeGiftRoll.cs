using System;

[Serializable]
public class RechargeOrExchangeGiftRoll
{
    public RechargeOrExchangeGiftRoll() { }
    public RechargeOrExchangeGiftRoll(JSONObject args)
    {
        type = (RechargeOrExchangeGiftRoll.Type)(int)args["type"].i;
        count = (int)args["count"].i;
        price = args["price"].f;
        time = args["time"].str;
    }

    /// <summary>
    /// 购买， 兑换时间
    /// </summary>
    public string time;

    /// <summary>
    /// 购买，兑换数量
    /// </summary>
    public int count;

    /// <summary>
    /// 购买，兑换价值
    /// </summary>
    public float price;

    /// <summary>
    /// 购买，兑换类型
    /// </summary>
    public Type type;

    public enum Type
    {
        Card = 1,
        GiftRoll
    }
}
