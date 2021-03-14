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

        public Ship(int id, string HIN, int trader_id, string NickName, int AllocatedBirth, int AllocatedTerminal, DateTime? CreatedAt=null, DateTime? UpdatedAt=null)
        {
            this.id         = id;
            this.HIN        = HIN;
            this.trader_id  = trader_id;
            this.NickName   = NickName;
            
            this.AllocatedBirth    = AllocatedBirth;
            this.AllocatedTerminal = AllocatedTerminal;

            this.CreatedAt = CreatedAt == null ? DateTime.Now : (DateTime)CreatedAt;
            this.UpdatedAt = UpdatedAt == null ? DateTime.Now : (DateTime)UpdatedAt;
        }

        public CustomDuty CustomDuty()
        {
            List<CustomDuty> duties = Models.CustomDuty.GetAllByShip(this.id).Where(cd => cd.Status == "Unpaid").ToList();

            if (duties.Count == 0)
                return null;
            return duties.Last();
        }

        public static int Add(Ship ship)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"INSERT INTO [ship] (hin, trader_id, nick_name, allocated_birth, allocated_terminal, created_at, updated_at) OUTPUT Inserted.ID VALUES ('{ship.HIN}', {ship.trader_id}, @NickName, '{ship.AllocatedBirth}', '{ship.AllocatedTerminal}', @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NickName", ship.NickName);
            cmd.Parameters.AddWithValue("@CreatedAt", ship.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", ship.UpdatedAt);
            int id = (int)cmd.ExecuteScalar();
            conn.Close();

            return id;
        }

        public static void Update(Ship ship)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"Update ship set hin = '{ship.HIN}' , nick_name = '{ship.NickName}' , allocated_birth = '{ship.AllocatedBirth}' , allocated_terminal = '{ship.AllocatedTerminal}' , updated_at = @UpdatedAt where id = '{ship.id}' ";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UpdatedAt", ship.UpdatedAt);
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
            conn.Close();

            return ships;
        }

        public static Ship GetShip(int ship_id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [ship] where id = '{ship_id}' ";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            Ship ship = null;
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
                
                ship = new Ship(Id, HIN.Trim(), TraderId, NickName.Trim(), AllocatedBirth, AllocatedTerminal);
            }
            conn.Close();
            return ship;
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

            conn.Close();
            return ships;
        }

        public void AttachItems(List<int> items, List<int> quantities)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            // clear existing items
            string query1 = $"DELETE FROM [items_ships] WHERE ship_id = {this.id}";
            SqlCommand cmd = new SqlCommand(query1, conn);
            cmd.ExecuteNonQuery();

            for (int i = 0; i < items.Count; i++)
            {
                string query2 = $"INSERT INTO [items_ships] (item_id, ship_id, quantity, created_at, updated_at) VALUES ({items[i]}, {this.id}, {quantities[i]}, @CreatedAt, @UpdatedAt)";
                cmd = new SqlCommand(query2, conn);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        public List<Item> Items()
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"SELECT * FROM [items_ships] WHERE ship_id = {this.id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<Item> items = new List<Item>();
            while (dr.Read())
                items.Add(Item.GetOne((int)dr[0]));

            conn.Close();
            return items;
        }
    }
}
