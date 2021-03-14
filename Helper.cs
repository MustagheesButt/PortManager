using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using PortManager.Models;

namespace PortManager
{
    public class Helper
    {
        public static IActionResult Protect(ISession session)
        {
            if (CurrentUser(session) == null)
                return new RedirectResult("/Login");
            return null;
        }

        public static Models.User CurrentUser(ISession session)
        {
            if (session.GetInt32("user_id") != null)
                return Models.User.GetOne((int)session.GetInt32("user_id"));
            return null;
        }

        // ITEMS
        public static Item ReadItem(SqlDataReader dr)
        {
            int
                Id = (int)dr[0],
                TraderId = (int)dr[4];
            Decimal
                Price = (Decimal)dr[2];
            string
                Name = (string)dr[1],
                Currency = (string)dr[3];
            DateTime
                CreatedAt = (DateTime)dr[5],
                UpdatedAt = (DateTime)dr[6];
            return new Item(Id, Name, TraderId, Price, Currency, CreatedAt, UpdatedAt);
        }

        public static CustomDuty ReadCustomDuty(SqlDataReader dr)
        {
            int
                Id = (int)dr[0],
                ShipId = (int)dr[3];
            Decimal
                Amount = (Decimal)dr[1];
            string
                Status = (string)dr[4],
                Currency = (string)dr[2];
            DateTime?
                DueDate = (DateTime)dr[5],
                PaidAt = (dr[6] == DBNull.Value) ? null : (DateTime)dr[6],
                CreatedAt = (DateTime)dr[7],
                UpdatedAt = (DateTime)dr[8];

            return new CustomDuty(Id, ShipId, Amount, Currency, DueDate, PaidAt, Status, CreatedAt, UpdatedAt);
        }
    }
}
