using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace PortManager.Models
{
    public class Item
    {
        public static String connString = "";
        public int id { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public string Currency { get; set; }
        public int trader_id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Item(int id, string Name, int trader_id, Decimal Price=0, string Currency="PKR", DateTime? CreatedAt=null, DateTime? UpdatedAt=null)
        {
            this.id        = id;
            this.Name      = Name;
            this.trader_id = trader_id;
            this.Price     = Price;
            this.Currency  = Currency;

            this.CreatedAt = CreatedAt == null ? DateTime.Now : (DateTime)CreatedAt;
            this.UpdatedAt = UpdatedAt == null ? DateTime.Now : (DateTime)UpdatedAt;
        }

        public static void Add(Item item)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"INSERT INTO [item] (name, price, currency, trader_id, created_at, updated_at) VALUES('{item.Name}', @Price, '{item.Currency}', @TraderId, @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Price",     item.Price);
            cmd.Parameters.AddWithValue("@TraderId",  item.trader_id);
            cmd.Parameters.AddWithValue("@CreatedAt", item.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", item.UpdatedAt);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static void Update(Item item)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"Update item set name = '{item.Name}', price = '{item.Price}', currency = '{item.Currency}', updated_at = @UpdatedAt where id = '{item.id}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UpdatedAt", item.UpdatedAt);
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
                items.Add(Helper.ReadItem(dr));
            }

            conn.Close();
            return items;
        }

        public static Item GetOne(int id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [item] where id = '{id}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            Item item = null;
            if (dr.Read())
            {
                item = Helper.ReadItem(dr);
            }

            conn.Close();
            return item;
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
                items.Add(Helper.ReadItem(dr));
            }

            conn.Close();
            return items;
        }

        public int QuantityOnShip(int ship_id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"SELECT * FROM [items_ships] WHERE item_id = {this.id} AND ship_id = {ship_id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            int quantity = -1;
            if (dr.Read())
                quantity = (int)dr[2];

            conn.Close();
            return quantity;
        }
    }
}
