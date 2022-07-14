

using System.IO;
using System.Text.Json;

namespace Shittopia_Server
{
    public static class Economy
    {
        public static CubixCoin cubixCoin = new CubixCoin();
        public static string path = "EconomyData";

        public static void Load()
        {
            if (!File.Exists(Server.path + Economy.path))
                return;
            Economy.cubixCoin = JsonSerializer.Deserialize<CubixCoin>(File.ReadAllText(Server.path + Economy.path));
        }

        public static void Save() => File.WriteAllText(Server.path + Economy.path, JsonSerializer.Serialize<CubixCoin>(Economy.cubixCoin));

        public static bool Buy(int _count)
        {
            if (_count > Economy.cubixCoin.stock)
                return false;
            Economy.cubixCoin.stock -= _count;
            Economy.cubixCoin.worth = (float)(Economy.cubixCoin.maxValue / Economy.cubixCoin.stock + Economy.cubixCoin.minWorth);
            Economy.Save();
            return true;
        }

        public static void Sell(int _count)
        {
            Economy.cubixCoin.stock += _count;
            Economy.cubixCoin.worth = (float)(Economy.cubixCoin.maxValue / Economy.cubixCoin.stock + Economy.cubixCoin.minWorth);
            Economy.Save();
        }
    }
}
