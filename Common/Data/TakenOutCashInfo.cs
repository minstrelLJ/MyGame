using System;

[Serializable]
public class TakenOutCashInfo
{
    public TakenOutCashInfo() { }
    public TakenOutCashInfo(JSONObject args)
    {
        id = (int)args["id"].i;
        nickname = args["nickname"].str;
        time = args["time"].str;
        playerType = (PlayerType)(int)args["playerType"].i;
        notTakenOut = args["notTakenOut"].f;
        haveTakenOut = args["haveTakenOut"].f;
    }

    public string nickname;
    public int id;
    public string time;
    public float notTakenOut;
    public float haveTakenOut;
    public PlayerType playerType;
}