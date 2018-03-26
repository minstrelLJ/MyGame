using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using AsyncSocket;

public class NetworkManager : UnitySingleton<NetworkManager>
{
     public static int PacketSize = 32 * 1024;
     public static SocketClient Socket;
     public static string host;
     public static int port;

     private Stack<DataBase> serverDatas;
     private Dictionary<CMD, Action<DataBase>> sendDic;

    public void Init()
    {
        host = "127.0.0.1";
        port = 9999;
        Socket = new SocketClient();
        serverDatas = new Stack<DataBase>();
        sendDic = new Dictionary<CMD, Action<DataBase>>();
    }
    public void Update()
    {
        if (serverDatas.Count > 0)
        {
            ReceiveMessage(serverDatas.Pop());
        }
    }
    public void Connect()
    {
        Socket.Connect(host, 9999);
        Socket.Receive = (data) => { serverDatas.Push(data); };

        StartCoroutine(Heartbeat());
    }
    public void ReceiveMessage(DataBase data)
    {
        switch (data.cmd)
        {
            case CMD.Heartbeat:
            case CMD.Login:
            case CMD.Register:
            case CMD.EnterGame:
                Action<DataBase> action = null;
                if (sendDic.TryGetValue(data.cmd, out action))
                {
                    action(data);
                }
                return;
        }

        ParseCMD.Parse(data);
    }
    private void SendMessage(DataBase data, Action<DataBase> ac)
    {
        try
        {
            sendDic[data.cmd] = ac;
            Socket.SendMessage(data);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    private IEnumerator Heartbeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            DataBase db = DataPool.Instance.Pop(CMD.Heartbeat);
            SendMessage(db, (data) => { });
        }
    }
    public void Disconnect()
    {
        Socket.Close();
    }

    #region SendMsg

    public void SendRegister(Action<DataBase> ac, string acc, string pw)
    {
        DataBase db = DataPool.Instance.Pop(CMD.Register);
        db.Add(acc);
        db.Add(pw);
        SendMessage(db, ac);
    }
    public void SendLogin(Action<DataBase> ac, string acc, string pw)
    {
        DataBase db = DataPool.Instance.Pop(CMD.Login);
        db.Add(acc);
        db.Add(pw);
        SendMessage(db, ac);
    }
    public void SendEnter(Action<DataBase> ac, int userId)
    {
        DataBase db = DataPool.Instance.Pop(CMD.EnterGame);
        db.Add(userId);
        SendMessage(db, ac);
    }

    #endregion
}
