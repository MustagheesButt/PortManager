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
        public int _Status { get; set; }
        public DateTime? ClearedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Status
        {
            get
            {
                return this._Status == 0 ? "Importing" : "Exporting";
            }
        }

        public Ship(int id, string HIN, int trader_id, string NickName, int AllocatedBirth, int AllocatedTerminal, int Status, DateTime? ClearedAt=null, DateTime? CreatedAt=null, DateTime? UpdatedAt=null)
        {
            this.id         = id;
            this.HIN        = HIN;
            this.trader_id  = trader_id;
            this.NickName   = NickName;
            this._Status    = Status;

            this.AllocatedBirth    = AllocatedBirth;
            this.AllocatedTerminal = AllocatedTerminal;

            this.ClearedAt = ClearedAt;
            this.CreatedAt = CreatedAt == null ? DateTime.Now : (DateTime)CreatedAt;
            this.UpdatedAt = UpdatedAt == null ? DateTime.Now : (DateTime)UpdatedAt;
        }

        public CustomDuty CustomDuty()
        {
            List<CustomDuty> duties = Models.CustomDuty.GetAllByShip(this.id).ToList();

            if (duties.Count == 0)
                return null;
            return duties.Last();
        }

        public static int Add(Ship ship)
        {
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"INSERT INTO [ship] (hin, trader_id, nick_name, allocated_birth, allocated_terminal, status, created_at, updated_at) OUTPUT Inserted.ID VALUES ('{ship.HIN}', {ship.trader_id}, @NickName, '{ship.AllocatedBirth}', '{ship.AllocatedTerminal}', @Status, @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.Parameters.AddWithValue("@NickName", ship.NickName);
            cmd.Parameters.AddWithValue("@Status", ship._Status);
            cmd.Parameters.AddWithValue("@CreatedAt", ship.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", ship.UpdatedAt);
            int id = (int)cmd.ExecuteScalar();

            Helper.CloseConn();
            return id;
        }

        public static void Update(Ship ship)
        {
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"Update ship set hin = '{ship.HIN}' , nick_name = @NickName , allocated_birth = '{ship.AllocatedBirth}', allocated_terminal = '{ship.AllocatedTerminal}', cleared_at = @ClearedAt, status = @Status, updated_at = @UpdatedAt where id = '{ship.id}';";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.Parameters.AddWithValue("@NickName", ship.NickName);
            cmd.Parameters.AddWithValue("@Status", ship._Status);
            cmd.Parameters.AddWithValue("@ClearedAt", ship.ClearedAt == null ? DBNull.Value : ship.ClearedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", ship.UpdatedAt);
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static void DeleteById(int id)
        {
            string query = $"delete from [ship] WHERE id = {id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static List<Ship> GetShips()
        {
            string query = $"select * from [ship]";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<Ship> ships = new List<Ship>();

            while (dr.Read())
            {
                ships.Add(Helper.ReadShip(dr));
            }
            Helper.CloseConn();

            return ships;
        }

        public static Ship GetShip(int ship_id)
        {
            string query = $"select * from [ship] where id = '{ship_id}' ";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            Ship ship = null;
            if (dr.Read())
            {                
                ship = Helper.ReadShip(dr);
            }
            Helper.CloseConn();
            return ship;
        }

        public static List<Ship> GetShipsByTrader(int trader_id)
        {
            string query = $"select * from [ship] where trader_id = '{trader_id}' ";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<Ship> ships = new List<Ship>();

            while (dr.Read())
            {
                ships.Add(Helper.ReadShip(dr));
            }

            Helper.CloseConn();
            return ships;
        }

        public void AttachItems(List<int> items, List<int> quantities)
        {
            // clear existing items
            SqlConnection conn = Helper.GetConn();
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
            Helper.CloseConn();
        }

        public void GenerateCustomDuty()
        {
            Decimal total = this.Items().Aggregate((decimal)0, (sum, next) => sum + next.Price * next.QuantityOnShip(this.id));
            Decimal duty = 0;
            if (this._Status == 0)
                duty = total * 0.27m;
            else
                duty = total * 0.15m;
            Models.CustomDuty.Add(new CustomDuty(-1, id, duty, DueDate: DateTime.Now.AddDays(15), ImportOrExport: this._Status));
        }

        public List<Item> Items()
        {
            string query = $"SELECT * FROM [items_ships] WHERE ship_id = {this.id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<Item> items = new List<Item>();
            while (dr.Read())
                items.Add(Item.GetOne((int)dr[0]));

            Helper.CloseConn();
            return items;
        }
    }
}
