using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Cmd : Singleton<Cmd>
    {
        public Action<int> errorCallBack;
        public bool Parse(string str, out JSONObject jsonData)
        {
            jsonData = null;

            try
            {
                jsonData = new JSONObject(str);

                try
                {
                    int errorCode = (int)jsonData["ErrorCode"].i;
                    if (errorCallBack != null) errorCallBack(errorCode);
                    return false;
                }
                catch
                {
                    return true;
                }
            }
            catch
            {
                if (errorCallBack != null) errorCallBack(999);
                return false;
            }
        }

        public void Login(Action<string> ac, string id, string pw)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.login).ToString());
            msg.Add(id);
            msg.Add(pw);
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    if (ac != null)
                    {
                        ac("115");
                    }
                }
            });
        }

        public void GetMainCompanyInfo(string userid, Action<CompanyInfo> ac)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.MainPlayerInfo).ToString());
            msg.Add(userid);
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    CompanyInfo info = new CompanyInfo(jsonData);
                    info.id = userid;
                    ac(info);
                }
            });
        }

        public void GetManagerList(string targetid, Action<ManagerInfo[]> ac)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.ManagerList).ToString());
            msg.Add(targetid);
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    ManagerInfo[] infos = new ManagerInfo[jsonData.list.Count];
                    for (int i = 0; i < jsonData.list.Count; i++)
                    {
                        infos[i] = new ManagerInfo(jsonData[i], true);
                    }

                    ac(infos);
                }
            });
        }
        public void GetBusinessList(string targetid, Action<BusinessInfo[]> ac)
        {
            string args = ResourceManager.Instance.LoadText("Datas/BusinessList");

            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.CooperativeBusinessList).ToString());
            //msg.Add(id);
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
            JSONObject jsonData = null;
            if (Parse(args, out jsonData))
            {
                BusinessInfo[] infos = new BusinessInfo[jsonData.list.Count];
                for (int i = 0; i < jsonData.list.Count; i++)
                {
                    infos[i] = new BusinessInfo(jsonData[i], true);
                }

                ac(infos);
            }
            //});
        }
        public void GetPlayerList(string targetid, Action<PlayerInfo[]> ac)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.playerList).ToString());
            msg.Add(targetid);
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    PlayerInfo[] infos = new PlayerInfo[jsonData.list.Count];
                    for (int i = 0; i < jsonData.list.Count; i++)
                    {
                        infos[i] = new PlayerInfo(jsonData[i], true);
                    }

                    ac(infos);
                }
            });
        }
        public void GetManagerInfo(string targetid, Action<ManagerInfo> ac)
        {
            string args = ResourceManager.Instance.LoadText("Datas/ManagerInfo");

            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.ManagerInfo).ToString());
            //msg.Add(id);
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
            JSONObject jsonData = null;
            if (Parse(args, out jsonData))
            {
                ManagerInfo info = new ManagerInfo(jsonData);
                ac(info);
            }
            //});
        }
        public void GetBusinessInfo(string targetid, Action<BusinessInfo> ac)
        {
            string args = ResourceManager.Instance.LoadText("Datas/BusinessInfo");

            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.MainPlayerInfo).ToString());
            //msg.Add(playerID);
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
            JSONObject jsonData = null;
            if (Parse(args, out jsonData))
            {
                BusinessInfo info = new BusinessInfo(jsonData);
                info.id = targetid;
                ac(info);
            }
            //});
        }
        public void GetPlayerInfo(string targetid, Action<PlayerInfo> ac)
        {
            string args = ResourceManager.Instance.LoadText("Datas/PlayerInfo");

            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.playerInfo).ToString());
            //msg.Add(id);
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
            JSONObject jsonData = null;
            if (Parse(args, out jsonData))
            {
                PlayerInfo info = new PlayerInfo(jsonData);
                info.id = targetid;
                ac(info);
            }
            //});
        }

        public void GetAuditsInfosInfo(string userid, Action<AuditsInfo[]> ac, TimePar par)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.AuditsInfo).ToString());
            msg.Add(userid);
            msg.Add(CommonFunction.GetTimeStr(par.year1, par.month1, par.day1));
            msg.Add(CommonFunction.GetTimeStr(par.year2, par.month2, par.day2));
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    AuditsInfo[] infos = new AuditsInfo[jsonData.list.Count];
                    for (int i = 0; i < jsonData.list.Count; i++)
                    {
                        infos[i] = new AuditsInfo(jsonData[i]);
                    }

                    ac(infos);
                }
            });
        }
        public void GetRechargeOrExchangeInfos(string targetid, Action<RechargeOrExchangeGiftRoll[]> ac, TimePar par)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.rechargeOrExchangeGiftRollInfos).ToString());
            msg.Add(targetid);
            msg.Add(CommonFunction.GetTimeStr(par.year1, par.month1, par.day1));
            msg.Add(CommonFunction.GetTimeStr(par.year2, par.month2, par.day2));
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    RechargeOrExchangeGiftRoll[] infos = new RechargeOrExchangeGiftRoll[jsonData.list.Count];
                    for (int i = 0; i < jsonData.list.Count; i++)
                    {
                        infos[i] = new RechargeOrExchangeGiftRoll(jsonData[i]);
                    }
                    ac(infos);
                }
            });
        }
        public void GetApplyBonusInfos(string targetid, Action<ApplyBonusInfo[]> ac, TimePar par, int dropV = 0)
        {
            string args = ResourceManager.Instance.LoadText("Datas/ApplyBonusList");

            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.ApplyBonusInfos).ToString());
            //msg.Add(CommonFunction.GetTimeStr(par.year1, par.month1, par.day1));
            //msg.Add(CommonFunction.GetTimeStr(par.year2, par.month2, par.day2));
            //msg.Add(dropIndex.ToString());
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
            JSONObject jsonData = null;
            if (Parse(args, out jsonData))
            {
                ApplyBonusInfo[] infos = new ApplyBonusInfo[jsonData.list.Count];
                for (int i = 0; i < jsonData.list.Count; i++)
                {
                    infos[i] = new ApplyBonusInfo(jsonData[i]);
                }

                ac(infos);
            }
            //});
        }
        public void GetApplyGiftRollInfos(string targetid, Action<ApplyGiftRollInfo[]> ac, TimePar par, int dropV = 0)
        {
            string args = ResourceManager.Instance.LoadText("Datas/ApplyGiftRollList");

            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.ApplyGiftRollInfos).ToString());
            //msg.Add(CommonFunction.GetTimeStr(par.year1, par.month1, par.day1));
            //msg.Add(CommonFunction.GetTimeStr(par.year2, par.month2, par.day2));
            //msg.Add(dropIndex.ToString());
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
            JSONObject jsonData = null;
            if (Parse(args, out jsonData))
            {
                ApplyGiftRollInfo[] infos = new ApplyGiftRollInfo[jsonData.list.Count];
                for (int i = 0; i < jsonData.list.Count; i++)
                {
                    infos[i] = new ApplyGiftRollInfo(jsonData[i]);
                }

                ac(infos);
            }
            // });
        }
        public void GetRechargeInfos(string targetid, Action<RechargeInfo[]> ac, TimePar par, int dropV = 0)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.RechargeInfo).ToString());
            msg.Add(targetid);
            msg.Add(CommonFunction.GetTimeStr(par.year1, par.month1, par.day1));
            msg.Add(CommonFunction.GetTimeStr(par.year2, par.month2, par.day2));
            msg.Add(dropV.ToString());
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    RechargeInfo[] infos = new RechargeInfo[jsonData.list.Count];
                    for (int i = 0; i < jsonData.list.Count; i++)
                    {
                        infos[i] = new RechargeInfo(jsonData[i]);
                    }
                    ac(infos);
                }
            });
        }
        public void GetTakenOutCashInfos(string targetid, Action<TakenOutCashInfo[]> ac, TimePar par, int dropV = 0)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.TakenOutCash).ToString());
            msg.Add(targetid);
            msg.Add(CommonFunction.GetTimeStr(par.year1, par.month1, par.day1));
            msg.Add(CommonFunction.GetTimeStr(par.year2, par.month2, par.day2));
            msg.Add(dropV.ToString());
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    TakenOutCashInfo[] infos = new TakenOutCashInfo[jsonData.list.Count];
                    for (int i = 0; i < jsonData.list.Count; i++)
                    {
                        infos[i] = new TakenOutCashInfo(jsonData[i]);
                    }
                    ac(infos);
                }
            });
        }
        public void GetExchangeGiftRollInfos(string targetid, Action<ExchangeGiftRollInfo[]> ac, TimePar par, int dropV = 0)
        {
            List<string> msg = new List<string>();
            msg.Add(((int)MessageType.ExchangeGiftRollInfo).ToString());
            msg.Add(targetid);
            msg.Add(CommonFunction.GetTimeStr(par.year1, par.month1, par.day1));
            msg.Add(CommonFunction.GetTimeStr(par.year2, par.month2, par.day2));
            msg.Add(dropV.ToString());
            HttpManager.Instance().SendHttpMessage(msg, (args) =>
            {
                JSONObject jsonData = null;
                if (Parse(args, out jsonData))
                {
                    ExchangeGiftRollInfo[] infos = new ExchangeGiftRollInfo[jsonData.list.Count];
                    for (int i = 0; i < jsonData.list.Count; i++)
                    {
                        infos[i] = new ExchangeGiftRollInfo(jsonData[i]);
                    }
                    ac(infos);
                }
            });
        }

        public void AuditsResult(string targetid, Action ac, int type)
        {
            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.AuditsResult).ToString());
            //msg.Add(id);
            //msg.Add(type.ToString());
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
            ac();
            //});
        }
        public void AddManager(string userid, Action ac, string targetid, string nickname, string pw1, string pw2, string phone, int starLv)
        {
            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.AddManager).ToString());
            //msg.Add(userid);
            //msg.Add(targetid);
            //msg.Add(nickname);
            //msg.Add(phone);
            //msg.Add(pw1);
            //msg.Add(pw2);
            //msg.Add(phone);
            //msg.Add(starLv.ToString());
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
                ac();
            //});
        }
        public void SetCompanyInfo(string userid, Action ac, string nickname, string phone, string pw1, string pw2)
        {
            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.SetPersonalInfo).ToString());
            //msg.Add(userid);
            //msg.Add(nickname);
            //msg.Add(phone);
            //msg.Add(pw1);
            //msg.Add(pw2);
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
                ac();
            //});
        }
        public void SetManagerInfo(string id, Action ac, int starLv, string playername, int sex, string age, string remarks)
        {
            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.SetManagerInfo).ToString());
            //msg.Add(id);
            //msg.Add(starLv.ToString());
            //msg.Add(playername);
            //msg.Add(sex.ToString());
            //msg.Add(age);
            //msg.Add(remarks);
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
                ac();
            //});
        }
        public void SetBusinessInfo(string id, Action ac, int starLv, string playername, int sex, string age, string shopName, string shopAddress, string remarks, string imageName)
        {
            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.SetManagerInfo).ToString());
            //msg.Add(id);
            //msg.Add(starLv.ToString());
            //msg.Add(playername);
            //msg.Add(sex.ToString());
            //msg.Add(age);
            //msg.Add(shopName);
            //msg.Add(shopAddress);
            //msg.Add(remarks);
            //msg.Add(imageName);
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
                ac();
            //});
        }
        public void SetPlayerInfo(string id, Action ac, string playername, int sex, string age, string address, string remarks)
        {
            //List<string> msg = new List<string>();
            //msg.Add(((int)MessageType.SetManagerInfo).ToString());
            //msg.Add(id);
            //msg.Add(playername);
            //msg.Add(sex.ToString());
            //msg.Add(age);
            //msg.Add(address);
            //msg.Add(remarks);
            //HttpManager.Instance().SendHttpMessage(msg, (args) =>
            //{
                ac();
            //});
        }
    }

    public class TimePar
    {
        public int toggleValue;
        public int year1;
        public int year2;
        public int month1;
        public int month2;
        public int day1;
        public int day2;

        public TimePar()
        {
            toggleValue = 1;
            year1 = DateTime.Now.Year;
            year2 = DateTime.Now.Year;
            month1 = DateTime.Now.Month;
            month2 = DateTime.Now.Month;
            day1 = DateTime.Now.Day;
            day2 = DateTime.Now.Day;
        }
    }
}
