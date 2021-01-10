using System;
using System.Collections.Generic;

namespace PortManager.Models
{
    public class Ship
    {
        public int id { get; set; }
        public string HIN { get; set; }
        public int trader_id { get; set; }
        public string NickName { get; set; }
        public int AllocatedBirth { get; set; }
        public int AllocatedTerminal { get; set; }
        public int CustomDuty { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Ship(int id, string HIN, int trader_id)
        {
            this.id        = id;
            this.HIN       = HIN;
            this.trader_id = trader_id;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public Ship(int id, string HIN, int trader_id, string NickName, int AllocatedBirth, int AllocatedTerminal, int CustomDuty)
        {
            this.id         = id;
            this.HIN        = HIN;
            this.trader_id  = trader_id;
            this.NickName   = NickName;
            this.CustomDuty = CustomDuty;
            
            this.AllocatedBirth    = AllocatedBirth;
            this.AllocatedTerminal = AllocatedTerminal;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        private static Ship[] arr = {
            new Ship(1, "123", 1, "MarineWay", 1, 1, 0),
            new Ship(2, "234", 1, "Sunny Go", 1, 2, 1000),
            new Ship(3, "345", 2, "Merry", 1, 3, 10000),
            new Ship(4, "456", 2, "Avenger", 2, 1, 5999),
            new Ship(5, "567", 3, "Providence", 2, 2, 15999)
        };

        public static List<Ship> GetShips()
        {
            return new List<Ship>(arr);
        }
    }
}
