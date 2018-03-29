using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsyncSocket;
using Data;
using Tools;

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
                case CMD.CreateRole: CreateRole(client, data); break;
                case CMD.SelectRole: SelectRole(client, data); break;
                case CMD.EnterBattleScene: EnterBattleScene(client, data); break;
                case CMD.Attack: Attack(client, data); break;

                default: LogManager.Instance.Logger.Error("未知 CMD " + data.cmd); break;
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
            if (error == 0) db.Add(user.userId);
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
            client.SendMessage(db);
        }
        private static void CreateRole(AsyncSocketUserToken client, DataBase data)
        {
            int userId = int.Parse(data.list[0]);
            string roleName = data.list[1];

            DataBase db;
            if (string.IsNullOrEmpty(roleName))
            {
                db = DataPool.Instance.Pop(data.cmd, 1004);
                client.SendMessage(db);
                return;
            }

            db = DataPool.Instance.Pop(data.cmd, 0);
            if (!DataManager.Instance.RoleIsExisting(roleName))
            {
                Role role = ConfigManager.Instance.GetRole(1000);
                role.name = roleName;

                int error = DataManager.Instance.AddNewRole(userId, role);
                db = DataPool.Instance.Pop(data.cmd, error);
            }
            else
            {
                db.error = 1003;
            }
            client.SendMessage(db);
        }
        private static void GetRole(AsyncSocketUserToken client, DataBase data)
        {
            int userId = int.Parse(data.list[0]);
            User user = DataManager.Instance.ReadUser(userId);
            Role role = DataManager.Instance.ReadRole(user.roleId);

            DataBase db;
            if (role != null)
            {
                db = DataPool.Instance.Pop(data.cmd, 0);
                db.Add(role.id);
                db.Add(role.name);
                db.Add(role.level);
                db.Add(role.exp);
                db.Add(role.fixedSTR);
                db.Add(role.fixedDEX);
                db.Add(role.fixedMAG);
                db.Add(role.fixedCON);
                db.Add(role.potentialSTR);
                db.Add(role.potentialDEX);
                db.Add(role.potentialMAG);
                db.Add(role.potentialCON);
                client.SendMessage(db);
            }
            else
            {
                db = DataPool.Instance.Pop(data.cmd, 1);
                client.SendMessage(db);
            }
        }
        private static void SelectRole(AsyncSocketUserToken client, DataBase data)
        {
            int userId = int.Parse(data.list[0]);
            User user = DataManager.Instance.ReadUser(userId);
            Entity role = DataManager.Instance.ReadRole(user.roleId);
            PlayerInfo player = new PlayerInfo();
            player.userId = userId;
            player.role = role;
            player.client = client;

            ServerManager.Instance.EnterNewPlayer(player);

            DataBase db = DataPool.Instance.Pop(data.cmd, 0);
            client.SendMessage(db);
        }
        private static void EnterBattleScene(AsyncSocketUserToken client, DataBase data)
        {
            int userId = int.Parse(data.list[0]);
            int levelId = int.Parse(data.list[1]);
            User user = DataManager.Instance.ReadUser(userId);
            Entity role = DataManager.Instance.ReadRole(user.roleId);
            PlayerInfo player = new PlayerInfo();
            player.userId = userId;
            player.role = role;
            player.client = client;

            BattleServer bs = ServerManager.Instance.CreateBattleScene(levelId);
            bs.EnterPlayer(player);

            DataBase db = DataPool.Instance.Pop(CMD.EnterBattleScene, 0);
            db.Add(bs.serverId);
            client.SendMessage(db);
        }
        private static void Attack(AsyncSocketUserToken client, DataBase data) 
        {
            int userId = int.Parse(data.list[0]);
            int serverId = int.Parse(data.list[1]);
            int entityId = int.Parse(data.list[2]);

            BattleServer bs = ServerManager.Instance.GetServer(serverId);
            if (bs != null)
            {
                bs.Attack(userId, entityId);
            }
        }
    }
}
