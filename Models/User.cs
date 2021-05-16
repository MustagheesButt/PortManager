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

        public User(int id , string FirstName, string LastName, string Email, int Type, int Gender=0, string PasswordHash=null, DateTime? CreatedAt=null, DateTime? UpdatedAt=null)
        {
            this.id = id ;
            this.FirstName    = FirstName;
            this.LastName     = LastName;
            this.Email        = Email;
            this.PasswordHash = PasswordHash;
            this._Gender      = Gender;
            this._Type        = Type;

            this.CreatedAt = (DateTime)CreatedAt;
            this.UpdatedAt = (DateTime)UpdatedAt;
        }

        public static void Add_User(User obj)
        {
            // TODO check if user exists in DB. if yes, then throw exception
            string query = $"insert into [user] (first_name, last_name, email, password_hash, cnic, user_type, gender, created_at, updated_at) values ('{obj.FirstName}', '{obj.LastName}', '{obj.Email}', '{obj.PasswordHash}' , '{obj.CNIC}', {obj._Type}, '{obj._Gender}', @CreatedAt, @UpdatedAt)";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.Parameters.AddWithValue("@CreatedAt", obj.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", obj.UpdatedAt);
            //cmd.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static List<User> GetUsers()
        {
            string query = $"select * from [user]";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<User> users = new List<User>();

            while (dr.Read())
            {
                int
                    Id = (int)dr[0],
                    UserType = (int)dr[6],
                    Gender = (int)dr[7];
                string
                    FirstName = dr[1].ToString(),
                    LastName = dr[2].ToString(),
                    Email = dr[3].ToString();
                DateTime
                    CreatedAt = (DateTime)dr[8],
                    UpdatedAt = (DateTime)dr[9];
                users.Add(new User(Id, FirstName, LastName, Email, UserType, Gender, CreatedAt: CreatedAt, UpdatedAt: UpdatedAt));
            }

            Helper.CloseConn();
            return users;
        }

        public static List<User> GetAllByType(int Type)
        {
            string query = $"SELECT * FROM [user] WHERE user_type = '{Type}'";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            List<User> users = new List<User>();

            while (dr.Read())
            {
                int
                    Id = (int)dr[0],
                    UserType = (int)dr[6],
                    Gender = (int)dr[7];
                string
                    FirstName = dr[1].ToString(),
                    LastName = dr[2].ToString(),
                    Email = dr[3].ToString();
                DateTime
                    CreatedAt = (DateTime)dr[8],
                    UpdatedAt = (DateTime)dr[9];
                users.Add(new User(Id, FirstName, LastName, Email, UserType, Gender, CreatedAt: CreatedAt, UpdatedAt: UpdatedAt));
            }

            Helper.CloseConn();
            return users;
        }

        public static User GetOne(int id)
        {
            List<User> arr = User.GetUsers();
            return arr.Find(e => e.id == id);
        }

        public static User GetUserByEmail(string email)
        {
            string query = $"select * from [user] where  email = '{email}'";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                int
                    Id = (int)dr[0],
                    UserType = (int)dr[6],
                    Gender = (int)dr[7];
                string
                    FirstName = dr[1].ToString(),
                    LastName = dr[2].ToString(),
                    Email = dr[3].ToString(),
                    PasswordHash = dr[4].ToString();
                DateTime
                    CreatedAt = (DateTime)dr[8],
                    UpdatedAt = (DateTime)dr[9];
                User user = new User(Id, FirstName, LastName, Email, UserType, Gender, PasswordHash, CreatedAt: CreatedAt, UpdatedAt: UpdatedAt);
                Helper.CloseConn();
                return user ;
            }
            else {
                Helper.CloseConn();
                return null;
            }
        }

        public static String hash(String Password)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        public static void Update(int user_id, string fname, string lname, string email)
        {
            // TODO check if user exists in DB. if yes, then throw exception
            string query = $"Update [user] set first_name = '{fname}' , last_name = '{lname}' , email = '{email}', updated_at = @UpdatedAt where id = '{user_id}'";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

        public static void Delete(int user_id)
        {
            string query = $"delete from [user] where id = '{user_id}' ";
            SqlCommand cmd = new SqlCommand(query, Helper.GetConn());
            cmd.ExecuteNonQuery();
            Helper.CloseConn();
        }

    }
}
