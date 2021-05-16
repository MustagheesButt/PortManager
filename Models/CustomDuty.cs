using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace PortManager.Models
{
    public class CustomDuty
    {
        public int id { get; set; }
        public int ship_id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public int ImportOrExport { get; set; }
        public DateTime? DueDate   { get; set; }
        public DateTime? PaidAt    { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CustomDuty(int id, int ship_id, decimal Amount=0, string Currency="PKR", DateTime? DueDate=null, DateTime? PaidAt=null, string Status="Unpaid", int ImportOrExport=0, DateTime? CreatedAt=null, DateTime? UpdatedAt=null)
        {
            this.id       = id;
            this.Amount   = Amount;
            this.Currency = Currency;
            this.Status   = Status;
            this.ImportOrExport = ImportOrExport;
            this.ship_id  = ship_id;
            this.DueDate  = DueDate;
            this.PaidAt   = PaidAt;

            this.CreatedAt = CreatedAt == null ? DateTime.Now : (DateTime)CreatedAt;
            this.UpdatedAt = UpdatedAt == null ? DateTime.Now : (DateTime)UpdatedAt;
        }

        public static void Add(CustomDuty cd)
        {
            string query = $"INSERT INTO [custom_duty] (ship_id, amount, currency, status, imp_exp, due_date, created_at, updated_at) OUTPUT Inserted.ID VALUES({cd.ship_id}, @Amount, '{cd.Currency}', @Status, @ImpExp, @DueDate, @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.Parameters.AddWithValue("@Amount",    cd.Amount);
            cmd.Parameters.AddWithValue("@Status",    cd.Status);
            cmd.Parameters.AddWithValue("@ImpExp",    cd.ImportOrExport);
            cmd.Parameters.AddWithValue("@DueDate",   cd.DueDate);
            //cmd.Parameters.AddWithValue("@PaidAt",    cd.PaidAt);
            cmd.Parameters.AddWithValue("@CreatedAt", cd.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", cd.UpdatedAt);
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static void Update(CustomDuty cd)
        {
            // TODO check if ship exists in DB. if yes, then throw exception
            string query = $"UPDATE custom_duty set amount = '{cd.Amount}', currency = '{cd.Currency}', status = '{cd.Status}', due_date = @DueDate, paid_at = @PaidAt, updated_at = @UpdatedAt where id = {cd.id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.Parameters.AddWithValue("@DueDate",   cd.DueDate);
            cmd.Parameters.AddWithValue("@PaidAt",    cd.PaidAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", cd.UpdatedAt);
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static void DeleteById(int id)
        {
            string query = $"delete from [custom_duty] WHERE id = {id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static List<CustomDuty> GetAll()
        {
            string query = $"select * from [custom_duty]";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<CustomDuty> custom_duties = new List<CustomDuty>();

            while (dr.Read())
            {
                custom_duties.Add(Helper.ReadCustomDuty(dr));
            }

            Helper.CloseConn();
            return custom_duties;
        }

        public static CustomDuty GetOne(int id)
        {
            string query = $"select * from [custom_duty] where id = {id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            CustomDuty cd = null;
            if (dr.Read())
            {
                cd = Helper.ReadCustomDuty(dr);
            }

            Helper.CloseConn();
            return cd;
        }

        public static List<CustomDuty> GetAllByShip(int ship_id)
        {
            string query = $"select * from [custom_duty] where ship_id = {ship_id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<CustomDuty> custom_duties = new List<CustomDuty>();

            while (dr.Read())
            {
                custom_duties.Add(Helper.ReadCustomDuty(dr));
            }

            Helper.CloseConn();
            return custom_duties;
        }

        public static List<CustomDuty> GetAllByTrader(int trader_id)
        {
            string query = $"SELECT * FROM [custom_duty] INNER JOIN [ship] ON custom_duty.ship_id = [ship].id WHERE [ship].trader_id = {trader_id}";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<CustomDuty> custom_duties = new List<CustomDuty>();

            while (dr.Read())
            {
                custom_duties.Add(Helper.ReadCustomDuty(dr));
            }

            Helper.CloseConn();
            return custom_duties;
        }

        public static List<string> ChartData(int state=0)
        {
            string query = $"SELECT SUM(amount), paid_at FROM [custom_duty] WHERE paid_at IS NOT NULL AND imp_exp={state} GROUP BY paid_at";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<string> custom_duties = new List<string>();

            while (dr.Read())
            {
                custom_duties.Add(((Decimal)dr[0]).ToString());
                custom_duties.Add(((DateTime)dr[1]).ToShortDateString());
            }

            Helper.CloseConn();
            return custom_duties;
        }
    }
}
