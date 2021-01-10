using System;
using System.Collections.Generic;
using System.Linq;

namespace PortManager.Models
{
    public class CustomDuty
    {
        public int id { get; set; }
        public int ship_id { get; set; }
        public decimal Amount { get; set; }
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
            new CustomDuty(1, 1500, DateTime.Now, "Unpaid", 5),
            new CustomDuty(2, 10000, DateTime.Now, "Unpaid", 4),
            new CustomDuty(3, 9500, DateTime.Now, "Unpaid", 3),
            new CustomDuty(4, 150000, DateTime.Now, "Unpaid", 2),
            new CustomDuty(5, 550000, DateTime.Now, "Unpaid", 1)
        };

        public static List<CustomDuty> GetCustomDuties()
        {
            return new List<CustomDuty>(arr);
        }

        public static List<CustomDuty> GetCustomDutiesByShip(int ship_id)
        {
            return arr.Where<CustomDuty>(s => s.ship_id == ship_id).ToList();
        }
    }
}
