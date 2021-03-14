using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace PortManager.Models
{
    public class Trader
    {
        public static String connString = @"";

        public int User_Id { get; set; }
        public string Nationality { get; set; }
        public string Trading_Number { get; set; }

        public Trader(int user_id , string nationality , string trading_number)
        {
            this.User_Id = user_id;
            this.Nationality = nationality;
            this.Trading_Number = trading_number;
        }
    }
}
