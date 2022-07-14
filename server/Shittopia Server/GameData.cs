
// Type: Shittopia_Server.GameData
// Assembly: Shittopia Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39E0F639-8647-4C60-A6C9-1ACE53BA2A14
// Assembly location: C:\Users\User\Desktop\cubixworlds\Shittopia Server\Shittopia Server\bin\Release\netcoreapp3.1\Shittopia Server.dll

namespace Shittopia_Server
{
    internal static class GameData
    {
        public static Item[] items = new Item[143]
        {
      new Item(0, "Air", "Air is everything, everything is nothing without air.", 0, 0, 1, false, -1, 0, 0, 0, 0, 0, 0, false, false, false, false, false, 4, false, 0),
      new Item(1, "Bedrock", "The only rock left from great lava explosion!", 0, 2, 1, true, -1, 1, 1, 1, 0, 0, 0, false, true, true, true, false, 4, true, 0),
      new Item(2, "White Door", "Given to worlds by god to let players spawn.", 0, 3, 1, false, -1, 2, 1, 1, 0, 0, 0, false, true, true, true, false, 4, true, 0),
      new Item(3, "Grass", "The origin of the life.", 0, 5, 1, true, 3, 3, 0, 4, 0, 3, 1, false, true, true, true, false, 5, true, 0),
      new Item(4, "Dirt", "The origin of the life.", 0, 4, 1, true, 3, 3, 0, 4, 0, 75, 1, false, true, true, true, false, 5, true, 0),
      new Item(5, "Lava", "Magma from the core of world.", 0, 6, 1, false, 4, 5, 0, 3, 0, 4, 3, true, true, true, true, false, 5, true, 10),
      new Item(6, "Stone", "So strong.", 0, 7, 1, true, 5, 6, 0, 2, 0, 4, 1, false, true, true, true, false, 3, true, 0),
      new Item(7, "Fist", "Fist", 0, 36, 2, false, -1, 0, 0, 0, 0, 0, 0, false, false, false, false, false, 1, false, 0),
      new Item(8, "Tool", "Tool", 0, 37, 2, false, -1, 0, 0, 0, 0, 0, 0, false, false, false, false, false, 1, false, 0),
      new Item(9, "Cubix Coin", "Tradable Coin", 0, 8, 2, false, 8, 9, 1, 1, 0, 0, 100, true, true, true, true, false, 2, true, 0),
      new Item(10, "Glass", "To see inside, farmable", 0, 9, 1, true, 3, 10, 0, 5, 15, 250, 5, false, true, true, true, false, 6, true, 0),
      new Item(11, "Dirt Background", "The background origin of the life.", 1, 10, 1, false, 3, 11, 0, 4, 0, 3, 1, false, true, true, true, false, 7, true, 0),
      new Item(12, "Invisible Block", "To make a background obstacle", 0, 0, 1, true, 1, 12, 1, 1, 0, 0, 4, false, true, true, true, false, 8, true, 0),
      new Item(13, "Blue Background", "Background for arts", 1, 11, 1, false, 1, 13, 1, 1, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(14, "Green Background", "Background for arts", 1, 12, 1, false, 1, 14, 1, 1, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(15, "Red Background", "Background for arts", 1, 13, 1, false, 1, 15, 1, 1, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(16, "Yellow Background", "Background for arts", 1, 14, 1, false, 1, 16, 1, 1, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(17, "Pink Background", "Background for arts", 1, 15, 1, false, 1, 17, 1, 1, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(18, "Purple Background", "Background for arts", 1, 16, 1, false, 1, 18, 1, 1, 4, 0, 5, false, true, true, true, false, 8, true, 0),
      new Item(19, "Cyan Background", "Background for arts", 1, 17, 1, false, 1, 19, 1, 1, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(20, "White Background", "Background for arts", 1, 18, 1, false, 1, 20, 1, 1, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(21, "Black Background", "Background for arts", 1, 19, 1, false, 1, 21, 1, 1, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(22, "Cubix MCoin", "Tradable Mega Coin", 0, 21, 2, false, 8, 9, 1, 1, 0, 0, 1000, false, true, true, true, false, 2, true, 0),
      new Item(23, "Water", "Source of life", 0, 22, 1, false, 4, 23, 0, 3, 0, 4, 3, true, true, true, true, false, 6, true, 0),
      new Item(24, "Oak Tree Part 2", "No Info", 0, 23, 1, false, 5, 24, 0, 2, 0, 4, 3, false, true, true, true, false, 6, true, 0),
      new Item(25, "Bush", "No Info", 0, 24, 1, false, 2, 25, 0, 2, 0, 4, 3, false, true, true, true, false, 6, true, 0),
      new Item(26, "Mud", "No Info", 0, 25, 1, false, 3, 26, 1, 3, 0, 3, 2000, true, true, true, true, false, 6, true, 0),
      new Item(27, "Sand", "No Info", 0, 26, 1, true, 3, 27, 1, 4, 0, 3, 2, false, true, true, true, false, 6, true, 0),
      new Item(28, "World Register", "World Register for trading your world!", 0, 27, 2, false, 1, 28, 1, 1, 0, 0, 1, false, false, false, false, false, 2, true, 0),
      new Item(29, "Grass", "The beauty of your world", 0, 28, 1, false, 2, 29, 1, 3, 0, 1, 1, false, true, true, true, false, 9, true, 0),
      new Item(30, "Sand Background", "No Info", 1, 29, 1, false, 4, 30, 1, 4, 0, 3, 2, false, true, true, true, false, 8, true, 0),
      new Item(31, "Dead Bush Variant 1", "No Info", 0, 30, 1, false, 2, 31, 1, 3, 0, 1, 1, false, true, true, true, false, 9, true, 0),
      new Item(32, "Dead Bush Variant 2", "No Info", 0, 31, 1, false, 2, 32, 1, 3, 0, 1, 1, false, true, true, true, false, 9, true, 0),
      new Item(33, "Tumbleweed", "No Info", 0, 32, 1, false, 2, 33, 1, 3, 0, 3, 1, false, true, true, true, false, 9, true, 0),
      new Item(34, "Cactus", "No Info", 0, 33, 1, false, 2, 34, 1, 3, 0, 1, 1, false, true, true, true, false, 9, true, 0),
      new Item(35, "Sand Stone", "No Info", 0, 34, 1, true, 5, 35, 1, 4, 0, 5, 2, false, true, true, true, false, 9, true, 0),
      new Item(36, "Sign", "No Info", 0, 35, 1, false, 3, 36, 1, 2, 0, 1, 5, false, true, true, true, true, 6, true, 0),
      new Item(37, "Blue Block", "Block for arts", 0, 39, 1, true, 4, 37, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(38, "Green Block", "Block for arts", 0, 40, 1, true, 4, 38, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(39, "Red Block", "Block for arts", 0, 41, 1, true, 4, 39, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(40, "Yellow Block", "Block for arts", 0, 42, 1, true, 4, 40, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(41, "Pink Block", "Block for arts", 0, 43, 1, true, 4, 41, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(42, "Purple Block", "Block for arts", 0, 44, 1, true, 4, 42, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(43, "Cyan Block", "Block for arts", 0, 45, 1, true, 4, 43, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(44, "White Block", "Block for arts", 0, 46, 1, true, 4, 44, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(45, "Black Block", "Block for arts", 0, 47, 1, true, 4, 45, 0, 2, 0, 4, 5, false, true, true, true, false, 8, true, 0),
      new Item(46, "Bricks", "No Info", 0, 112, 1, true, 5, 46, 1, 4, 0, 5, 2, false, true, true, true, false, 9, true, 0),
      new Item(47, "Metal Sign", "No Info", 0, 49, 1, false, 4, 47, 1, 2, 0, 1, 5, false, true, true, true, true, 6, true, 0),
      new Item(48, "Sign With Metal Chain", "No Info", 0, 48, 1, false, 3, 48, 1, 2, 0, 1, 5, false, true, true, true, true, 6, true, 0),
      new Item(49, "Metal Sign With Metal Chain", "No Info", 0, 49, 1, false, 4, 49, 1, 2, 0, 1, 5, false, true, true, true, true, 6, true, 0),
      new Item(50, "Straw Hat", "No Info", 0, 52, 8, false, 0, 50, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(51, "Furnace", "This can be used for melting", 0, 53, 1, false, 0, 51, 0, 0, 0, 0, 69, false, true, true, true, true, 9, true, 0),
      new Item(52, "Horse Mask", "No Info", 0, 59, 8, false, 0, 52, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(53, "Dandelions", "The beauty of your world", 0, 56, 1, false, 2, 53, 1, 3, 0, 1, 1, false, true, true, true, false, 9, true, 0),
      new Item(54, "Rose", "No Info", 0, 57, 1, false, 2, 54, 1, 3, 0, 1, 1, false, true, true, true, false, 9, true, 0),
      new Item(55, "Rocks", "No Info", 0, 58, 1, false, 3, 55, 1, 3, 0, 1, 1, false, true, true, true, false, 9, true, 0),
      new Item(56, "Fedora Hat", "No Info", 0, 63, 8, false, 0, 56, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(57, "White T-shirt", "No Info", 0, 60, 4, false, 0, 57, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(58, "Blue Jeans", "No Info", 0, 61, 5, false, 0, 58, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(59, "Black Shoes", "No Info", 0, 62, 6, false, 0, 59, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(60, "Overall Suit", "No Info", 0, 64, 4, false, 0, 60, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(61, "Leather Shoes", "No Info", 0, 65, 6, false, 0, 61, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(62, "Oak Tree Part 1", "No Info", 0, 66, 1, false, 5, 62, 0, 2, 0, 4, 3, false, true, true, true, false, 6, true, 0),
      new Item(63, "Oak Tree Part 3", "No Info", 0, 67, 1, false, 5, 63, 0, 2, 0, 4, 3, false, true, true, true, false, 6, true, 0),
      new Item(64, "Wood", "No Info", 0, 55, 1, true, 4, 64, 0, 2, 0, 4, 3, false, true, true, true, false, 6, true, 0),
      new Item(65, "Magma", "No Info", 0, 68, 1, true, 8, 65, 0, 2, 0, 4, 3, false, true, true, true, false, 6, true, 10),
      new Item(66, "Pumpkin Mask", "No Info", 0, 69, 8, false, 0, 66, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(67, "Ghost Pumpkin Mask", "No Info", 0, 70, 8, false, 0, 67, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(68, "Police Uniform", "No Info", 0, 71, 4, false, 0, 68, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(69, "Police Pants", "No Info", 0, 72, 5, false, 0, 69, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(70, "Police Shoes", "No Info", 0, 73, 6, false, 0, 70, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(71, "Navy Uniform", "No Info", 0, 74, 4, false, 0, 71, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(72, "Navy Pants", "No Info", 0, 75, 5, false, 0, 72, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(73, "Navy Shoes", "No Info", 0, 76, 6, false, 0, 73, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(74, "Plague Mask", "No Info", 0, 77, 8, false, 0, 74, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(75, "Goblin Mask", "No Info", 0, 78, 8, false, 0, 75, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(76, "Straight Hair", "No Info", 0, 79, 3, false, 0, 76, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(77, "Comb Over Hair", "No Info", 0, 80, 3, false, 0, 77, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(78, "Braided Hair", "No Info", 0, 81, 3, false, 0, 78, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(79, "Worker Hat", "No Info", 0, 82, 8, false, 0, 79, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(80, "Navy Hat", "No Info", 0, 83, 8, false, 0, 80, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(81, "Police Hat", "No Info", 0, 84, 8, false, 0, 81, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(82, "Inferno Wings", "No Info", 0, 85, 7, false, 0, 82, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(83, "Devil Wings", "No Info", 0, 86, 7, false, 0, 83, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(84, "Leaf Wings", "No Info", 0, 87, 7, false, 0, 84, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(85, "Angel Wings", "No Info", 0, 88, 7, false, 0, 85, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(86, "Crystal Wings", "No Info", 0, 89, 7, false, 0, 86, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(87, "Butterfly Wings", "No Info", 0, 94, 7, false, 0, 87, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(88, "Black Hoodie", "No Info", 0, 90, 4, false, 0, 88, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(89, "Black Pants", "No Info", 0, 91, 5, false, 0, 89, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(90, "Black Sport Shoes", "No Info", 0, 92, 6, false, 0, 90, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(91, "Top Hat", "No Info", 0, 93, 8, false, 0, 91, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(92, "Mushroom", "No Info", 0, 95, 1, true, 4, 92, 0, 2, 0, 4, 3, false, true, true, true, false, 6, true, 0),
      new Item(93, "Gift 1", "No Info", 0, 96, 1, false, 2, 93, 1, 1, 0, 0, 10, false, true, true, true, false, 6, true, 0),
      new Item(94, "Gift 2", "No Info", 0, 97, 1, false, 2, 94, 1, 1, 0, 0, 10, false, true, true, true, false, 6, true, 0),
      new Item(95, "Rainbow Block", "Farmable", 0, 98, 1, true, 5, 95, 0, 5, 100, 1000, 5, false, true, true, true, false, 6, true, 0),
      new Item(96, "Painting 1", "No Info", 0, 99, 1, false, 2, 96, 1, 1, 0, 0, 10, false, true, true, true, false, 6, true, 0),
      new Item(97, "Painting 2", "No Info", 0, 100, 1, false, 2, 97, 1, 1, 0, 0, 10, false, true, true, true, false, 6, true, 0),
      new Item(98, "Painting 3", "No Info", 0, 101, 1, false, 2, 98, 1, 1, 0, 0, 10, false, true, true, true, false, 6, true, 0),
      new Item(99, "Chandelier", "Farmable", 0, 102, 1, false, 3, 99, 0, 4, 100, 1000, 10, false, true, true, true, false, 6, true, 0),
      new Item(100, "Cloud", "From high hills", 0, 103, 1, true, 4, 100, 0, 4, 100, 1000, 8, false, true, true, true, false, 6, true, 0),
      new Item(101, "Wooden Chair", "No Info", 0, 104, 1, false, 3, 101, 0, 2, 0, 4, 4, false, true, true, true, false, 6, true, 0),
      new Item(102, "Wooden Table", "No Info", 0, 105, 1, false, 3, 102, 0, 2, 0, 4, 4, false, true, true, true, false, 6, true, 0),
      new Item(103, "Wooden Block", "No Info", 0, 106, 1, true, 4, 103, 0, 2, 0, 4, 4, false, true, true, true, false, 6, true, 0),
      new Item(104, "Wooden Windows", "No Info", 1, 107, 1, false, 3, 104, 0, 2, 0, 4, 4, false, true, true, true, false, 6, true, 0),
      new Item(105, "Wooden Fence", "No Info", 0, 108, 1, false, 3, 105, 0, 2, 0, 4, 4, false, true, true, true, false, 6, true, 0),
      new Item(106, "Pink Couch", "No Info", 0, 109, 1, false, 3, 106, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(107, "Blue Couch", "No Info", 0, 110, 1, false, 3, 107, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(108, "Black Couch", "No Info", 0, 111, 1, false, 4, 108, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(109, "Brick Background", "No Info", 1, 113, 1, false, 3, 109, 0, 3, 0, 6, 8, false, true, true, true, false, 6, true, 0),
      new Item(110, "Wooden Background", "No Info", 1, 114, 1, false, 3, 110, 0, 3, 0, 6, 8, false, true, true, true, false, 6, true, 0),
      new Item(111, "Portrait 1", "No Info", 0, 115, 1, false, 2, 111, 1, 1, 0, 0, 10, false, true, true, true, false, 6, true, 0),
      new Item(112, "Chad Portrait", "No Info", 0, 116, 1, false, 69, 112, 1, 1, 0, 0, 10, false, true, true, true, false, 6, true, 0),
      new Item(113, "Portrait 2", "No Info", 0, 117, 1, false, 2, 113, 1, 1, 0, 0, 10, false, true, true, true, false, 6, true, 0),
      new Item(114, "Santa Hat", "No Info", 0, 118, 8, false, 0, 114, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(115, "Santa Uniform", "No Info", 0, 119, 4, false, 0, 115, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(116, "Santa Pants", "No Info", 0, 120, 5, false, 0, 116, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(117, "Santa Beard", "No Info", 0, 121, 10, false, 0, 117, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(118, "Santa Shoes", "No Info", 0, 122, 6, false, 0, 118, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(119, "XMAS Hat", "No Info", 0, 123, 8, false, 0, 119, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(120, "Straight Brown Hair", "No Info", 0, 124, 3, false, 0, 120, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(121, "Brown Full-Beard", "No Info", 0, 125, 10, false, 0, 121, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(122, "Death Wings", "No Info", 0, 126, 7, false, 0, 122, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(123, "Pickaxe", "No Info", 0, (int) sbyte.MaxValue, 9, false, 0, 123, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0, 2),
      new Item(124, "Dragon Wings", "No Info", 0, 128, 7, false, 0, 124, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(125, "Dresser", "No Info", 0, 129, 1, false, 3, 125, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(126, "Cupboard", "No Info", 0, 130, 1, false, 3, 126, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item((int) sbyte.MaxValue, "Bookcase", "No Info", 0, 131, 1, false, 4, (int) sbyte.MaxValue, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(128, "Fireplace", "No Info", 0, 132, 1, false, 4, 128, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(129, "Bathub", "No Info", 0, 133, 1, false, 4, 129, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(130, "Toilet", "No Info", 0, 134, 1, false, 4, 130, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(131, "Bed", "No Info", 0, 135, 1, false, 4, 131, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(132, "Sunflower", "No Info", 0, 137, 1, false, 4, 132, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(133, "Clock", "No Info", 0, 136, 1, false, 4, 133, 0, 2, 0, 4, 6, false, true, true, true, false, 6, true, 0),
      new Item(134, "Tuxedo Uniform", "No Info", 0, 138, 4, false, 0, 134, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(135, "Tuxedo Pants", "No Info", 0, 139, 5, false, 0, 135, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(136, "Blue Dragonic Wings", "No Info", 0, 140, 7, false, 0, 136, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(137, "Black Suit Uniform", "No Info", 0, 141, 4, false, 0, 137, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(138, "Black Suit Shoes", "No Info", 0, 142, 6, false, 0, 138, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(139, "Black Suit Pants", "No Info", 0, 143, 5, false, 0, 139, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(140, ":) Shirt", "No Info", 0, 144, 4, false, 0, 140, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(141, "Sunglasses", "No Info", 0, 145, 10, false, 0, 141, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0),
      new Item(142, "Godly Fist", "No Info", 0, 36, 9, false, 0, 142, 0, 0, 0, 0, 10, false, true, true, true, true, 4, true, 0, 100)

        };
        public static Pack[] packs = new Pack[23]
        {
      new Pack(0, 0, "Colored Block Pack", "Art Pack contains 20x of every background and block for cool arts!", 2000, new int[18]
      {
        13,
        14,
        15,
        16,
        17,
        18,
        19,
        20,
        21,
        37,
        38,
        39,
        40,
        41,
        42,
        43,
        44,
        45
      }, new int[18]
      {
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20
      }, 38),
      new Pack(1, 3, "Cubix Coin", string.Format("Buy 1 Cubix Coin for {0} coins! Created for big trades.", (object) Economy.cubixCoin.worth), (int) Economy.cubixCoin.worth, new int[1]
      {
        9
      }, new int[1]{ 1 }, 8),
      new Pack(2, 0, "House Kit Pack 1", "House Pack contains 20x of every house block!", 2500, new int[14]
      {
        10,
        36,
        46,
        64,
        101,
        102,
        103,
        104,
        105,
        106,
        107,
        108,
        109,
        110
      }, new int[14]
      {
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20,
        20
      }, 38),
      new Pack(3, 1, "Farmer Pack", "Farmer clothes to get farmer vibe!", 500, new int[3]
      {
        50,
        60,
        61
      }, new int[3]{ 1, 1, 1 }, 52),
      new Pack(4, 1, "Basic Clothes Pack", "Includes basic clothes.", 500, new int[10]
      {
        56,
        57,
        58,
        59,
        88,
        89,
        90,
        91,
        139,
        140
      }, new int[10]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 60),
      new Pack(5, 2, "Inferno Wings", "From the soul of satan", 25000, new int[1]
      {
        82
      }, new int[1]{ 1 }, 85),
      new Pack(6, 2, "Leaf Wings", "From the soul of satan", 2500, new int[1]
      {
        84
      }, new int[1]{ 1 }, 87),
      new Pack(7, 2, "Death Wings", "Fly like a butterfly", 50000, new int[1]
      {
        122
      }, new int[1]{ 1 }, 126),
      new Pack(8, 2, "Blue Dragonic Wings", "Fly like a dragon", 10000, new int[1]
      {
        136
      }, new int[1]{ 1 }, 140),
      new Pack(9, 1, "Police Pack", "Police clothes to get police vibe!", 1000, new int[4]
      {
        68,
        69,
        70,
        81
      }, new int[4]{ 1, 1, 1, 1 }, 84),
      new Pack(10, 1, "Navy Pack", "Navy clothes to get navy vibe!", 1000, new int[4]
      {
        71,
        72,
        73,
        80
      }, new int[4]{ 1, 1, 1, 1 }, 83),
      new Pack(11, 1, "Santa Pack", "Santa clothes to get santa vibe!", 1000, new int[6]
      {
        114,
        115,
        116,
        117,
        118,
        119
      }, new int[6]{ 1, 1, 1, 1, 1, 1 }, 118),
      new Pack(12, 1, "Hair Pack", "Cool hairs to make femboys like you!", 1000, new int[4]
      {
        76,
        77,
        78,
        120
      }, new int[4]{ 1, 1, 1, 1 }, 79),
      new Pack(13, 1, "Mask Pack", "Cool masks to make femboys like you!", 2000, new int[4]
      {
        66,
        67,
        74,
        75
      }, new int[4]{ 1, 1, 1, 1 }, 77),
      new Pack(14, 1, "Horse Mask", "Only rich boys will get this", 10000, new int[1]
      {
        52
      }, new int[1]{ 1 }, 59),
      new Pack(15, 0, "Farmable Pack", "Get 25x of each farmable to farm", 2500, new int[3]
      {
        95,
        99,
        100
      }, new int[3]{ 25, 25, 25 }, 98),
      new Pack(16, 0, "Portrait Pack", "Get 1x of each portrait", 2500, new int[6]
      {
        96,
        97,
        98,
        111,
        112,
        113
      }, new int[6]{ 1, 1, 1, 1, 1, 1 }, 115),
      new Pack(17, 0, "House Kit Pack 2", "House Pack contains 20x of every house block!", 2500, new int[8]
      {
        125,
        126,
        (int) sbyte.MaxValue,
        128,
        139,
        130,
        131,
        133
      }, new int[8]{ 10, 10, 10, 10, 10, 10, 10, 10 }, 38),
      new Pack(18, 1, "Suit Pack", "Suit clothes to look cooler!", 1000, new int[3]
      {
        137,
        138,
        139
      }, new int[3]{ 1, 1, 1 }, 141),
      new Pack(19, 1, "Tuxedo Pack", "Tuxedo Pack to be the fuckboy!", 1000, new int[2]
      {
        134,
        135
      }, new int[2]{ 1, 1 }, 134),
      new Pack(20, 2, "Butterfly Wings", "Fly like a butterfly", 5000, new int[1]
      {
        87
      }, new int[1]{ 1 }, 94),
      new Pack(21, 2, "Crystal Wings", "From the universe of FROZEN", 10000, new int[1]
      {
        86
      }, new int[1]{ 1 }, 89),
      new Pack(22, 1, "Pickaxe", "Pickaxe, lets you to break the blocks faster.", 500, new int[1]
      {
        123
      }, new int[1]{ 1 }, (int) sbyte.MaxValue)
        };
        public static News[] news = new News[1]
        {
      new News(0, "BETA 4 is out!", "<b>Hello Build4Fun Players!</b>, \nWelcome to <b>Build4fun BETA 4</b>. We tried our best to permit\n<b>you</b> over the best experience in <b>Build4Fun</b>.\n\n<b>You</b> will be able to experience <b>BETA 4</b> for <b>3 Days</b>.\nPlease let us know if you find any <b>bugs</b>, use <b>/help</b> to see all\nthe commands in-game.\nEnjoy <b>BETA 4</b>!", "https://cdn.discordapp.com/attachments/480307512713805824/943615906075017236/unknown.png")
        };
    }
}
