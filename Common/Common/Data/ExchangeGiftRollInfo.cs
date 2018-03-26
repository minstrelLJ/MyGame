using System;

[Serializable]
public class ExchangeGiftRollInfo 
{
    public ExchangeGiftRollInfo() { }
    public ExchangeGiftRollInfo(JSONObject args, PlayerType playerType)
    {
        switch (playerType)
        {
            case PlayerType.Company:
                id = args["id"].str;
                //playerType = (PlayerType)(int)args["playerType"].i;
                notExchange = args["notExchange"].f;
                haveExchange = args["haveExchange"].f;
                nickname = args["nickname"].str;
                time = args["time"].str;
                break;

            case PlayerType.Business:
                ip = args["ip"].str;
                nickname = args["nickname"].str;
                time = args["time"].str;
                exchangeType = (ExchangeGiftRollType)(int)args["exchangeType"].i;
                haveExchange = args["haveExchange"].f;
                break;

            case PlayerType.Manager:
                break;

            case PlayerType.Player:
                break;
        }

       
    }

    public string id;
    public string ip;
    public string nickname;
    public string time;
    public float notExchange;
    public float haveExchange;
    public PlayerType playerType;
    public ExchangeGiftRollType exchangeType;
}