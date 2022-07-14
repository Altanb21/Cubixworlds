
namespace Shittopia_Server
{
    internal class Pack
    {
        public int id { get; set; }

        public int categoryID { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public int cost { get; set; }

        public int[] itemIDs { get; set; }

        public int[] itemCounts { get; set; }

        public int logoTextureID { get; set; }

        public Pack(
          int _id,
          int _categoryID,
          string _title,
          string _description,
          int _cost,
          int[] _itemIDs,
          int[] _itemCounts,
          int _logoTextureID)
        {
            this.id = _id;
            this.categoryID = _categoryID;
            this.title = _title;
            this.description = _description;
            this.cost = _cost;
            this.itemIDs = new int[_itemIDs.Length];
            this.itemIDs = _itemIDs;
            this.itemCounts = new int[_itemCounts.Length];
            this.itemCounts = _itemCounts;
            this.logoTextureID = _logoTextureID;
        }
    }
}
