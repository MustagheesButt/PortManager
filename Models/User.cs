using System;
using System.Collections.Generic;

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

        private static User[] arr = {
            new User(1, "Test User", "#1", "test1@yahoo.com", 0, 1),
            new User(2, "Test User", "#2", "test1@yahoo.com", 1, 1),
            new User(3, "Test User", "#3", "test1@yahoo.com", 2, 1),
            new User(4, "Test User", "#4", "test1@yahoo.com", 1, 1),
            new User(5, "Test User", "#5", "test1@yahoo.com", 0, 1)
        };

        public static List<User> GetUsers()
        {
            return new List<User>(arr);
        }

        public static User GetUser(int user_id)
        {
            return Array.Find<User>(arr, e => e.id == user_id);
        }
    }
}
