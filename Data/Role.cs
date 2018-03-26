using System;

namespace Data
{
    [Serializable]
    public class Role
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
        public int level { get; set; }
        public int exp { get; set; }

        public float STR { get; set; }
        public float DEX { get; set; }
        public float INT { get; set; }
        public float CON { get; set; }

        public float potentialSTR { get; set; }
        public float potentialDEX { get; set; }
        public float potentialINT { get; set; }
        public float potentialCON { get; set; }

        public Role() { }
        public Role(AsyncSocket.DataBase data)
        {
            roleId = int.Parse(data.list[0]);
            roleName = data.list[1];
            level = int.Parse(data.list[2]);
            exp = int.Parse(data.list[3]);

            STR = float.Parse(data.list[4]);
            DEX = float.Parse(data.list[5]);
            INT = float.Parse(data.list[6]);
            CON = float.Parse(data.list[7]);

            potentialSTR = float.Parse(data.list[8]);
            potentialDEX = float.Parse(data.list[9]);
            potentialINT = float.Parse(data.list[10]);
            potentialCON = float.Parse(data.list[11]);
        }
    }
}
