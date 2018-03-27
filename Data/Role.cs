using System;

namespace Data
{
    [Serializable]
    public class Role
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
        public int level { get; set; }
        public long exp { get; set; }

        public float fixedSTR { get; set; }
        public float fixedDEX { get; set; }
        public float fixedMAG { get; set; }
        public float fixedCON { get; set; }

        public float potentialSTR { get; set; }
        public float potentialDEX { get; set; }
        public float potentialMAG { get; set; }
        public float potentialCON { get; set; }

        public Role() { }
        public Role(AsyncSocket.DataBase data)
        {
            roleId = int.Parse(data.list[0]);
            roleName = data.list[1];
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
    }
}
