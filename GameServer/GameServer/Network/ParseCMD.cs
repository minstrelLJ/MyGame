using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsyncSocket;
using Data;

namespace GameServer
{
    public class ParseCMD
    {
        public static void Parse(AsyncSocketUserToken client, ClientData data)
        {
            switch (data.cmd)
            {
                case CMD.Heartbeat: Heartbeat(client, data); break;
                case CMD.Login: Login(client, data); break;
                case CMD.Register: Register(client, data); break;
                case CMD.EnterGame: EnterGame(client, data); break;
                case CMD.GetRole: GetRole(client, data); break;
                default:
                    break;
            }
        }

        private static void Heartbeat(AsyncSocketUserToken client, DataBase data)
        {
            DataBase db = DataPool.Instance.Pop(data.cmd, 0);
            client.SendMessage(db);
        }
        private static void Login(AsyncSocketUserToken client, DataBase data)
        {
            string acc = data.list[0];
            string pw = data.list[1];
            User user = DataManager.Instance.ReadUser(acc);
            int error = DataManager.Instance.CheckUser(acc, pw);

            DataBase db = DataPool.Instance.Pop(data.cmd, error);
            db.Add(user.userId);
            client.SendMessage(db);
        }
        private static void Register(AsyncSocketUserToken client, DataBase data)
        {
            string acc = data.list[0];
            string pw = data.list[1];

            DataBase db = DataPool.Instance.Pop(data.cmd, DataManager.Instance.AddNewUser(acc, pw));
            client.SendMessage(db);
        }
        private static void EnterGame(AsyncSocketUserToken client, DataBase data)
        {
            int userId = int.Parse(data.list[0]);
            User user = DataManager.Instance.ReadUser(userId);

            DataBase db = DataPool.Instance.Pop(data.cmd, 0);
            db.Add(user.roleId);
            client.SendMessage(db);
        }
        private static void CreateRole(AsyncSocketUserToken client, DataBase data)
        {
            Role role = new Role();

            DataBase db = DataPool.Instance.Pop(data.cmd, 0);
            client.SendMessage(db);
        }
        private static void GetRole(AsyncSocketUserToken client, DataBase data)
        {
            int roleId = int.Parse(data.list[0]);
            Role role = DataManager.Instance.ReadRole(roleId);

            DataBase db;
            if (role == null)
                db = DataPool.Instance.Pop(data.cmd, 1);

            db = DataPool.Instance.Pop(data.cmd, 0);
            db.Add(role.roleId);
            db.Add(role.name);
            db.Add(role.level);
            db.Add(role.exp);
            db.Add(role.STR);
            db.Add(role.DEX);
            db.Add(role.INT);
            db.Add(role.CON);
            db.Add(role.potentialSTR);
            db.Add(role.potentialDEX);
            db.Add(role.potentialINT);
            db.Add(role.potentialCON);
            client.SendMessage(db);
        }
    }
}
