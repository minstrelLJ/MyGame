using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncSocket
{
    public class ServerData : DataBase
    {
        public ServerData(string msg)
        {
            try
            {
                list = new List<string>();
                string[] msgs = msg.Split('^');
                for (int i = 0; i < msgs.Length; i++)
                {
                    switch (i)
                    {
                        case 0: cmd = (CMD)int.Parse(msgs[0]); break;
                        case 1: 
                            error = int.Parse(msgs[1]);
                            if (error > 0)
                                return;
                            break;
                        default: list.Add(msgs[i]); break;
                    }
                }
            }
            catch (Exception e)
            {
                Global.Logger.Error(e.Message);
            }
        }
    }
}
