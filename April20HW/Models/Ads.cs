using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace April20HW.Models
{
    public class Ads
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class DB
    {
        private string _connection;
        public DB(string cs)
        {
            _connection = cs;
        }
        public void SignUp(string Name, string email, string Password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(Password);
            using (var connection = new SqlConnection(_connection))
            using(var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"insert into Users(Name, Email, Hash)
                                    values(@name, @email, @hash)";
                cmd.Parameters.AddWithValue("@name", Name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@hash", hash);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Users GetByEmail(string email)
        {
            using (var connection = new SqlConnection(_connection))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"select * from Users where email =@email";
                cmd.Parameters.AddWithValue("@email", email);
                connection.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                return new Users
                {
                    Id = (int)reader["Id"],
                    Email = (string)reader["Email"],
                    Name = (string)reader["Name"],
                    Password = (string)reader["Hash"]
                };
            }
        }
        public Users LogIn(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (isValidPassword)
            {
                return user;
            }
            return null;
        }
        public void AddAd(Ads ad)
        {
            using (var connection = new SqlConnection(_connection))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"insert into Ads(Title, PhoneNumber, Description, UserId)
                                  values(@title, @PN, @Description, @UserId)";
                cmd.Parameters.AddWithValue("@title", ad.Title);
                cmd.Parameters.AddWithValue("@PN", ad.PhoneNumber);
                cmd.Parameters.AddWithValue("@Description", ad.Description);
                cmd.Parameters.AddWithValue("@UserId", ad.UserId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            
        }
        public List<Ads> GetAds()
        {
            using (var connection = new SqlConnection(_connection))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"select * from Ads";
                connection.Open();
                var result = new List<Ads>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Ads
                    {
                        Id = (int)reader["id"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        PhoneNumber = (string)reader["phoneNumber"],
                        UserId = (int)reader["userId"]
                    });
                }
                return result;
            }
        }
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connection))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"delete from Ads where id =@id";
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<Ads> GetAdsForUser(Users u)
        {
            using (var connection = new SqlConnection(_connection))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"select * from Ads where userId = @Id";
                cmd.Parameters.AddWithValue("@Id", u.Id);
                connection.Open();
                var result = new List<Ads>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Ads
                    {
                        Id = (int)reader["id"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        PhoneNumber = (string)reader["phoneNumber"],
                        UserId = (int)reader["userId"]
                    });
                }
                return result;
            }
        }
    }
}
