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
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CustomDuty(int id, decimal Amount, DateTime DueDate, string Status, int ship_id)
        {
            this.id      = id;
            this.Amount  = Amount;
            this.Status  = Status;
            this.ship_id = ship_id;
            this.DueDate = DueDate;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        private static CustomDuty[] arr = {
            new CustomDuty(1, 1500, DateTime.Now, "Unpaid", 1),
            new CustomDuty(2, 10000, DateTime.Now, "Unpaid", 1),
            new CustomDuty(3, 9500, DateTime.Now, "Unpaid", 1),
            new CustomDuty(4, 150000, DateTime.Now, "Paid", 1),
            new CustomDuty(5, 550000, DateTime.Now, "Unpaid", 1)
        };

        public static List<CustomDuty> GetAll()
        {
            return new List<CustomDuty>(arr);
        }

        public static List<CustomDuty> GetAllByShip(int ship_id)
        {
            return arr.Where<CustomDuty>(s => s.ship_id == ship_id).ToList();
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
    }
}
