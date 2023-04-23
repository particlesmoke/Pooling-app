using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Dapper;
using System.Data;
using Pooling_Backend.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pooling_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        readonly string connectionString = "database=railway;server=containers-us-west-159.railway.app;port=7894;uid=postgres;sslmode=allow;password=jHdCbDG2c2eftgSJwd6I";
        readonly IDbConnection connection;

        public UserController()
        {
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        ~UserController()
        {
            System.Diagnostics.Debug.WriteLine("Closing connection");
            connection.Close();
        }

        
        [HttpGet]
        public IEnumerable<UserProfile> Get()
        {

            string sql = "SELECT * FROM users";
            var result = connection.Query<UserProfile>(sql);
            return result;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public UserProfile Get(int id)
        {
            string sql = $"SELECT * FROM users where id = \'{id}\'";
            var result = connection.QueryFirst<UserProfile>(sql);
            return result;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] UserProfileInput userInput)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userInput.PlaintextPassword);
            string uid = userInput.Email.Split("@")[0].Replace("f20", "");
            string sql = $"INSERT INTO users (id, name, email, password, phone) VALUES ('{uid}', '{userInput.Name}', '{userInput.Email}', '{hashedPassword}', '{userInput.Phone}')";
            
            connection.Execute(sql);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string sql = $"DELETE FROM users WHERE id = '{id}'";
            connection.Execute(sql);
        }

        [HttpPost("/login")]
        public string Post (string email, string password)
        {
            string sql = $"SELECT email, password FROM users where email= '{email}'";
            var result = connection.QueryFirst<User>(sql);
            if (BCrypt.Net.BCrypt.Verify(password, result.Password))
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("particlesmoke111111111111111111111111"));
                var token = new JwtSecurityToken(signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return tokenString;
            }
            else
            {
                return "Error";
            }             

        }


    }
}
