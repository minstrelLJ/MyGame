using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using AsyncSocket;

namespace GameServer
{
    public class PlayerInfo
    {
        public int userId { get; set; }
        public Role role { get; set; }
        public AsyncSocketUserToken client;
    }
}
