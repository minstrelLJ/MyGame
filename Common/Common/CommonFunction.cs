using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Common;

namespace Common
{
    /// <summary>
    /// 公共方法类
    /// </summary>
    public class CommonFunction
    {
        public static string GetTime(string timeStamp, int type = 0)
        {
            //处理字符串,截取括号内的数字
            var strStamp = Regex.Matches(timeStamp, @"(?<=\()((?<gp>\()|(?<-gp>\))|[^()]+)*(?(gp)(?!))").Cast<Match>().Select(t => t.Value).ToArray()[0].ToString();
            //处理字符串获取+号前面的数字
            var str = Convert.ToInt64(strStamp.Substring(0, strStamp.IndexOf("+")));
            long timeTricks = new DateTime(1970, 1, 1).Ticks + str * 10000 + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours * 3600 * (long)10000000;
            switch (type)
            {
                case 1: return new DateTime(timeTricks).ToString("yyyy/MM/dd");
                default: return new DateTime(timeTricks).ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
        public static string GetTimeStr(int year, int month, int day)
        {
            return string.Format("{0}-{1}-{2}", year, month, day);
        }

        public static bool ListIsNull<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return true;
            }
            return false;
        }
        public static bool ListIsNull<T>(T[] list)
        {
            if (list == null || list.Length == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 字符颜色
        /// </summary>
        public static string SetTextColor(string text, string color)
        {
            return string.Format("<color=#{0}>{1}</color>", color, text);
        }
        public static string SetTextColor(int text, string color)
        {
            return SetTextColor(text.ToString(), color);
        }
        public static string SetTextColor(float text, string color)
        {
            return SetTextColor(text.ToString(), color);
        }

        public static string GetManagerLevelName(int level)
        {
            switch (level)
            {
                case 1: return "一星市场经理";
                case 2: return "二星市场经理";
                case 3: return "三星市场经理";
                case 4: return "四星市场经理";
                case 5: return "五星市场经理";
            }
            return null;
        }
        public static string GetBusinessNameByLevel(int level)
        {
            switch (level)
            {
                case 1: return "一星合作商家";
                case 2: return "二星合作商家";
                case 3: return "三星合作商家";
                case 4: return "四星合作商家";
                case 5: return "五星合作商家";
            }
            return null;
        }
        public static List<string> GetStarLevelManagerNames()
        {
            List<string> optionsNmae = new List<string>();
            optionsNmae.Add("一星市场经理");
            optionsNmae.Add("二星市场经理");
            optionsNmae.Add("三星市场经理");
            optionsNmae.Add("四星市场经理");
            optionsNmae.Add("五星市场经理");
            return optionsNmae;
        }
        public static List<string> GetStarLevelBusinessNames()
        {
            List<string> optionsNmae = new List<string>();
            optionsNmae.Add("一星合作商家");
            optionsNmae.Add("二星合作商家");
            optionsNmae.Add("三星合作商家");
            optionsNmae.Add("四星合作商家");
            optionsNmae.Add("五星合作商家");
            return optionsNmae;
        }
        public static string GetRechargeTypeName(RechargeType type)
        {
            switch (type)
            {
                case RechargeType.RoomCard: return "房卡充值";
                case RechargeType.Money: return "钻石充值";

                default: Debug.LogError("未知类型: " + type); return "未知类型";
            }
        }
        public static string GetExchangeGiftRollTypeName(ExchangeGiftRollType type)
        {
            switch (type)
            {
                case ExchangeGiftRollType.GiftRoll: return "礼品卷";

                default: Debug.LogError("未知类型: " + type); return "未知类型";
            }
        }
        public static string GetAuditsByType(AuditsType type)
        {
            string str = "";
            switch (type)
            {
                case AuditsType.Adopt: str = "审核中"; break;
                case AuditsType.NotAdopt: str = "审核未通过"; break;
                case AuditsType.HaveAdopt: str = "审核已通过"; break;
            }
            return str;
        }
    }
}
