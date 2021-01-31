using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PortManager.Models
{
    public class Ship
    {
        public static String connString = "";
        public int id { get; set; }
        public string HIN { get; set; }
        public int trader_id { get; set; }
        public string NickName { get; set; }
        public int AllocatedBirth { get; set; }
        public int AllocatedTerminal { get; set; }
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

        public Ship(int id, string HIN, int trader_id, string NickName, int AllocatedBirth, int AllocatedTerminal)
        {
            this.id         = id;
            this.HIN        = HIN;
            this.trader_id  = trader_id;
            this.NickName   = NickName;
            
            this.AllocatedBirth    = AllocatedBirth;
            this.AllocatedTerminal = AllocatedTerminal;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public int CustomDuty()
        {
            return 5000;
        }

        public static void AddShip(Ship ship)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"insert into [ship] (hin, nick_name, allocated_birth, allocated_terminal, created_at, updated_at) values ('{ship.HIN}', '{ship.NickName}', '{ship.AllocatedBirth}', '{ship.AllocatedTerminal}', '{ship.CreatedAt}', '{ship.UpdatedAt}')";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<Ship> GetShips()
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [ship]";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<Ship> ships = new List<Ship>();

            while (dr.Read())
            {
                int
                    Id = (int)dr[0],
                    TraderId = (int)dr[2],
                    AllocatedBirth = (int)dr[4],
                    AllocatedTerminal = (int)dr[5];
                string
                    HIN = (string)dr[1],
                    NickName = (string)dr[3];
                
                ships.Add(new Ship(Id, HIN, TraderId, NickName, AllocatedBirth, AllocatedTerminal));
            }

            return ships;
        }

        public static Ship GetShip(int ship_id)
        {
            List<Ship> arr = Ship.GetShips();
            return arr.Find(e => e.id == ship_id);
        }

        public static List<Ship> GetShipsByTrader(int trader_id)
        {
            List<Ship> arr = Ship.GetShips();
            return arr.Where<Ship>(s => s.trader_id == trader_id).ToList();
        }
    }
}
