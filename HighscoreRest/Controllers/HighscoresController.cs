using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HighscoreModel;
using System.Data.SqlClient;

namespace HighscoreRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HighscoresController : ControllerBase
    {
        private static List<Highscore> highscoreList = new List<Highscore>();
        private static string conString;

    // GET: api/Highscores
    [HttpGet]
        public IEnumerable<Highscore> Get()
        {
            string connectionString = conString;
            
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                
                using (SqlCommand command = new SqlCommand("SELECT * FROM HighScore", con))

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Highscore obj = new Highscore();
                        obj.ID = Convert.ToInt32(reader["ID"]);
                        obj.Name = reader["Name"].ToString();
                        obj.Score = Convert.ToInt32(reader["Score"]);
                        
                        if (highscoreList.All(I => I.ID != Convert.ToInt32(reader["ID"])))
                        {
                            highscoreList.Add(obj);
                        }
                    }
                }

                con.Close();
            }

            return highscoreList;
        }

        // POST: api/Highscores
        [HttpPost]
        public void Post([FromBody] Highscore obj)
        {
            string connectionString = conString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                
                using (SqlCommand command =
                    new SqlCommand("INSERT INTO HighScore(Name, Score) VALUES(@Name, @Score)", con))
                {
                    command.Parameters.AddWithValue("@Name", obj.Name);
                    command.Parameters.AddWithValue("@Score", obj.Score);
                    command.ExecuteNonQuery();
                }

                con.Close();

            }

        }

        // PUT: api/Highscores/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
