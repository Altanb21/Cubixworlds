
using System;
using System.Threading;

namespace Shittopia_Server
{
    internal class Program
    {
        private static bool isRunning;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(Program.OnApplicationExit);
            Console.Title = "Game Server";
            Program.isRunning = true;
            new Thread(new ThreadStart(Program.MainThread)).Start();
            new Thread(new ThreadStart(Security.UpdateLoadBalancer)).Start();
            Server.Start(1000, 1000, 26950);
        }

        private static void MainThread()
        {
            Server.Log(string.Format("Main thread started. Running at {0} ticks per second.", (object)30), ConsoleColor.Green);
            DateTime dateTime = DateTime.Now;
            while (Program.isRunning)
            {
                while (dateTime < DateTime.Now)
                {
                    try
                    {
                        GameLogic.Update();
                    }
                    catch (Exception ex)
                    {
                        Server.Log("Error:", ex.Message);
                        Security.LogError(ex.ToString());
                    }
                    dateTime = dateTime.AddMilliseconds(33.3333320617676);
                    if (dateTime > DateTime.Now)
                    {
                        try
                        {
                            Thread.Sleep(dateTime - DateTime.Now);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            foreach (World world in Server.worlds.Values)
                world.SaveWorld();
            foreach (Client client in Server.clients.Values)
            {
                if (client.player != null)
                    client.account.Save();
            }
        }
    }
}
