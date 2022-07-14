
namespace Shittopia_Server
{
    public static class GameManager
    {
        public static void SendItemsDataToClient(int _clientID) => ServerSend.SendItemData(_clientID);
    }
}
