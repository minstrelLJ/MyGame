using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using Tools;

namespace GameServer
{
    public class ConfigManager : Singleton<ConfigManager>
    {
        public static string CONFIG_PATH = System.IO.Directory.GetCurrentDirectory() + "/Configs/";

        public Dictionary<int, Role> roleDic = new Dictionary<int, Role>();

        public void Init()
        {
            GetRoles();
        }
        private void GetRoles()
        {
            var roles = FileIO.ReadJson<Role>(CONFIG_PATH + "Role");
            foreach (var item in roles)
            {
                roleDic[item.roleId] = item;
            }
        }

        public Role GetRole(int roleId)
        {
            Role role;
            if (!roleDic.TryGetValue(roleId, out role))
            {
                LogManager.Instance.Logger.Error("没有配置角色 " + roleId);
            }
            return role;
        }
    }
}
