using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PortManager.Models
{
    public class Item
    {
        public static String connString = "";
        public int id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
        public string Currency { get; set; }
        public int trader_id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Item(int id, string Name, int trader_id, int Quantity=0, Decimal Price=0, string Currency="PKR", DateTime? CreatedAt=null, DateTime? UpdatedAt=null)
        {
            this.id        = id;
            this.Name      = Name;
            this.trader_id = trader_id;
            this.Quantity  = Quantity;
            this.Price     = Price;
            this.Currency  = Currency;

            this.CreatedAt = (DateTime)CreatedAt;
            this.UpdatedAt = (DateTime)UpdatedAt;
        }

        public static void Add(Item item, int ship_id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"insert into [item] (name, quantity, price, currency, trader_id, created_at, updated_at) values ('{item.Name}', '{item.Quantity}', '{item.Price}', '{item.Currency}', '{item.trader_id}', '{item.CreatedAt}', '{item.UpdatedAt}')";
            SqlCommand cmd = new SqlCommand(query, conn);
            int id = (int)cmd.ExecuteScalar();

            string query2 = $"insert into [items_ships] (item_id, ship_id, created_at, updated_at) values ('{id}', '{ship_id}', '{DateTime.Now}', '{DateTime.Now}')";
            cmd = new SqlCommand(query2, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static void Update(Item item)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"Update item set name = '{item.Name}' , quantity = '{item.Quantity}' , price = '{item.Price}', currency = '{item.Currency}', updated_at = '{DateTime.Now}' where id = '{item.id}' ";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void DeleteById(int id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"delete from [item] WHERE id = {id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<Item> GetAll()
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [item]";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<Item> items = new List<Item>();

            while (dr.Read())
            {
                int
                    Id = (int)dr[0],
                    TraderId = (int)dr[5],
                    Quantity = (int)dr[2],
                    Price = (int)dr[3];
                string
                    Name = (string)dr[1],
                    Currency = (string)dr[4];
                DateTime
                    CreatedAt = (DateTime)dr[6],
                    UpdatedAt = (DateTime)dr[7];
                
                items.Add(new Item(Id, Name, TraderId, Quantity, Price, Currency, CreatedAt, UpdatedAt));
            }

            return items;
        }

        public static Item GetOne(int id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [item] where id = '{id}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                int
                    Id = (int)dr[0],
                    TraderId = (int)dr[5],
                    Quantity = (int)dr[2],
                    Price = (int)dr[3];
                string
                    Name = (string)dr[1],
                    Currency = (string)dr[4];
                DateTime
                    CreatedAt = (DateTime)dr[6],
                    UpdatedAt = (DateTime)dr[7];

                return new Item(Id, Name, TraderId, Quantity, Price, Currency, CreatedAt, UpdatedAt);
            }

            return null;
        }

        public static List<Item> GetAllByTrader(int trader_id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [item] where trader_id = '{trader_id}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<Item> items = new List<Item>();

            while (dr.Read())
            {
                int
                    Id = (int)dr[0],
                    TraderId = (int)dr[5],
                    Quantity = (int)dr[2],
                    Price = (int)dr[3];
                string
                    Name = (string)dr[1],
                    Currency = (string)dr[4];
                DateTime
                    CreatedAt = (DateTime)dr[6],
                    UpdatedAt = (DateTime)dr[7];

                items.Add(new Item(Id, Name, TraderId, Quantity, Price, Currency, CreatedAt, UpdatedAt));
            }

            return items;
        }
    }
}
