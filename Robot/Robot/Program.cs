using System;

namespace Robot
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.Instance.Connect();
            Network.Instance.SLogin();
           

            Console.ReadLine();
        }
    }
}
