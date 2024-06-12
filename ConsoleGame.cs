using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Find + symbols to heal yourself. Beware the holes!");
            Console.ReadKey();
            var randomavatar = new Random();
            List<Player> players = new List<Player>();
            List<Item> items = new List<Item>();

            List<string> availableavatars = new List<string>()
            {
                "@", "%", "&"
            };
            int index = randomavatar.Next (availableavatars.Count);
            int itemsLimit = 2;
            int playersLimit = 1; 
            for (int i = 0; i < playersLimit; i++)
            {
                players.Add(CreatePlayer("player " + (i + 1) + "(" + availableavatars[index] + ")"));
                players[i].x += i;
                players[i].y += 1;
                players[i].avatar = availableavatars[index];
            }
            for (int i = 0; i < itemsLimit; i++)
            {
                items.Add(CreateItem(i));
                items[i].x += i+2;
                items[i].y += i+2;
            }
            foreach (Item t in items)
            {
                Console.WriteLine(t);
            }

            foreach (Player p in players)
            {
                Console.WriteLine(p);
            }
            Console.ReadKey(true);
            Console.Clear();

            List<List<int>> map = new List<List<int>>()
            {
                new List<int> {0, 0, 0, 0, 0, 0, 0, 0, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 2, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 0},
                new List<int> {0, 0, 0, 0, 0, 0, 0, 0, 0},
            };

            for (int x = 0; x < map.Count; x++)
            {
                for(int y = 0; y < map[x].Count; y++)
                {
                    Console.SetCursorPosition(x, y);
                    int cell = map[x][y];
                    string cellView = GetCellView(cell);


                    Console.Write(cellView);
                }
            }

            foreach (Player p in players)
            {
                Console.SetCursorPosition(p.x, p.y);
                Console.Write(p.avatar);
            }
            foreach (Item t in items)
            {
                Console.SetCursorPosition(t.x, t.y);
                Console.Write(t.avatar);
            }


            bool keepPlaying = true;
            while (keepPlaying)
            {
                foreach (Player p in players)
                {
                    int oldX = p.x;
                    int oldY = p.y;

                    p.Move(map);
                    Console.SetCursorPosition(oldX, oldY);
                    int cell = map[oldX][oldY];
                    string cellView = GetCellView(cell);
                    Console.Write(cellView);
                    
                    Console.SetCursorPosition(p.x, p.y);
                    if (map[p.x][p.y] == 2)
                    {
                        Console.Write("+");
                        keepPlaying = false;
                        break;
                    }
                    else
                    {
                        Console.Write(p.avatar);
                    }
                    foreach (Item t in items)
                    {
                        if (oldX == t.x && oldY == t.y)
                        {
                            int indx = t.index;
                            p.Heal (items[indx]);
                        }
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Game over");
        }

        static string GetCellView(int cell)
        {
            string cellView = "!";

            if (cell == 0)
            {
                cellView = "#";
            }
            else if (cell == 1)
            {
                cellView = ".";
            }
            else if(cell == 2)
            {
                cellView = "O";
            }

            return cellView;
        }

        static Player CreatePlayer(string text)
        {
            Console.WriteLine("What's your name, " + text + "?");
            Player player = new Player();
            player.name = Console.ReadLine();

            return player;
        }
        static Item CreateItem(int k)
        {
            Item items = new Item();
            items.index = k;

            return items;
        }

    class Player
    {
        public string name;
        public int hp = 60;
        public int maxHp = 100;
        int minDamage = 5;
        int maxDamage = 20;
        int healAmount = 2;
        Random rng = new Random();
        public int x = 2;
        public int y = 0;
        public int speed = 1;
        public string avatar = "@";

        public override string ToString()
        {
            return "Player's name: " + name + ", hp: " + hp + "/" + maxHp + ", position: (x: " + x + ", y: " + y + ")";
        }

        public void Attack(Player other)
        {
            int damage = rng.Next(minDamage, maxDamage+1);
            int damageRangeLength = maxDamage - minDamage;
            int rolledDamage = damage - minDamage;

            float damagePercent = (float)rolledDamage / damageRangeLength;
            Console.WriteLine("Damage range length: " + damageRangeLength);
            Console.WriteLine("Rolled damage: " + rolledDamage);
            Console.WriteLine("Damage percent: " + damagePercent);

            string attackType;
            if (damagePercent <= 0.15)
            {
                attackType = "weak";
            }
            else if (damagePercent <= 0.85)
            {
                attackType = "normal";
            }
            else
            {
                attackType = "CRITICAL";
            }

            Console.WriteLine(name + " attacking " + other.name + " for " + damage + " (" + attackType + " attack)");
            other.hp -= damage;
        }

        public void Heal(Item other)
        {
            if (other.healValue == 0)
            { 
                
            }
            else 
            {
                Console.SetCursorPosition(1,9);
                Console.WriteLine(name + " is healing");
                hp += other.healValue;
                other.healValue = 0;
                Console.ReadKey();
                Console.SetCursorPosition(1,9);
                Console.WriteLine("                                              ");
            }
        }

        public void Move(List<List<int>> map)
        {
            ConsoleKeyInfo moveKeyInfo = Console.ReadKey(true);
            if (moveKeyInfo.Key == ConsoleKey.D)
            {
                if (x + speed < Console.BufferWidth && map[x + speed][y] != 0)
                {
                    x += speed;
                }
            }
            else if (moveKeyInfo.Key == ConsoleKey.A && map[x - speed][y] != 0)
            {
                if (x - speed >= 0)
                {
                    x -= speed;
                }
            }
            else if (moveKeyInfo.Key == ConsoleKey.W && map[x][y - speed] != 0)
            {
                if (y - speed >= 0)
                {
                    y -= speed;
                }
            }
            else if (moveKeyInfo.Key == ConsoleKey.S && map[x][y + speed] != 0)
            {
                if (y + speed < Console.BufferHeight)
                {
                    y += speed;
                }
            }
        }
        
    }
       class Item
        {
            public string ItemName;
            public int healValue = 20;
            public string avatar = "+";
            public int x = 2;
            public int y = 3;
            public int index;
            
        }
}
}
