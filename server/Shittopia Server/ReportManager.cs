
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Shittopia_Server
{
    internal static class ReportManager
    {
        public static void CreateReport(int _clientID, string _reportText, int _reportTypeID)
        {
            Report report = new Report();
            report.id = Directory.EnumerateFiles(Server.path + "Reports", "*.json", SearchOption.AllDirectories).Select<string, string>((Func<string, string>)(file => file)).Count<string>();
            report.reporterName = Server.clients[_clientID].player.username;
            report.reportText = _reportText;
            report.reportTypeID = _reportTypeID;
            string contents = JsonSerializer.Serialize<Report>(report);
            File.WriteAllText(Server.path + "Reports" + report.id.ToString() + ".json", contents);
            for (int index = 1; index < Server.clients.Count; ++index)
            {
                if (Server.clients[index].player != null && (Server.clients[index].account.accountLevel == 7 || Server.clients[index].account.accountLevel == 5 || Server.clients[index].account.accountLevel == 4))
                {
                    string str = ChatManager.coloredTexts["red"] + "Report from " + ChatManager.coloredTexts["orange"] + Server.clients[_clientID].player.username + "</color> in " + ChatManager.coloredTexts["blue"] + Server.clients[_clientID].world.name + "</color></color>: " + _reportText;
                    ChatManager.SendMessageToClient(index, str, false, str);
                }
            }
        }
    }
}
