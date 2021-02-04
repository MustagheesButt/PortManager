using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PortManager.Models
{
    public class User
    {
        public static String connString = @"";

        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string CNIC { get; set; }
        public int _Gender;
        public string Gender
        {
            get
            {
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
        public string Type
        {
            get
            {
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
            get
            {
                return $"{this.FirstName} {this.LastName}";
            }
        }

        // use this at registration
        public User(string FirstName, string LastName, string Email, string PasswordHash, int Type )
        {
            this.id = id ;
            this.FirstName    = FirstName;
            this.LastName     = LastName;
            this.Email        = Email;
            this.PasswordHash = PasswordHash;
            this._Gender      = 0;
            this._Type        = Type;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public User(int id , string FirstName, string LastName, string Email, string PasswordHash, int Type )
        {
            this.id = id ;
            this.FirstName    = FirstName;
            this.LastName     = LastName;
            this.Email        = Email;
            this.PasswordHash = PasswordHash;
            this._Gender      = 0;
            this._Type        = Type;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public User(int id, string FirstName, string LastName, string Email, int Type)
        {
            this.id        = id;
            this.FirstName = FirstName;
            this.LastName  = LastName;
            this.Email     = Email;
            //this._Gender   = Gender;
            this._Type     = Type;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
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

        public User(int id, string FirstName, string LastName, string Email, string PasswordHash, int Gender, int Type)
        {
            this.id = id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.PasswordHash = PasswordHash;
            this._Gender = Gender;
            this._Type = Type;

            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public static void Add_User(User obj)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if user exists in DB. if yes, then throw exception
            string query = $"insert into [user] (first_name, last_name, email, password_hash, cnic, user_type, gender, created_at, updated_at) values ('{obj.FirstName}', '{obj.LastName}', '{obj.Email}', '{obj.PasswordHash}' , '{obj.CNIC}', {obj._Type}, '{obj._Gender}', '{obj.CreatedAt}', '{obj.UpdatedAt}')";
            SqlCommand cmd = new SqlCommand(query, conn);
            //cmd.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
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
                int
                    Id = (int)dr[0],
                    UserType = (int)dr[6];
                    //Gender = (int)dr[7];
                string
                    FirstName = dr[1].ToString(),
                    LastName = dr[2].ToString(),
                    Email = dr[3].ToString();
                users.Add(new User(Id, FirstName, LastName, Email, UserType));
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

            if (dr.Read())
            {
                User user = new User((int)dr[0], (string)dr[1], (string)dr[2], (string)dr[3], (string)dr[4], (int)dr[6]);
                return user ;
            }
            else { return null; }
        }

        public static User GetUserByEmailAndPassword(string email , string password)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"select * from [user] where  email = '{email}' and  password_hash = '{password}'  ";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                return new User((int)dr[0], (string)dr[1], (string)dr[2], (string)dr[3], (string)dr[4] ,  (int)dr[6]);
            }
            else { return null; }
        }

        public static String hash(String Password)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        public static void Add_Trader(string fname , string lname , string email , string password , int type)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if user exists in DB. if yes, then throw exception
            string query = $"insert into [user] (first_name, last_name, email, password_hash, user_type , created_at) values ('{fname}', '{lname}', '{email}', '{password}' , '{type}' , '{System.DateTime.Now}')";
            SqlCommand cmd = new SqlCommand(query, conn);
            //cmd.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Add_Staff(string fname , string lname , string email , string password , int type)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if user exists in DB. if yes, then throw exception
            string query = $"insert into [user] (first_name, last_name, email, password_hash, user_type , created_at) values ('{fname}', '{lname}', '{email}', '{password}' , '{type}' , '{System.DateTime.Now}' )";
            SqlCommand cmd = new SqlCommand(query, conn);
            //cmd.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Add_Admin(string fname , string lname , string email , string password , int type)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if user exists in DB. if yes, then throw exception
            string query = $"insert into [user] (first_name, last_name, email, password_hash, user_type , created_at) values ('{fname}', '{lname}', '{email}', '{password}' , '{type}' , '{System.DateTime.Now}' )";
            SqlCommand cmd = new SqlCommand(query, conn);
            //cmd.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Edit_User(int user_id , string fname , string lname , string email , string password)
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            // TODO check if user exists in DB. if yes, then throw exception
            string query = $"Update [user] set first_name = '{fname}' , last_name = '{lname}' , email = '{email}' , password_hash = '{password}' , updated_at = '{System.DateTime.Now}' where id = '{user_id}' ";
            //cmd.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void DeleteUser(int user_id)
        {
        
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            string query = $"delete from [user] where id = '{user_id}' ";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        
        }
    }
}
