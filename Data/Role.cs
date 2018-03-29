using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class Role : Entity
    {
        public float fixedSTR { get; set; }
        public float fixedDEX { get; set; }
        public float fixedMAG { get; set; }
        public float fixedCON { get; set; }

        public float potentialSTR { get; set; }
        public float potentialDEX { get; set; }
        public float potentialMAG { get; set; }
        public float potentialCON { get; set; }

        public Role(AsyncSocket.DataBase data)
        {
            id = int.Parse(data.list[0]);
            name = data.list[1];
            level = int.Parse(data.list[2]);
            exp = int.Parse(data.list[3]);

            fixedSTR = float.Parse(data.list[4]);
            fixedDEX = float.Parse(data.list[5]);
            fixedMAG = float.Parse(data.list[6]);
            fixedCON = float.Parse(data.list[7]);

            potentialSTR = float.Parse(data.list[8]);
            potentialDEX = float.Parse(data.list[9]);
            potentialMAG = float.Parse(data.list[10]);
            potentialCON = float.Parse(data.list[11]);
        }

        public int GetHp()
        {
            switch (type)
            {
                case 1: return (int)(fixedCON * 10);
                case 2: return macHp;
                default: return -1;
            }

        }
        public int GetAtk()
        {
            switch (type)
            {
                case 1: return (int)(fixedSTR + 0.5) + (int)(fixedMAG + 0.5);
                case 2: return atk;
                default: return -1;
            }
        }
        public int GetDef()
        {
            switch (type)
            {
                case 1: return (int)(fixedDEX + 0.3);
                case 2: return def;
                default: return -1;
            }
        }
    }
}
