
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;

namespace GameServer
{
    public class BattleServer : ServerBase
    {
        public int serverId;

        public void EnterPlayer(PlayerInfo player)
        {

        }

        public void Start()
        {
            ServerManager.Instance.AddPlayingServer(this);
        }
        public void Update()
        {

        }
    }
}
