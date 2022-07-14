
namespace Shittopia_Server
{
    internal class Trade
    {
        public int _id;
        public Client client1;
        public Client client2;
        public GameItem[] items1 = new GameItem[4];
        public GameItem[] items2 = new GameItem[4];
        public bool isAcceptedClient1;
        public bool isAcceptedClient2;
    }
}
