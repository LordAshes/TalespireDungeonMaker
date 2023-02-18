using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordAshes
{
    class Program
    {
        class Room
        {
            public int x { get; set; }
            public int y { get; set; }
            public int w { get; set; }
            public int h { get; set; }
            public bool rotunda { get; set; }
        }

        class Direction
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        class Door
        {
            public int x { get; set; }
            public int y { get; set; }
            public Direction dir { get; set; }
            public int type { get; set; }
        }

        public static Random rnd = new Random();

        static void Main(string[] args)
        {
            Dictionary<string, object> json = JsonConvert.DeserializeObject<Dictionary<string, object>>(System.IO.File.ReadAllText(args[0]));
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (Room rect in JsonConvert.DeserializeObject<Room[]>(json["rects"].ToString()))
            {
                if (rect.x < minX) { minX = rect.x; }
                if (rect.x+rect.w > maxX) { maxX = rect.x+rect.w; }
                if (rect.y < minY) { minY = rect.y; }
                if (rect.y+rect.h > maxY) { maxY = rect.y+rect.h; }
            }
            int deltaX = (int)(maxX - minX + 4);
            int deltaY = (int)(maxY - minY + 4);

            // Create map space
            string[,] map = new string[deltaX, deltaY];  
            
            // Populate default map
            for (int y=0; y<deltaY; y++)
            {
                for (int x = 0; x < deltaX; x++)
                {
                    if (x == 0 || y == 0 || x == (deltaX - 1) || y == (deltaY - 1))
                    {
                        map[x, y] = "-";
                    }
                    else
                    {
                        map[x, y] = "#";
                    }
                }
            }

            // Populate map space
            foreach (Room rect in (Room[])JsonConvert.DeserializeObject<Room[]>(json["rects"].ToString()))
            {
                for (int dy = (rect.y - minY); dy < (rect.y + rect.h - minY); dy++)
                {
                    for (int dx = (rect.x - minX); dx < (rect.x + rect.w - minX); dx++)
                    {
                        map[dx + 2, dy + 2] = ".";
                    }
                }
            }

            // Populate doors
            foreach (Door door in JsonConvert.DeserializeObject<Door[]>(json["doors"].ToString()))
            {
                map[door.x - minX + 2, door.y - minY + 2] = "O";
            }

            // Apply translations
            string[] translations = System.IO.File.ReadAllLines("Translations.txt");
            foreach (string translation in translations)
            {
                if (translation.Trim() != "" && !translation.Trim().StartsWith("'"))
                {
                    string pattern = translation.Substring(0, translation.IndexOf(":"));
                    string replacement = translation.Substring(translation.IndexOf(":")+1);
                    for (int y = 1; y < deltaY-1; y++)
                    {
                        for (int x = 1; x < deltaX-1; x++)
                        {
                            if (IsMatch(map, x, y, pattern))
                            {
                                if (!replacement.Contains("W"))
                                {
                                    Write(ref map, x, y, replacement);
                                }
                                else
                                {
                                    if(rnd.Next(1,100+1)>50)
                                    {
                                        Write(ref map, x, y, replacement);
                                    }
                                    else
                                    {
                                        rnd.Next(1,100);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Trim the border
            for (int y = 0; y < deltaY; y++)
            {
                for (int x = 0; x < deltaX; x++)
                {
                    if (x == 0 || y == 0 || x == (deltaX - 1) || y == (deltaY - 1))
                    {
                        map[x, y] = "";
                    }
                }
            }

            // Draw translated map
            DrawMap(map);

            // Create slab data
            SlabBuilder.CreateJSON(map);

            DrawMap(map,@"D:\wwwroot\TS\map.txt");
            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        public static bool IsMatch(string[,] map, int x, int y, string pattern)
        {
            int offset = 0;
            for(int dy=y-1; dy<=(y+1); dy++)
            {
                for (int dx = x - 1; dx <=(x+1); dx++)
                {
                    if(pattern.Substring(offset,1) != "?" && map[dx,dy]!=pattern.Substring(offset,1))
                    {
                        return false;
                    }
                    offset++;
                }
            }
            return true;
        }

        public static bool Write(ref string[,] map, int x, int y, string replacement)
        {
            int offset = 0;
            for (int dy = y - 1; dy <= y + 1; dy++)
            {
                for (int dx = x - 1; dx <= x + 1; dx++)
                {
                    if (replacement.Substring(offset,1) != "?")
                    {
                        map[dx, dy] = replacement.Substring(offset,1);
                    }
                    offset++;
                }
            }
            return true;
        }

        public static void DrawMap(string[,] map, string outputfile = "")
        {
            // Draw map
            if (outputfile != "") { System.IO.File.WriteAllText(outputfile,""); }
                for (int y = 0; y <= map.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= map.GetUpperBound(0); x++)
                {
                    if (outputfile == "")
                    {
                        Console.Write(map[x, y]);
                    }
                    else
                    {
                        System.IO.File.AppendAllText(outputfile,map[x, y]);
                    }
                }
                if (outputfile == "")
                {
                    Console.WriteLine();
                }
                else
                {
                    System.IO.File.AppendAllText(outputfile, "\r\n");
                }
            }
        }
    }
}
