using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Dapper;
using System.Data;
using Pooling_Backend.Models;
using BCrypt.Net;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pooling_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        readonly string connectionString = "database=pooling_app;server=localhost;port=5432;uid=postgres;sslmode=allow;password=8168988439";
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

            string sql = "SELECT * FROM \"user\"";
            var result = connection.Query<UserProfile>(sql);
            return result;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public UserProfile Get(int id)
        {
            string sql = $"SELECT * FROM \"user\" where id = \'{id}\'";
            var result = connection.QueryFirst<UserProfile>(sql);
            return result;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] UserProfileInput userInput)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userInput.PlaintextPassword);
            string uid = userInput.Email.Split("@")[0].Replace("f20", "");
            string sql = $"INSERT INTO \"user\" (id, name, email, password, phone) VALUES ('{uid}', '{userInput.Name}', '{userInput.Email}', '{hashedPassword}', '{userInput.Phone}')";
            
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
            string sql = $"DELETE FROM \"user\" WHERE id = '{id}'";
            connection.Execute(sql);
        }
    }
}
