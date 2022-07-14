
using System.Text.Json;

namespace Shittopia_Server
{
    internal class Inventory
    {
        public Player player;

        public int maxSlots { get; set; }

        public GameItem[] items { get; set; }

        public void Initialize()
        {
            this.items = new GameItem[GameData.items.Length];
            for (int index = 0; index < GameData.items.Length; ++index)
                this.items[index] = new GameItem()
                {
                    id = GameData.items[index].id,
                    quantity = 0
                };
            this.items[7].quantity = 1;
            this.items[8].quantity = 1;
        }

        public void AddItem(int _itemID, int _quantity)
        {
            this.items[_itemID].quantity += _quantity;
            ServerSend.EditInventoryData(this.player.id, _itemID, this.items[_itemID].quantity);
            if (Server.clients[this.player.id].account == null)
                return;
            Server.clients[this.player.id].account.Save();
        }

        public void RemoveItem(int _itemID, int _quantity)
        {
            this.items[_itemID].quantity -= _quantity;
            ServerSend.EditInventoryData(this.player.id, _itemID, this.items[_itemID].quantity);
            if (Server.clients[this.player.id].account == null)
                return;
            Server.clients[this.player.id].account.Save();
        }

        public void SetItem(int _itemID, int _quantity)
        {
            this.items[_itemID].quantity = _quantity;
            ServerSend.EditInventoryData(this.player.id, _itemID, _quantity);
            if (Server.clients[this.player.id].account == null)
                return;
            Server.clients[this.player.id].account.Save();
        }

        public string ToJson() => JsonSerializer.Serialize<Inventory>(this);
    }
}
