using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using AsyncSocket;
using Tools;

namespace GameServer
{
    public class BattleServer : ServerBase
    {
        public int serverId;
        public int nextEntityId = 1;

        private List<PlayerInfo> playerList = new List<PlayerInfo>();
        private List<Entity> roleList = new List<Entity>();

        public void Init(int levelId)
        {
            LevelInfo levelConfig = ConfigManager.Instance.GetCheckPoint(levelId);

            string[] monsters = levelConfig.monsters.Split('|');
            if (monsters.Length > 0)
            {
                foreach (var item in monsters)
                {
                    int monsterId = int.Parse(item);
                    AddMonster(monsterId);
                }
            }

            ServerManager.Instance.AddPlayingServer(this);
        }
        public void Stop()
        {
            nextEntityId = 1;
            ServerManager.Instance.AddPlayingServer(this);
        }
        public void Update()
        {
            
        }

        // 玩家加入
        public void EnterPlayer(PlayerInfo player)
        {
            string roleJson = FileIO.ToJson(player.role);
            playerList.Add(player);
            roleList.Add(player.role);

            // 给加入的玩家发送当前活物信息
            DataBase db = DataPool.Instance.Pop(CMD.EnterNewRole, 0);
            db.Add(FileIO.ToJson(roleList));
            player.client.SendMessage(db);

            // 给其余玩家发送活物信息
            foreach (var item in playerList)
            {
                if (item.userId == player.userId)
                    continue;

                db = DataPool.Instance.Pop(CMD.EnterNewRole, 0);
                db.Add(FileIO.ToJson(roleJson));
                item.client.SendMessage(db);
            }
        }
        // 攻击
        public void Attack(int entityId, int targetId)
        {
            Entity role = GetRole(entityId);
            Entity target = GetRole(targetId);

            if (role == null || target == null)
                return;
            

        }

        // 添加怪物
        private void AddMonster(int id)
        {
            Monster m = ConfigManager.Instance.GetMonster(id);
            if (m != null)
            {
                Entity r = new Entity(m);
                r.id = ++nextEntityId;
                roleList.Add(r);
            }
        }

        private Entity GetRole(int id)
        {
            foreach (var item in roleList)
            {
                if (item.id == id)
                    return item;
            }
            return null;
        }
    }
}
