using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AsyncSocket
{
    public class BasicFunc
    {
        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {
                try
                {
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    inUse = false;
                }
                catch
                {
                    inUse = true;
                }
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;
        }

        public static string MD5String(string value)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = Encoding.Default.GetBytes(value);
            byte[] md5Data = md5.ComputeHash(data);
            md5.Clear();
            string result = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                result += md5Data[i].ToString("x").PadLeft(2, '0');
            }
            return result;
        }

        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter(); 
                formatter.Serialize(ms, obj); 
                return ms.GetBuffer();
            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static object BytesToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter(); 
                return formatter.Deserialize(ms);
            }
        }
    }
}
