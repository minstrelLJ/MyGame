
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tools;
using Data;

namespace GameServer
{
    public class DataManager : Singleton<DataManager>
    {
        private Settings settings;
        public Dictionary<int, Role> roleDic = new Dictionary<int, Role>();

        #region 初始化

        public void Init()
        {
            ReadSettings();
        }

        // 读取设定信息
        private void ReadSettings()
        {
            List<Settings> list = MySqlTemplate.SELECT<Settings>(new string[] { "setting" }, new string[] { "*" });
            settings = list[0];
        }

        #endregion 初始化

        // 添加用户
        public int AddNewUser(string accountNumber, string password)
        {
            try
            {
                User user = new User();
                user.userId = ++settings.nextUserId;
                user.accountNumber = accountNumber;
                user.password = password;

                MySqlTemplate.INSERT("user", new string[] { "userid", "accountNumber", "password" },
                    new string[] { settings.nextUserId.ToString(), accountNumber, password });
                MySqlTemplate.UPDATE("setting", new string[] { "nextUserId" }, new object[] { settings.nextUserId });

                Program.logger.Debug("add new user " + settings.nextUserId);
                return 0;
            }
            catch (Exception e)
            {
                Program.logger.Error(e.Message + e.TargetSite);
                return 1000;
            }
        }
        // 检测用户存在或密码正确与否
        public int CheckUser(string accountNumber, string password)
        {
            string where = string.Format("accountNumber = '{0}'", accountNumber);
            var users = MySqlTemplate.SELECT<User>(new string[] { "user" }, new string[] { "*" }, where);
            foreach (var item in users)
            {
                if (item.password == password)
                    return 0;
            }
            return 1002;
        }
        // 读取用户
        public User ReadUser(string accountNumber)
        {
            string where = string.Format("accountNumber = '{0}'", accountNumber);
            var users = MySqlTemplate.SELECT<User>(new string[] { "user" }, new string[] { "*" }, where);
            foreach (var item in users)
            {
                return item;
            }
            return null;
        }
        public User ReadUser(int userId)
        {
            string where = string.Format("userId = '{0}'", userId);
            var users = MySqlTemplate.SELECT<User>(new string[] { "user" }, new string[] { "*" }, where);
            foreach (var item in users)
            {
                return item;
            }
            return null;
        }

        // 读取角色
        public Role ReadRole(int roleId)
        {
            Role role;
            if (!roleDic.TryGetValue(roleId, out role))
            {
                string where = string.Format("roleId = {0}", roleId);
                var roles = MySqlTemplate.SELECT<Role>(new string[] { "role" }, new string[] { "*" }, where);
                if (roles.Count > 0) role = roles[0];

                if (role != null)
                    roleDic[role.roleId] = role;
            }
            return role;
        }
        public bool RoleIsExisting(string roleName)
        {
            string where = string.Format("roleName = '{0}'", roleName);
            var roles = MySqlTemplate.SELECT<Role>(new string[] { "role" }, new string[] { "*" }, where);
            if (roles.Count < 1)
                return false;
            
            return roles[0] != null;
        }
        // 添加角色
        public int AddNewRole(int userId, Role role)
        {
            User user = DataManager.Instance.ReadUser(userId);
            if (user != null)
            {
                role.roleId = ++settings.nextRoleId;

                MySqlTemplate.UPDATE("user", new string[] { "roleId" }, new object[] { role.roleId });
                MySqlTemplate.UPDATE("setting", new string[] { "nextRoleId" }, new object[] { settings.nextRoleId });
                //MySqlTemplate.INSERT("role", new string[] { "roleId", "roleName", "level", "exp", "STR", "DEX", "INT", "CON", 
                //     "potentialSTR", "potentialDEX", "potentialINT", "potentialCON" }, new object[] {
                //     role.roleId, role.roleName, role.level, role.exp, role.fixedSTR,
                //     role.fixedDEX, role.fixedMAG, role.fixedCON, role.potentialSTR, role.potentialDEX,
                //     role.potentialMAG, role.potentialCON });

                MySqlTemplate.INSERT("role", new string[] { "roleId", "roleName", "level", "exp", "fixedSTR" }, new object[] {
                     role.roleId, role.roleName, role.level, role.exp, role.fixedSTR });

                return 0;
            }
            return 1;
        }

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
    }
}
