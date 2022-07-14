
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shittopia_Server
{
    internal static class WorldGeneration
    {
        public static WorldGeneration.Biome[] Biomes = new WorldGeneration.Biome[1]
        {
        //new WorldGeneration.Biome(WorldGeneration.Desert)
         new WorldGeneration.Biome(WorldGeneration.Plains)
        };

        public static void Plains(ref World _world, int _worldWidth, int _worldHeight)
        {
            _world.whiteDoorPosition = new Vector2((float)new Random().Next(0, _worldWidth - 1), 0.0f);
            List<int> intList1 = new List<int>();
            List<int> intList2 = new List<int>();
            for (int x = 0; x < _worldWidth; ++x)
            {
                Noise noise = new Noise(1.0, 1.0, 1.0, 1, _world.seed);
                _world.currentTerrainHeight = (double)_world.terrainHeight + _world.generationscale * noise.Get2D((double)x * _world.generationXScale, (double)_world.seed);
                int num = (int)Math.Floor(_world.currentTerrainHeight);
                if (_world.currentTerrainHeight > (double)_world.waterLevel)
                {
                    intList1.Add(x);
                    intList2.Add(num);
                }
                for (int y = 0; y < _worldHeight; ++y)
                {
                    if (y <= num)
                    {
                        _world.worldLayers[1][x, y] = new Block();
                        _world.worldLayers[1][x, y].id = 11;
                        _world.worldLayers[1][x, y].health = GameData.items[11].health;
                    }
                    else
                    {
                        _world.worldLayers[1][x, y] = new Block();
                        _world.worldLayers[1][x, y].id = 0;
                        _world.worldLayers[1][x, y].health = GameData.items[0].health;
                    }
                    if (y == num)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 3;
                        _world.worldLayers[0][x, y].health = GameData.items[3].health;
                    }
                    else if (y < 6)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 1;
                        _world.worldLayers[0][x, y].health = GameData.items[1].health;
                    }
                    else if (y < 10 && noise.Get2DTrueFalse(new Vector2((float)x, (float)y), 10, 0.4f, 0.03f, _world.seed))
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 65;
                        _world.worldLayers[0][x, y].health = GameData.items[65].health;
                    }
                    else if (y < 13 && noise.Get2DTrueFalse(new Vector2((float)x, (float)y), 500, 0.4f, 0.06f, _world.seed))
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 5;
                        _world.worldLayers[0][x, y].health = GameData.items[5].health;
                    }
                    else if (noise.Get2DTrueFalse(new Vector2((float)x, (float)y), 500, 0.4f, 0.06f, _world.seed) && (double)y < _world.currentTerrainHeight - 12.0)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 0;
                        _world.worldLayers[0][x, y].health = GameData.items[0].health;
                        if (_world.worldLayers[0][x, y - 1].id != 0 && y > num / 2)
                        {
                            _world.worldLayers[0][x, y - 1].id = 3;
                            _world.worldLayers[0][x, y - 1].health = GameData.items[3].health;
                        }
                    }
                    else if (y < num / 2 + 2 && noise.Get2DTrueFalse(new Vector2((float)x, (float)y), 500, 0.4f, 0.06f, _world.seed))
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 6;
                        _world.worldLayers[0][x, y].health = GameData.items[6].health;
                    }
                    else if (y < num / 2)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 6;
                        _world.worldLayers[0][x, y].health = GameData.items[6].health;
                    }
                    else if (y < num)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 4;
                        _world.worldLayers[0][x, y].health = GameData.items[4].health;
                    }
                    else if (y < _world.waterLevel)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 23;
                        _world.worldLayers[0][x, y].health = GameData.items[23].health;
                    }
                    else
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 0;
                        _world.worldLayers[0][x, y].health = GameData.items[0].health;
                    }
                }
            }
            int index1 = new Random().Next(0, intList1.Count);
            _world.whiteDoorPosition = new Vector2((float)intList1[index1], (float)(intList2[index1] + 1));
            _world.worldLayers[0][intList1[index1], intList2[index1] + 1] = new Block();
            _world.worldLayers[0][intList1[index1], intList2[index1] + 1].id = 2;
            _world.worldLayers[0][intList1[index1], intList2[index1] + 1].health = GameData.items[2].health;
            _world.worldLayers[0][intList1[index1], intList2[index1]] = new Block();
            _world.worldLayers[0][intList1[index1], intList2[index1]].id = 1;
            _world.worldLayers[0][intList1[index1], intList2[index1]].health = GameData.items[1].health;
            List<int> intList3 = new List<int>();
            List<int> intList4 = new List<int>();
            for (int index2 = 0; index2 < intList1.Count; ++index2)
            {
                if (new Vector2((float)intList1[index2], (float)(intList2[index2] + 1)) != _world.whiteDoorPosition)
                {
                    intList3.Add(intList1[index2]);
                    intList4.Add(intList2[index2]);
                }
            }
            int num1 = new Random().Next(6, 18);
            for (int index3 = 0; index3 < num1; ++index3)
            {
                int index4 = new Random().Next(0, intList3.Count);
                int num2 = 1;
                int num3 = 0;
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num2] = new Block();
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num2].id = 62;
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num2].health = GameData.items[62].health;
                int num4 = num2 + 1;
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num4] = new Block();
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num4].id = 24;
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num4].health = GameData.items[24].health;
                int num5 = num4 + 1;
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num5] = new Block();
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num5].id = 24;
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num5].health = GameData.items[24].health;
                int num6 = num5 + 1;
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num6] = new Block();
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num6].id = 63;
                _world.worldLayers[0][intList3[index4] + num3, intList4[index4] + num6].health = GameData.items[63].health;
                int num7 = num3 + 1;
                if (intList3[index4] + num7 >= _worldWidth)
                    --num7;
                _world.worldLayers[0][intList3[index4] + num7, intList4[index4] + num6] = new Block();
                _world.worldLayers[0][intList3[index4] + num7, intList4[index4] + num6].id = 25;
                _world.worldLayers[0][intList3[index4] + num7, intList4[index4] + num6].health = GameData.items[25].health;
                int num8 = -1;
                if (intList3[index4] + num8 < 0)
                    ++num8;
                _world.worldLayers[0][intList3[index4] + num8, intList4[index4] + num6] = new Block();
                _world.worldLayers[0][intList3[index4] + num8, intList4[index4] + num6].id = 25;
                _world.worldLayers[0][intList3[index4] + num8, intList4[index4] + num6].health = GameData.items[25].health;
                int num9 = 0;
                int num10 = num6 + 1;
                _world.worldLayers[0][intList3[index4] + num9, intList4[index4] + num10] = new Block();
                _world.worldLayers[0][intList3[index4] + num9, intList4[index4] + num10].id = 25;
                _world.worldLayers[0][intList3[index4] + num9, intList4[index4] + num10].health = GameData.items[25].health;
                int num11 = num9 + 1;
                if (intList3[index4] + num11 >= _worldWidth)
                    --num11;
                _world.worldLayers[0][intList3[index4] + num11, intList4[index4] + num10] = new Block();
                _world.worldLayers[0][intList3[index4] + num11, intList4[index4] + num10].id = 25;
                _world.worldLayers[0][intList3[index4] + num11, intList4[index4] + num10].health = GameData.items[25].health;
                int num12 = num11 + 1;
                if (intList3[index4] + num12 >= _worldWidth)
                    --num12;
                _world.worldLayers[0][intList3[index4] + num12, intList4[index4] + num10] = new Block();
                _world.worldLayers[0][intList3[index4] + num12, intList4[index4] + num10].id = 25;
                _world.worldLayers[0][intList3[index4] + num12, intList4[index4] + num10].health = GameData.items[25].health;
                int num13 = -1;
                if (intList3[index4] + num13 < 0)
                    ++num13;
                _world.worldLayers[0][intList3[index4] + num13, intList4[index4] + num10] = new Block();
                _world.worldLayers[0][intList3[index4] + num13, intList4[index4] + num10].id = 25;
                _world.worldLayers[0][intList3[index4] + num13, intList4[index4] + num10].health = GameData.items[25].health;
                int num14 = num13 - 1;
                if (intList3[index4] + num14 < 0)
                    ++num14;
                _world.worldLayers[0][intList3[index4] + num14, intList4[index4] + num10] = new Block();
                _world.worldLayers[0][intList3[index4] + num14, intList4[index4] + num10].id = 25;
                _world.worldLayers[0][intList3[index4] + num14, intList4[index4] + num10].health = GameData.items[25].health;
                int num15 = 0;
                int num16 = num10 + 1;
                _world.worldLayers[0][intList3[index4] + num15, intList4[index4] + num16] = new Block();
                _world.worldLayers[0][intList3[index4] + num15, intList4[index4] + num16].id = 25;
                _world.worldLayers[0][intList3[index4] + num15, intList4[index4] + num16].health = GameData.items[25].health;
                int num17 = num15 + 1;
                if (intList3[index4] + num17 >= _worldWidth)
                    --num17;
                _world.worldLayers[0][intList3[index4] + num17, intList4[index4] + num16] = new Block();
                _world.worldLayers[0][intList3[index4] + num17, intList4[index4] + num16].id = 25;
                _world.worldLayers[0][intList3[index4] + num17, intList4[index4] + num16].health = GameData.items[25].health;
                int num18 = num17 + 1;
                if (intList3[index4] + num18 >= _worldWidth)
                    --num18;
                _world.worldLayers[0][intList3[index4] + num18, intList4[index4] + num16] = new Block();
                _world.worldLayers[0][intList3[index4] + num18, intList4[index4] + num16].id = 25;
                _world.worldLayers[0][intList3[index4] + num18, intList4[index4] + num16].health = GameData.items[25].health;
                int num19 = -1;
                if (intList3[index4] + num19 < 0)
                    ++num19;
                _world.worldLayers[0][intList3[index4] + num19, intList4[index4] + num16] = new Block();
                _world.worldLayers[0][intList3[index4] + num19, intList4[index4] + num16].id = 25;
                _world.worldLayers[0][intList3[index4] + num19, intList4[index4] + num16].health = GameData.items[25].health;
                int num20 = num19 - 1;
                if (intList3[index4] + num20 < 0)
                    ++num20;
                _world.worldLayers[0][intList3[index4] + num20, intList4[index4] + num16] = new Block();
                _world.worldLayers[0][intList3[index4] + num20, intList4[index4] + num16].id = 25;
                _world.worldLayers[0][intList3[index4] + num20, intList4[index4] + num16].health = GameData.items[25].health;
                int num21 = 0;
                int num22 = num16 + 1;
                _world.worldLayers[0][intList3[index4] + num21, intList4[index4] + num22] = new Block();
                _world.worldLayers[0][intList3[index4] + num21, intList4[index4] + num22].id = 25;
                _world.worldLayers[0][intList3[index4] + num21, intList4[index4] + num22].health = GameData.items[25].health;
                int num23 = num21 + 1;
                if (intList3[index4] + num23 >= _worldWidth)
                    --num23;
                _world.worldLayers[0][intList3[index4] + num23, intList4[index4] + num22] = new Block();
                _world.worldLayers[0][intList3[index4] + num23, intList4[index4] + num22].id = 25;
                _world.worldLayers[0][intList3[index4] + num23, intList4[index4] + num22].health = GameData.items[25].health;
                int num24 = -1;
                if (intList3[index4] + num24 < 0)
                    ++num24;
                _world.worldLayers[0][intList3[index4] + num24, intList4[index4] + num22] = new Block();
                _world.worldLayers[0][intList3[index4] + num24, intList4[index4] + num22].id = 25;
                _world.worldLayers[0][intList3[index4] + num24, intList4[index4] + num22].health = GameData.items[25].health;
                intList3.RemoveAt(index4);
                intList4.RemoveAt(index4);
            }
            int num25 = new Random().Next(20, 40);
            if (num25 > intList3.Count)
                num25 = intList3.Count;
            for (int index5 = 0; index5 < num25; ++index5)
            {
                int index6 = new Random().Next(0, intList3.Count);
                _world.worldLayers[0][intList3[index6], intList4[index6] + 1] = new Block();
                _world.worldLayers[0][intList3[index6], intList4[index6] + 1].id = 29;
                _world.worldLayers[0][intList3[index6], intList4[index6] + 1].health = GameData.items[29].health;
                intList3.RemoveAt(index6);
                intList4.RemoveAt(index6);
            }
            int num26 = new Random().Next(5, 15);
            if (num26 > intList3.Count)
                num26 = intList3.Count;
            Server.Log(num26.ToString());
            for (int index7 = 0; index7 < num26; ++index7)
            {
                int index8 = new Random().Next(0, intList3.Count);
                int index9 = new Random().Next(53, 55);
                _world.worldLayers[0][intList3[index8], intList4[index8] + 1] = new Block();
                _world.worldLayers[0][intList3[index8], intList4[index8] + 1].id = index9;
                _world.worldLayers[0][intList3[index8], intList4[index8] + 1].health = GameData.items[index9].health;
                intList3.RemoveAt(index8);
                intList4.RemoveAt(index8);
            }
            int num27 = new Random().Next(2, 8);
            if (num27 > intList3.Count)
                num27 = intList3.Count;
            for (int index10 = 0; index10 < num27; ++index10)
            {
                int index11 = new Random().Next(0, intList3.Count);
                _world.worldLayers[0][intList3[index11], intList4[index11] + 1] = new Block();
                _world.worldLayers[0][intList3[index11], intList4[index11] + 1].id = 55;
                _world.worldLayers[0][intList3[index11], intList4[index11] + 1].health = GameData.items[55].health;
                intList3.RemoveAt(index11);
                intList4.RemoveAt(index11);
            }
        }

       /* public static void Desert(ref World _world, int _worldWidth, int _worldHeight)
        {
            _world.whiteDoorPosition = new Vector2((float)new Random().Next(0, _worldWidth - 1), 0.0f);
            List<int> intList1 = new List<int>();
            List<int> intList2 = new List<int>();
            for (int x = 0; x < _worldWidth; ++x)
            {
                Noise noise = new Noise(1.0, 1.0, 1.0, 1, _world.seed);
                _world.currentTerrainHeight = (double)_world.terrainHeight + _world.generationscale * noise.Get2D((double)x * _world.generationXScale, (double)_world.seed);
                int num = (int)Math.Floor(_world.currentTerrainHeight);
                if (_world.currentTerrainHeight >= (double)(_world.waterLevel - 1))
                {
                    intList1.Add(x);
                    intList2.Add(num);
                }
                for (int y = 0; y < _worldHeight; ++y)
                {
                    if (y <= num)
                    {
                        _world.worldLayers[1][x, y] = new Block();
                        _world.worldLayers[1][x, y].id = 30;
                        _world.worldLayers[1][x, y].health = GameData.items[30].health;
                    }
                    else
                    {
                        _world.worldLayers[1][x, y] = new Block();
                        _world.worldLayers[1][x, y].id = 0;
                        _world.worldLayers[1][x, y].health = GameData.items[0].health;
                    }
                    if (y == num)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 27;
                        _world.worldLayers[0][x, y].health = GameData.items[27].health;
                    }
                    else if (y < 6)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 1;
                        _world.worldLayers[0][x, y].health = GameData.items[1].health;
                    }
                    else if (y < 12 && noise.Get2DTrueFalse(new Vector2((float)x, (float)y), 500, 0.4f, 0.06f, _world.seed))
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 5;
                        _world.worldLayers[0][x, y].health = GameData.items[5].health;
                    }
                    else if (noise.Get2DTrueFalse(new Vector2((float)x, (float)y), 500, 0.4f, 0.06f, _world.seed) && (double)y < _world.currentTerrainHeight - 12.0)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 0;
                        _world.worldLayers[0][x, y].health = GameData.items[0].health;
                    }
                    else if (y < num / 2 + 2 && noise.Get2DTrueFalse(new Vector2((float)x, (float)y), 500, 0.4f, 0.06f, _world.seed))
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 27;
                        _world.worldLayers[0][x, y].health = GameData.items[27].health;
                    }
                    else if (y < num / 2)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 35;
                        _world.worldLayers[0][x, y].health = GameData.items[35].health;
                    }
                    else if (y < num)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 27;
                        _world.worldLayers[0][x, y].health = GameData.items[27].health;
                    }
                    else if (y < _world.waterLevel)
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 5;
                        _world.worldLayers[0][x, y].health = GameData.items[5].health;
                    }
                    else
                    {
                        _world.worldLayers[0][x, y] = new Block();
                        _world.worldLayers[0][x, y].id = 0;
                        _world.worldLayers[0][x, y].health = GameData.items[0].health;
                    }
                }
            }
            int index1 = new Random().Next(0, intList1.Count);
            _world.whiteDoorPosition = new Vector2((float)intList1[index1], (float)(intList2[index1] + 1));
            _world.worldLayers[0][intList1[index1], intList2[index1] + 1] = new Block();
            _world.worldLayers[0][intList1[index1], intList2[index1] + 1].id = 2;
            _world.worldLayers[0][intList1[index1], intList2[index1] + 1].health = GameData.items[2].health;
            _world.worldLayers[0][intList1[index1], intList2[index1]] = new Block();
            _world.worldLayers[0][intList1[index1], intList2[index1]].id = 1;
            _world.worldLayers[0][intList1[index1], intList2[index1]].health = GameData.items[1].health;
            List<int> intList3 = new List<int>();
            List<int> intList4 = new List<int>();
            for (int index2 = 0; index2 < intList1.Count; ++index2)
            {
                if (new Vector2((float)intList1[index2], (float)(intList2[index2] + 1)) != _world.whiteDoorPosition)
                {
                    intList3.Add(intList1[index2]);
                    intList4.Add(intList2[index2]);
                }
            }
            int num1 = new Random().Next(5, 15);
            for (int index3 = 0; index3 < num1; ++index3)
            {
                int index4 = new Random().Next(0, intList3.Count);
                int num2 = 0;
                _world.worldLayers[0][intList3[index4] + num2, intList4[index4] + 1] = new Block();
                _world.worldLayers[0][intList3[index4] + num2, intList4[index4] + 1].id = 34;
                _world.worldLayers[0][intList3[index4] + num2, intList4[index4] + 1].health = GameData.items[34].health;
                intList3.RemoveAt(index4);
                intList4.RemoveAt(index4);
            }
            int num3 = new Random().Next(2, 6);
            if (num3 > intList3.Count)
                num3 = intList3.Count;
            for (int index5 = 0; index5 < num3; ++index5)
            {
                int index6 = new Random().Next(0, intList3.Count);
                _world.worldLayers[0][intList3[index6], intList4[index6] + 1] = new Block();
                _world.worldLayers[0][intList3[index6], intList4[index6] + 1].id = 31;
                _world.worldLayers[0][intList3[index6], intList4[index6] + 1].health = GameData.items[31].health;
                intList3.RemoveAt(index6);
                intList4.RemoveAt(index6);
            }
            int num4 = new Random().Next(2, 5);
            if (num4 > intList3.Count)
                num4 = intList3.Count;
            for (int index7 = 0; index7 < num4; ++index7)
            {
                int index8 = new Random().Next(0, intList3.Count);
                _world.worldLayers[0][intList3[index8], intList4[index8] + 1] = new Block();
                _world.worldLayers[0][intList3[index8], intList4[index8] + 1].id = 32;
                _world.worldLayers[0][intList3[index8], intList4[index8] + 1].health = GameData.items[32].health;
                intList3.RemoveAt(index8);
                intList4.RemoveAt(index8);
            }
            int num5 = new Random().Next(2, 5);
            if (num5 > intList3.Count)
                num5 = intList3.Count;
            for (int index9 = 0; index9 < num5; ++index9)
            {
                int index10 = new Random().Next(0, intList3.Count);
                _world.worldLayers[0][intList3[index10], intList4[index10] + 1] = new Block();
                _world.worldLayers[0][intList3[index10], intList4[index10] + 1].id = 33;
                _world.worldLayers[0][intList3[index10], intList4[index10] + 1].health = GameData.items[33].health;
                intList3.RemoveAt(index10);
                intList4.RemoveAt(index10);
            }
        }*/

        public delegate void Biome(ref World _world, int _worldWidth, int _worldHeight);
    }
}
