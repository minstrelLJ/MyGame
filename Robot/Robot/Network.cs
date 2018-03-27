using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsyncSocket;
using Tools;
using Data;

namespace Robot
{
    public class Network : Singleton<Network>
    {
        SocketClient Socket;

        public void Connect()
        {
            Socket = new SocketClient();
            Socket.Connect("127.0.0.1", 9999);
            Socket.Receive = ReceiveData;
        }

        private void SendMessage(DataBase data)
        {
            try
            {
                Socket.SendMessage(data);
            }
            catch (Exception e) { Console.WriteLine(e.Message + e.TargetSite); }
        }
        private void ReceiveData(DataBase data)
        {
            switch (data.cmd)
            {
                case CMD.Login: RLogin(data); break;
                case CMD.EnterGame: REnterGame(data); break;
                case CMD.GetRole: RGetRole(data); break;
                case CMD.CreateRole: RCreateRole(data); break;
                case CMD.SelectRole: SelectRole(data); break;

                default: Console.WriteLine("未知 CMD " + data.cmd); break;
            }
        }

        private void RLogin(DataBase data)
        {
            if (data.error > 0)
            {
                Console.WriteLine("ERR: " + data.error);
                return;
            }

            DataManager.Instance.userId = int.Parse(data.list[0]);
            SEnterGame();
        }
        private void RGetRole(DataBase data)
        {
            if (data.error > 0)
            {
                SCreateRole();
            }
            else
            {
                DataManager.Instance.role = new Role(data);
                SelectRole();
            }
        }
        private void RCreateRole(DataBase data)
        {
            if (data.error > 0)
            {
                Console.WriteLine("ERR: " + data.error);
                return;
            }
            SGetRole();
        }
        private void REnterGame(DataBase data)
        {
            if (data.error > 0)
            {
                Console.WriteLine("ERR: " + data.error);
                return;
            }
            SGetRole();
        }
        private void SelectRole(DataBase data)
        {
            if (data.error > 0)
            {
                Console.WriteLine("ERR: " + data.error);
                return;
            }
        }

        public void SLogin()
        {
            DataBase db = DataPool.Instance.Pop(CMD.Login);
            db.Add("test001");
            db.Add("123");
            SendMessage(db);
        }
        private void SGetRole()
        {
            DataBase db = DataPool.Instance.Pop(CMD.GetRole);
            db.Add(DataManager.Instance.userId);
            SendMessage(db);
        }
        private void SCreateRole()
        {
            DataBase db = DataPool.Instance.Pop(CMD.CreateRole);
            db.Add(DataManager.Instance.userId);
            db.Add("机器人" + DataManager.Instance.userId);
            SendMessage(db);
        }
        private void SEnterGame()
        {
            DataBase db = DataPool.Instance.Pop(CMD.EnterGame);
            db.Add(DataManager.Instance.userId);
            SendMessage(db);
        }
        private void SelectRole()
        {
            DataBase db = DataPool.Instance.Pop(CMD.SelectRole);
            db.Add(DataManager.Instance.userId);
            SendMessage(db);
        }
    }
}
