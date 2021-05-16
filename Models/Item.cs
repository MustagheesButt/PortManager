using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace PortManager.Models
{
    public class Item
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public Decimal Price { get; set; }
        public string Currency { get; set; }
        public int trader_id { get; set; }
        public int _Category;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Category
        {
            get
            {
                return CategoryMap[this._Category];
            }
        }
        public static string[] CategoryMap = {
            "Others", "Vehicles", "Electronics", "Food", "Clothing", "Petroleum", "Computers & Mobiles",
            "Chemicals", "Medical & Optical", "Gems & Metals", "Heavy Machinery", "Sports Equipment"
        };

        public Item(int id, string Name, int trader_id, Decimal Price=0, string Currency="PKR", string Manufacturer="", int Category=0, DateTime? CreatedAt=null, DateTime? UpdatedAt=null)
        {
            this.id        = id;
            this.Name      = Name;
            this.Manufacturer = Manufacturer;
            this.trader_id = trader_id;
            this.Price     = Price;
            this.Currency  = Currency;
            this._Category = Category;

            this.CreatedAt = CreatedAt == null ? DateTime.Now : (DateTime)CreatedAt;
            this.UpdatedAt = UpdatedAt == null ? DateTime.Now : (DateTime)UpdatedAt;
        }

        public static void Add(Item item)
        {
            string query = $"INSERT INTO [item] (name, price, currency, manufacturer, trader_id, category, created_at, updated_at) VALUES('{item.Name}', @Price, '{item.Currency}', '{item.Manufacturer}', @TraderId, @Category, @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.Parameters.AddWithValue("@Price",     item.Price);
            cmd.Parameters.AddWithValue("@TraderId",  item.trader_id);
            cmd.Parameters.AddWithValue("@Category",  item._Category);
            cmd.Parameters.AddWithValue("@CreatedAt", item.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", item.UpdatedAt);
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static void Update(Item item)
        {
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"Update item set name = '{item.Name}', price = '{item.Price}', currency = '{item.Currency}', manufacturer = '{item.Manufacturer}', category = @Category, updated_at = @UpdatedAt where id = '{item.id}'";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.Parameters.AddWithValue("@Category",  item._Category);
            cmd.Parameters.AddWithValue("@UpdatedAt", item.UpdatedAt);
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static void DeleteById(int id)
        {
            string query = $"delete from [item] WHERE id = {id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static List<Item> GetAll()
        {
            string query = $"select * from [item]";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<Item> items = new List<Item>();

            while (dr.Read())
            {                
                items.Add(Helper.ReadItem(dr));
            }

            Helper.CloseConn();
            return items;
        }

        public static Item GetOne(int id)
        {
            string query = $"select * from [item] where id = '{id}'";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            Item item = null;
            if (dr.Read())
            {
                item = Helper.ReadItem(dr);
            }

            Helper.CloseConn();
            return item;
        }

        public static List<Item> GetAllByTrader(int trader_id)
        {
            string query = $"select * from [item] where trader_id = '{trader_id}'";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<Item> items = new List<Item>();

            while (dr.Read())
            {
                items.Add(Helper.ReadItem(dr));
            }

            Helper.CloseConn();
            return items;
        }

        public int QuantityOnShip(int ship_id)
        {
            string query = $"SELECT * FROM [items_ships] WHERE item_id = {this.id} AND ship_id = {ship_id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            int quantity = -1;
            if (dr.Read())
                quantity = (int)dr[2];

            Helper.CloseConn();
            return quantity;
        }

        public List<Ship> Ships()
        {
            string query = $"SELECT * FROM [items_ships] WHERE item_id = {this.id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<Ship> ships = new List<Ship>();
            while (dr.Read())
                ships.Add(Ship.GetShip((int)dr[1]));

            Helper.CloseConn();
            return ships;
        }

        public static int[] CategoryCount(int state=0)
        {
            List<Ship> ships = Ship.GetShips().FindAll(s => s._Status == state);
            int[] counters = new int[CategoryMap.Length];
            for (int i = 0; i < CategoryMap.Length; i++)
            {
                ships.ForEach(s =>
                {
                    counters[i] += s.Items().FindAll(item => item._Category == i).Count;
                });
            }

            return counters;
        }
    }
}
