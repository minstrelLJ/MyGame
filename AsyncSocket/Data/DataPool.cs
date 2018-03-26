using System;
using System.Collections.Generic;
using Tools;

namespace AsyncSocket
{
    public class DataPool : Singleton<DataPool>
    {
         private Stack<DataBase> pool;

         public DataPool()
        {
            pool = new Stack<DataBase>();
            for (int i = 0; i < 50; i++)
            {
                DataBase db = new DataBase();
                pool.Push(db);
            }
        }

         public void Push(DataBase item)
        {
            if (item == null)
            {
                throw new ArgumentException("Items added to a AsyncSocketUserToken cannot be null");
            }
            lock (pool)
            {
                pool.Push(item);
            }
        }

         public DataBase Pop()
         {
             lock (pool)
             {
                 DataBase db = pool.Pop();
                 if (db == null)
                     db = new DataBase();
                 
                 return db;
             }
         }
         public DataBase Pop(CMD cmd, int error = -1)
         {
             lock (pool)
             {
                 DataBase db = pool.Pop();
                 db.cmd = cmd;
                 db.error = error;
                 return db;
             }
         }

        public int Count
        {
            get { return pool.Count; }
        }
    }
}
