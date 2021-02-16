using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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

        public Ship(int id , string HIN, string NickName, int AllocatedBirth, int AllocatedTerminal)
        {
            this.id = id ;
            this.HIN        = HIN;
            this.NickName   = NickName;
            
            this.AllocatedBirth    = AllocatedBirth;
            this.AllocatedTerminal = AllocatedTerminal;

            //this.CreatedAt = DateTime.Now;
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

        public Ship(string HIN, int trader_id ,string NickName, int AllocatedBirth, int AllocatedTerminal)
        {
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

        public static void Add(Ship ship)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"insert into [ship] (hin, trader_id , nick_name, allocated_birth, allocated_terminal, created_at, updated_at) values ('{ship.HIN}', '{ship.trader_id}' ,'{ship.NickName}', '{ship.AllocatedBirth}', '{ship.AllocatedTerminal}', @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CreatedAt", ship.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", ship.UpdatedAt);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Update(Ship ship)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"Update ship set hin = '{ship.HIN}' , nick_name = '{ship.NickName}' , allocated_birth = '{ship.AllocatedBirth}' , allocated_terminal = '{ship.AllocatedTerminal}' , updated_at = '{ship.UpdatedAt}' where id = '{ship.id}' ";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void DeleteById(int id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"delete from [ship] WHERE id = {id}";
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
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [ship] where id = '{ship_id}' ";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                int
                    Id = (int)dr[0],
                    TraderId = (int)dr[2],
                    AllocatedBirth = (int)dr[4],
                    AllocatedTerminal = (int)dr[5];
                string
                    HIN = (string)dr[1],
                    NickName = (string)dr[3];
                
                Ship ship = new Ship(Id, HIN.Trim(), TraderId, NickName.Trim(), AllocatedBirth, AllocatedTerminal);
                return ship;
            }else{
                
                return null ;
            }

        }

        public static List<Ship> GetShipsByTrader(int trader_id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [ship] where trader_id = '{trader_id}' ";
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
    }
}
