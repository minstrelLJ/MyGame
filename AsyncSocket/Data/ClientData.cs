using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncSocket
{
    public class ClientData : DataBase
    {
        public ClientData(string msg)
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
