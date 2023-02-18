using System;
using System.Collections.Generic;
using System.Linq;

namespace LordAshes
{
    public static class SlabBuilder
    {
        public class TileAsset
        {
            public int rotation;
            public string center;
            public string extents;
        }

        public static string[,] map; 

        private static string Map(int x, int y)
        {
            try
            { 
                return map[x - 1, y - 1];
            }
            catch           
            {
                return "#";
            }
        }

        public static void CreateJSON(string[,] mapData)
        {
            map = mapData;
            Dictionary<string, List<TileAsset>> tiles = new Dictionary<string, List<TileAsset>>();
            Console.WriteLine("   Analyzing Tiles...");
            string[] recordTiles = new string[] { "┌", "┐", "└", "┘", "┴", "┬", "┤", "├", "2", "4", "1", "3", "W", ".", "#", "╫" };
            for (int y = 1; y <= (map.GetUpperBound(1)+1); y++)
            {
                for (int x = 1; x <= (map.GetUpperBound(0)+1); x++)
                {
                    if (recordTiles.Contains(Map(x, y)))
                    {
                        string nguid = GetNGuid(Map(x, y));
                        if (!tiles.ContainsKey(nguid)) { tiles.Add(nguid, new List<TileAsset>()); }
                        tiles[nguid].Add(new TileAsset() { rotation = GetTileRotation(Map(x, y)), center = GetTilePosition(Map(x, y), x, y), extents = GetTileExtends(Map(x, y)) });
                        if ("1234".Contains(Map(x, y)))
                        {
                            tiles[GetNGuid(".")].Add(new TileAsset() { rotation = GetTileRotation("."), center = GetTilePosition(".", x, y), extents = GetTileExtends(".") });
                        }
                        if ("#".Contains(Map(x, y)))
                        {
                            tiles[GetNGuid("╬")].Add(new TileAsset() { rotation = GetTileRotation("╬"), center = GetTilePosition("╬", x, y), extents = GetTileExtends("╬") });
                        }
                        if ("╫".Contains(Map(x, y)))
                        {
                            tiles[GetNGuid("╪")].Add(new TileAsset() { rotation = GetTileRotation("╪"), center = GetTilePosition("╪", x, y), extents = GetTileExtends("╪") });
                        }
                    }
                }
            }

            Console.WriteLine("   Writing Tiles JSON...");
            System.IO.File.WriteAllText(@"D:\wwwroot\TS\layout.js", "create_payload =\r\n");
            System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "[\r\n");
            for (int n = 0; n < tiles.Count; n++)
            {
                Console.WriteLine("     Writing '" + tiles.ElementAt(n).Key + "' Tiles...");
                System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "    {\r\n");
                System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "        'nguid': '" + tiles.ElementAt(n).Key + "',\r\n");
                System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "        'assets':\r\n");
                System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "        [\r\n");
                for (int t = 0; t < tiles.ElementAt(n).Value.Count; t++)
                {
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "            {\r\n");
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "                'rotation': " + tiles.ElementAt(n).Value.ElementAt(t).rotation + ",\r\n");
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "                'bounds':\r\n");
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "                {\r\n");
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "                    'center': " + tiles.ElementAt(n).Value.ElementAt(t).center + ",\r\n");
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "                    'extents': " + tiles.ElementAt(n).Value.ElementAt(t).extents + "\r\n");
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "                }\r\n");

                    if (t < tiles.ElementAt(n).Value.Count - 1)
                    {
                        System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "            },\r\n");
                    }
                    else
                    {
                        System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "            }\r\n");
                    }
                }
                System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "        ]\r\n");
                if (n < tiles.Count - 1)
                {
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "    },\r\n");
                }
                else
                {
                    System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "    }\r\n");
                }
            }
            System.IO.File.AppendAllText(@"D:\wwwroot\TS\layout.js", "]\r\n");
        }

        private static string GetNGuid(string tile)
        {
            switch (tile)
            {
                case "┌":
                case "┐":
                case "└":
                case "┘":
                    return "ff6b5a27-a041-4ba1-ad33-01d82c77bb34";
                case "┴":
                case "┬":
                case "┤":
                case "├":
                    return "05b8b832-e457-4ea7-88d5-25fa44037f39";
                case "2":
                case "4":
                case "1":
                case "3":
                    return "a9759b32-9ad0-4157-ba8f-7d7e73a1f00c";
                case ".":
                    return "cf6063bb-5c6e-4107-b3e9-9c0c5ac75768";
                case "#":
                case "╬":
                case "╫":
                case "╪":
                    return "82b03fd9-1afb-463f-a3f8-2f30204d6561";
                case "W":
                    switch(Program.rnd.Next(1,5+1))
                    {
                        case 1:
                            return "2fc1f203-3702-4126-8777-5f94067a0c9d";
                        case 2:
                            return "910bdbf7-1cdf-4601-929a-9c89c5eaa8be";
                        case 3:
                            return "f8fdaf2d-3b5b-47d8-8e1d-077994535798";
                        case 4:
                            return "f12afdd6-a171-4a24-b3db-935c05e7aaf9";
                        default:
                            return "d3ec0216-95e4-44cc-b076-74f39504166e";
                    }
                default:
                    return "0b9f9061-a6e4-4126-829f-1deb43147f49";
            }
        }

        private static int GetTileRotation(string tile)
        {
            switch (tile)
            {
                case "┌": return 12;
                case "┐": return 0;
                case "└": return 8;
                case "┘": return 4;
                case "┴": return 12;
                case "├": return 0;
                case "┬": return 4;
                case "┤": return 8;
                case "2": return 4;
                case "1": return 4;
                case "3": return 8;
                case "4": return 8;
                case "#": return 0;
                case "╬": return 0;
                case "╫": return 4;
                case "╪": return 4;
                case "W": return 12;
                default: return 0;
            }
        }

        private static string GetTilePosition(string tile, double x, double y)
        {
            switch (tile)
            {
                case "4":
                    return "{'x': " + (x * -1) + ", 'y': 0.5, 'z': " + (y+0.5) + "}";
                case "3":
                    return "{'x': " + (x * -1) + ", 'y': 0.5, 'z': " + y + "}";
                case "2":
                    return "{'x': " + (x * -1) + ", 'y': 0.5, 'z': " + y + "}";
                case "1":
                    return "{'x': " + ((x * -1) + 0.5) + ", 'y': 0.5, 'z': " + y + "}";
                case "#":
                    return "{'x': " + (x * -1) + ", 'y': 0.5, 'z': " + y + "}";
                case "╬":
                    return "{'x': " + ((x * -1) + 0.5) + ", 'y': 0.5, 'z': " + y + "}";
                case "╫":
                    return "{'x': " + (x * -1) + ", 'y': 0.5, 'z': " + y + "}";
                case "╪":
                    return "{'x': " + (x * -1) + ", 'y': 0.5, 'z': " + (y + 0.5) + "}";
                case "+":
                    return "{'x': " + (x * -1) + ", 'y': 0.5, 'z': " + y + "}";
                case "W":
                    return "{'x': " + (x * -1) + ", 'y': 0, 'z': " + (y+1.0) + "}";
                default:
                    return "{'x': " + (x*-1) + ", 'y': 0, 'z': " + y + "}";
            }
        }

        private static string GetTileExtends(string tile)
        {
            switch (tile)
            {
                case "W":
                    return "{ 'x': 2, 'y': 1, 'z': 2}";
                default:
                    return "{ 'x': 1, 'y': 1, 'z': 1}";
            }
        }
    }
}
