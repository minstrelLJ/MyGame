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
        BattleManager bm;

        public void Init()
        {
            bm = new BattleManager();
        }

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
                case CMD.SelectRole: RSelectRole(data); break;
                case CMD.EnterBattleScene: REnterBattleScene(data); break;
                case CMD.EnterNewRole: REnterNewRole(data); break;

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
                SSelectRole();
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
        private void RSelectRole(DataBase data)
        {
            if (data.error > 0)
            {
                Console.WriteLine("ERR: " + data.error);
                return;
            }
            SEnterBattleScene(101);
        }
        private void REnterBattleScene(DataBase data)
        {
            if (data.error > 0)
            {
                Console.WriteLine("ERR: " + data.error);
                return;
            }

            BattleManager.Instance.serverId = int.Parse(data.list[0]);
        }
        private void REnterNewRole(DataBase data)
        {
            if (data.error > 0)
            {
                Console.WriteLine("ERR: " + data.error);
                return;
            }

            string roleJson = data.list[0];
            BattleManager.Instance.AddEntity(roleJson);

            BattleManager.Instance.Attack(1);
        }

        public void SLogin()
        {
            DataBase db = DataPool.Instance.Pop(CMD.Login);
            db.Add("test001");
            db.Add("123");
            SendMessage(db);

            Console.WriteLine("SLogin");
        }
        private void SGetRole()
        {
            DataBase db = DataPool.Instance.Pop(CMD.GetRole);
            db.Add(DataManager.Instance.userId);
            SendMessage(db);

            Console.WriteLine("SGetRole");
        }
        private void SCreateRole()
        {
            DataBase db = DataPool.Instance.Pop(CMD.CreateRole);
            db.Add(DataManager.Instance.userId);
            db.Add("机器人" + DataManager.Instance.userId);
            SendMessage(db);

            Console.WriteLine("SCreateRole");
        }
        private void SEnterGame()
        {
            DataBase db = DataPool.Instance.Pop(CMD.EnterGame);
            db.Add(DataManager.Instance.userId);
            SendMessage(db);

            Console.WriteLine("SEnterGame");
        }
        private void SSelectRole()
        {
            DataBase db = DataPool.Instance.Pop(CMD.SelectRole);
            db.Add(DataManager.Instance.userId);
            SendMessage(db);

            Console.WriteLine("SelectRole");
        }
        private void SEnterBattleScene(int sceneId)
        {
            DataBase db = DataPool.Instance.Pop(CMD.EnterBattleScene);
            db.Add(DataManager.Instance.userId);
            db.Add(sceneId);
            SendMessage(db);

            Console.WriteLine("StartBattle");
        }
        public void SAttack(int entityId)
        {
            DataBase db = DataPool.Instance.Pop(CMD.Attack);
            db.Add(DataManager.Instance.userId);
            db.Add(BattleManager.Instance.serverId);
            db.Add(entityId);
            SendMessage(db);
        }
    }
}
