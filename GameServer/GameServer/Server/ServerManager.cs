using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;
using Data;

namespace GameServer
{
    public class ServerManager : Singleton<ServerManager>
    {
        private ServerPool serverPool;

        Dictionary<int, PlayerInfo> playerDic;
        Dictionary<int, Role> roleDic;

        public void Init()
        {
            playerDic = new Dictionary<int, PlayerInfo>();
            roleDic = new Dictionary<int, Role>();

            serverPool = new ServerPool(100);
            for (int i = 0; i < 100; i++)
            {
                BattleServer bs = new BattleServer();
                bs.serverId = i;

                serverPool.Push(bs);
            }
        }

        public void EnterNewPlayer(PlayerInfo player)
        {
            playerDic[player.userId] = player;
            roleDic[player.role.roleId] = player.role;
        }

        public void BattleStart(int userId)
        {
            PlayerInfo player;
            if (playerDic.TryGetValue(userId, out player))
            {
                BattleServer bs = serverPool.Pop();
                bs.EnterPlayer(player);
            }
        }
    }
}
