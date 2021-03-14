using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace PortManager.Models
{
    public class CustomDuty
    {
        public static String connString = "";
        public int id { get; set; }
        public int ship_id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate   { get; set; }
        public DateTime? PaidAt    { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CustomDuty(int id, int ship_id, decimal Amount=0, string Currency="PKR", DateTime? DueDate=null, DateTime? PaidAt=null, string Status="Unpaid", DateTime? CreatedAt=null, DateTime? UpdatedAt=null)
        {
            this.id       = id;
            this.Amount   = Amount;
            this.Currency = Currency;
            this.Status   = Status;
            this.ship_id  = ship_id;
            this.DueDate  = DueDate;
            this.PaidAt   = PaidAt;

            this.CreatedAt = CreatedAt == null ? DateTime.Now : (DateTime)CreatedAt;
            this.UpdatedAt = UpdatedAt == null ? DateTime.Now : (DateTime)UpdatedAt;
        }

        public static void Add(CustomDuty cd)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"INSERT INTO [custom_duty] (ship_id, amount, currency, status, due_date, created_at, updated_at) OUTPUT Inserted.ID VALUES({cd.ship_id}, @Amount, '{cd.Currency}', @Status, @DueDate, @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Amount",    cd.Amount);
            cmd.Parameters.AddWithValue("@Status",    cd.Status);
            cmd.Parameters.AddWithValue("@DueDate",   cd.DueDate);
            //cmd.Parameters.AddWithValue("@PaidAt",    cd.PaidAt);
            cmd.Parameters.AddWithValue("@CreatedAt", cd.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", cd.UpdatedAt);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public static void Update(CustomDuty cd)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"UPDATE custom_duty set amount = '{cd.Amount}', currency = '{cd.Currency}', status = '{cd.Status}', due_date = @DueDate, paid_at = @PaidAt, updated_at = @UpdatedAt where id = {cd.id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@DueDate",   cd.DueDate);
            cmd.Parameters.AddWithValue("@PaidAt",    cd.PaidAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", cd.UpdatedAt);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void DeleteById(int id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"delete from [custom_duty] WHERE id = {id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<CustomDuty> GetAll()
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [custom_duty]";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<CustomDuty> custom_duties = new List<CustomDuty>();

            while (dr.Read())
            {
                custom_duties.Add(Helper.ReadCustomDuty(dr));
            }

            conn.Close();
            return custom_duties;
        }

        public static CustomDuty GetOne(int id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [custom_duty] where id = {id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            CustomDuty cd = null;
            if (dr.Read())
            {
                cd = Helper.ReadCustomDuty(dr);
            }

            conn.Close();
            return cd;
        }

        public static List<CustomDuty> GetAllByShip(int ship_id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [custom_duty] where ship_id = {ship_id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<CustomDuty> custom_duties = new List<CustomDuty>();

            while (dr.Read())
            {
                custom_duties.Add(Helper.ReadCustomDuty(dr));
            }

            conn.Close();
            return custom_duties;
        }

        public static List<CustomDuty> GetAllByTrader(int trader_id)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"SELECT * FROM [custom_duty] INNER JOIN [ship] ON custom_duty.ship_id = [ship].id WHERE [ship].trader_id = {trader_id}";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<CustomDuty> custom_duties = new List<CustomDuty>();

            while (dr.Read())
            {
                custom_duties.Add(Helper.ReadCustomDuty(dr));
            }

            conn.Close();
            return custom_duties;
        }
    }
}
