

namespace Shittopia_Server
{
    internal class Item
    {
        public int layer;
        public int itemDropID;
        public int itemDropRarityMin;
        public int itemDropRarityMax;
        public int gemDropRarityMin;
        public int gemDropRarityMax;

        public int id { get; set; }

        public int inventoryOrder { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public int rarity { get; set; }

        public bool isDropable { get; set; }

        public bool isTrashable { get; set; }

        public bool isConvertable { get; set; }

        public bool isTradeable { get; set; }

        public int textureID { get; set; }

        public int typeID { get; set; }

        public bool isSolid { get; set; }

        public bool isSign { get; set; }

        public bool waterPhysics { get; set; }

        public int health { get; set; }

        public int damage { get; set; }

        public int attack { get; set; }

        public Item(
          int _id,
          string _name,
          string _description,
          int _layer,
          int _textureID,
          int _typeID,
          bool _isSolid,
          int _health,
          int _itemDropID,
          int _itemDropRarityMin,
          int _itemDropRarityMax,
          int _gemDropRarityMin,
          int _gemDropRarityMax,
          int _rarity,
          bool _waterPhysics,
          bool _isDropable,
          bool _isTrashable,
          bool _isConvertable,
          bool _isSign,
          int _inventoryOrder,
          bool _isTradeable,
          int _damage)
        {
            this.id = _id;
            this.name = _name;
            this.description = _description;
            this.layer = _layer;
            this.textureID = _textureID;
            this.typeID = _typeID;
            this.isSolid = _isSolid;
            this.health = _health;
            this.itemDropID = _itemDropID;
            this.itemDropRarityMin = _itemDropRarityMin;
            this.itemDropRarityMax = _itemDropRarityMax + 1;
            this.gemDropRarityMin = _gemDropRarityMin;
            this.gemDropRarityMax = _gemDropRarityMax + 1;
            this.rarity = _rarity;
            this.waterPhysics = _waterPhysics;
            this.isDropable = _isDropable;
            this.isTrashable = _isTrashable;
            this.isConvertable = _isConvertable;
            this.isSign = _isSign;
            this.inventoryOrder = _inventoryOrder;
            this.isTradeable = _isTradeable;
            this.damage = _damage;
        }

        public Item(
          int _id,
          string _name,
          string _description,
          int _layer,
          int _textureID,
          int _typeID,
          bool _isSolid,
          int _health,
          int _itemDropID,
          int _itemDropRarityMin,
          int _itemDropRarityMax,
          int _gemDropRarityMin,
          int _gemDropRarityMax,
          int _rarity,
          bool _waterPhysics,
          bool _isDropable,
          bool _isTrashable,
          bool _isConvertable,
          bool _isSign,
          int _inventoryOrder,
          bool _isTradeable,
          int _damage,
          int _attack)
        {
            this.id = _id;
            this.name = _name;
            this.description = _description;
            this.layer = _layer;
            this.textureID = _textureID;
            this.typeID = _typeID;
            this.isSolid = _isSolid;
            this.health = _health;
            this.itemDropID = _itemDropID;
            this.itemDropRarityMin = _itemDropRarityMin;
            this.itemDropRarityMax = _itemDropRarityMax + 1;
            this.gemDropRarityMin = _gemDropRarityMin;
            this.gemDropRarityMax = _gemDropRarityMax + 1;
            this.rarity = _rarity;
            this.waterPhysics = _waterPhysics;
            this.isDropable = _isDropable;
            this.isTrashable = _isTrashable;
            this.isConvertable = _isConvertable;
            this.isSign = _isSign;
            this.inventoryOrder = _inventoryOrder;
            this.isTradeable = _isTradeable;
            this.damage = _damage;
            this.attack = _attack;
        }
    }
}
