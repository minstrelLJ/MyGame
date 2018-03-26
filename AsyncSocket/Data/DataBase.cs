using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncSocket
{
    public class DataBase
    {
        public CMD cmd;
        public int error;
        public List<string> list;

        public DataBase()
        {
            list = new List<string>();
        }

        public void Add(string v)
        {
            list.Add(v);
        }
        public void Add(int v)
        {
            list.Add(v.ToString());
        }
        public void Add(float v)
        {
            list.Add(v.ToString());
        }
    }
}
