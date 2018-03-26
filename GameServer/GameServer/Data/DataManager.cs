
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
        public Dictionary<string, User> userDicByAcc = new Dictionary<string, User>();
        public Dictionary<int, User> userDicById = new Dictionary<int, User>();
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
                User user;
                if (!userDicByAcc.TryGetValue(accountNumber, out user))
                {
                    user = new User();
                    user.userId = ++settings.nextUserId;
                    user.accountNumber = accountNumber;
                    user.password = password;
                    userDicByAcc[accountNumber] = user;

                    MySqlTemplate.INSERT("user", new string[] { "userid", "accountNumber", "password" },
                        new string[] { settings.nextUserId.ToString(), accountNumber, password });
                    MySqlTemplate.UPDATE("setting", new string[] { "nextUserId" },new string[]{settings.nextUserId.ToString()});

                    Program.logger.Debug("add new user " + settings.nextUserId);
                    return 0;
                }

                return 1001;
            }
            catch (Exception e)
            {
                Program.logger.Error(e.Message);
                return 1000;
            }
        }
        // 检测用户存在或密码正确与否
        public int CheckUser(string accountNumber, string password)
        {
            User user;
            if (userDicByAcc.TryGetValue(accountNumber, out user))
            {
                if (user.password != password)
                    return 1002;

                return 0;
            }
            return 1002;
        }
        // 读取用户
        public User ReadUser(string accountNumber)
        {
            User user;
            userDicByAcc.TryGetValue(accountNumber, out user);

            if (user == null)
            {
                string where = string.Format("accountNumber = '{0}'", accountNumber);
                user = MySqlTemplate.SELECT<User>(new string[] { "user" }, new string[] { "*" }, where)[0];

                if (user != null)
                {
                    userDicByAcc[user.accountNumber] = user;
                    userDicById[user.userId] = user;
                }
            }
            return user;
        }
        public User ReadUser(int userId)
        {
            User user;
            if (!userDicById.TryGetValue(userId, out user))
            {
                string where = string.Format("userId = '{0}'", userId);
                user = MySqlTemplate.SELECT<User>(new string[] { "user" }, new string[] { "*" }, where)[0];

                if (user != null)
                {
                    userDicByAcc[user.accountNumber] = user;
                    userDicById[user.userId] = user;
                }
            }

            return user;
        }
        // 读取角色
        public Role ReadRole(int roleId)
        {
            Role role;
            if (!roleDic.TryGetValue(roleId, out role))
            {
                string where = string.Format("roleId = '{0}'", roleId);
                role = MySqlTemplate.SELECT<Role>(new string[] { "role" }, new string[] { "*" }, where)[0];

                if (role != null)
                    roleDic[role.roleId] = role;
            }
            return role;
        }
    }
}
