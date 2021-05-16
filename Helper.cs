using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using PortManager.Models;

namespace PortManager
{
    public class Helper
    {
        private static SqlConnection DbConn;
        public static String ConnString;

        public static SqlConnection GetConn()
        {
            DbConn = new SqlConnection(ConnString);
            DbConn.Open();

            return DbConn;
        }

        public static void CloseConn()
        {
            DbConn.Close();
        }

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

        public static Item ReadItem(SqlDataReader dr)
        {
            int
                Id = (int)dr[0],
                TraderId = (int)dr[5],
                Category = (int)dr[6];
            Decimal
                Price = (Decimal)dr[2];
            string
                Name = (string)dr[1],
                Manufacturer = dr[4] == DBNull.Value ? "" : (string)dr[4],
                Currency = (string)dr[3];
            DateTime
                CreatedAt = (DateTime)dr[7],
                UpdatedAt = (DateTime)dr[8];
            return new Item(Id, Name, TraderId, Price, Currency, Manufacturer, Category, CreatedAt, UpdatedAt);
        }

        public static Ship ReadShip(SqlDataReader dr)
        {
            int
                Id = (int)dr[0],
                TraderId = (int)dr[2],
                AllocatedBirth = (int)dr[4],
                AllocatedTerminal = (int)dr[5],
                Status = (int)dr[6];
            string
                HIN = (string)dr[1],
                NickName = (string)dr[3];
            DateTime?
                ClearedAt = (dr[7] == DBNull.Value) ? null : (DateTime)dr[7],
                CreatedAt = (DateTime)dr[8],
                UpdatedAt = (DateTime)dr[9];

            return new Ship(Id, HIN, TraderId, NickName, AllocatedBirth, AllocatedTerminal, Status, ClearedAt, CreatedAt, UpdatedAt);
        }

        public static CustomDuty ReadCustomDuty(SqlDataReader dr)
        {
            int
                Id = (int)dr[0],
                ShipId = (int)dr[3],
                ImportOrExport = (int)dr[5];
            Decimal
                Amount = (Decimal)dr[1];
            string
                Status = (string)dr[4],
                Currency = (string)dr[2];
            DateTime?
                DueDate = (DateTime)dr[6],
                PaidAt = (dr[7] == DBNull.Value) ? null : (DateTime)dr[7],
                CreatedAt = (DateTime)dr[8],
                UpdatedAt = (DateTime)dr[9];

            return new CustomDuty(Id, ShipId, Amount, Currency, DueDate, PaidAt, Status, ImportOrExport, CreatedAt, UpdatedAt);
        }
    }
}
