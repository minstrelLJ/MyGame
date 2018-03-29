using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;
using Data;
using AsyncSocket;

namespace GameServer
{
    public class ServerManager : Singleton<ServerManager>
    {
        private ServerPool serverPool;

        Dictionary<int, PlayerInfo> playerDic;
        Dictionary<int, Entity> roleDic;
        List<BattleServer> playingServer;

        public void Init()
        {
            playerDic = new Dictionary<int, PlayerInfo>();
            roleDic = new Dictionary<int, Entity>();
            playingServer = new List<BattleServer>();

            serverPool = new ServerPool(100);
            for (int i = 0; i < 100; i++)
            {
                BattleServer bs = new BattleServer();
                bs.serverId = i;

                serverPool.Push(bs);
            }

            Task t = new Task(n => Update(), 10);
            t.Start();
        }
        private void Update()
        {
            while (true)
            {
                for (int i = 0; i < playingServer.Count; i++)
                {
                    if (playingServer[i] != null)
                    {
                        playingServer[i].Update();
                    }
                }
            }
        }

        public void EnterNewPlayer(PlayerInfo player)
        {
            playerDic[player.userId] = player;
            roleDic[player.role.id] = player.role;
        }

        public BattleServer CreateBattleScene(int levelId)
        {
            BattleServer bs = serverPool.Pop();
            bs.Init(levelId);
            return bs;
        }

        public void AddPlayingServer(BattleServer bs)
        {
            playingServer.Add(bs);
        }
        public void RemovePlayingServer(BattleServer bs)
        {
            playingServer.Remove(bs);
        }

        public BattleServer GetServer(int serverId)
        {
            foreach (var item in playingServer)
            {
                if (item.serverId == serverId)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
