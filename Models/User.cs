using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace PortManager.Models
{
    public class User
    {
        public static string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PortManager;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string CNIC { get; set; }
        public int UserType { get; set; }
        private int _Gender;
        public string Gender
        {
            get {
                switch (this._Gender)
                {
                    case 0:
                        return "Prefer Not Say";
                    case 1:
                        return "Male";
                    case 2:
                        return "Female";
                    default:
                        return "GenderOutOfBoundsException";
                }
            }
        }
        private int _Type { get; set; }
        public string Type {
            get {
                switch (this._Type)
                {
                    case 0:
                        return "Admin";
                    case 1:
                        return "Trader";
                    case 2:
                        return "Port Staff";
                    default:
                        return "TypeOutOfBoundsException";
                }
            }
        }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FullName
        {
            get {
                return $"{this.FirstName} {this.LastName}";
            }
        }

        public User(int id, string FirstName, string LastName, string Email, int Gender, int Type)
        {
            this.id        = id;
            this.FirstName = FirstName;
            this.LastName  = LastName;
            this.Email     = Email;
            this._Gender   = Gender;
            this._Type     = Type;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
        
        public static void Add_User(User obj)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            string query = $"insert into [user] (first_name , last_name , email , password , cnic , user_type , gender , user_type , created_at , updated_at ) values ('{obj.FirstName}' , '{obj.LastName}' , '{obj.Email}' , '{obj.PasswordHash}' , '{obj.CNIC}' , '{obj.UserType}' , '{obj.CreatedAt}' , '{obj.UpdatedAt}')";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<User> GetUsers()
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [user]";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            List<User> users = new List<User>();

            while (dr.Read())
            {
                users.Add(new User((int)dr[0], (string)dr[1], (string)dr[2], (string)dr[2], (int)dr[3] , (int)dr[4]));
            }

            return users;
        }

        public static User GetUser(int user_id)
        {
            List<User> arr = User.GetUsers();
            return arr.Find(e => e.id == user_id);
        }

        public static User GetUserByEmail(string email)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [user] where  email = '{email}'  ";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            if(dr.Read())
            {
                return new User((int)dr[0], (string)dr[1], (string)dr[2], (string)dr[2], (int)dr[3] , (int)dr[4]);
            }

            else { return null; }

            
        }
    }
}
