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

            string query = $"INSERT INTO [item] (name, quantity, price, currency, trader_id, created_at, updated_at) OUTPUT Inserted.ID VALUES('{item.Name}', @Quantity, @Price, '{item.Currency}', @TraderId, @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Quantity",  item.Quantity);
            cmd.Parameters.AddWithValue("@Price",     item.Price);
            cmd.Parameters.AddWithValue("@TraderId",  item.trader_id);
            cmd.Parameters.AddWithValue("@CreatedAt", item.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", item.UpdatedAt);
            int id = (int)cmd.ExecuteScalar();

            string query2 = $"insert into [items_ships] (item_id, ship_id, created_at, updated_at) values ('{id}', '{ship_id}', @CreatedAt, @UpdatedAt)";
            cmd = new SqlCommand(query2, conn);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static void Update(Item item)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"Update item set name = '{item.Name}' , quantity = '{item.Quantity}' , price = '{item.Price}', currency = '{item.Currency}', updated_at = @UpdatedAt where id = '{item.id}'";
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
                int
                    Id = (int)dr[0],
                    TraderId = (int)dr[5],
                    Quantity = (int)dr[2];
                Decimal
                    Price = (Decimal)dr[3];
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
                    Quantity = (int)dr[2];
                Decimal
                    Price = (Decimal)dr[3];
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
                    Quantity = (int)dr[2];
                Decimal
                    Price = (Decimal)dr[3];
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
