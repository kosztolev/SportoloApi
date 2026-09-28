using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using SportoloApi.Models;

namespace SportoloApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;database=sportolo13b;";

        [HttpGet]
        public List<Eredmeny> GetEredmenyek()
        {
            List<Eredmeny> eredmenyek = new List<Eredmeny>();
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = "SELECT * FROM eredmeny";
            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var eredmeny = new Eredmeny
                {
                    Id = data.GetInt32("id"),
                    Competition = data.GetString("Competition"),
                    Description = data.GetString("Description"),
                    ResultTime = data.GetDateTime("ResultTime"),
                    UpdateTime = data.GetDateTime("UpdateTime"),
                    SportoloId = data.GetInt32("SportoloId")
                };
                eredmenyek.Add(eredmeny);
            }
            
            connection.Close();
            return eredmenyek;
        }

        [HttpGet("{id}")]
        public Eredmeny GetById(int id)
        {
            Eredmeny eredmeny = null;

            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = "SELECT * FROM eredmeny WHERE Id = @Id;";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Id", id);

            var data = cmd.ExecuteReader();

            if (data.Read())
            {
                eredmeny = new Eredmeny
                {
                    Id = data.GetInt32("Id"),
                    Competition = data.GetString("Competition"),
                    Description = data.GetString("Description"),
                    ResultTime = data.GetDateTime("ResultTime"),
                    UpdateTime = data.GetDateTime("UpdateTime"),
                    SportoloId = data.GetInt32("SportoloId")
                };
            }

            connection.Close();
            return eredmeny;
        }

        [HttpPost]
        public string Create(Eredmeny ujEredmeny)
        {
            DateTime mostido = DateTime.Now;

            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = "INSERT INTO eredmeny (Competition, Description, ResultTime, UpdateTime, SportoloId) VALUES (@Competition, @Description, @ResultTime, @UpdateTime, @SportoloId);";
            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Competition", ujEredmeny.Competition);
            cmd.Parameters.AddWithValue("@Description", ujEredmeny.Description);
            cmd.Parameters.AddWithValue("@ResultTime", mostido);
            cmd.Parameters.AddWithValue("@UpdateTime", mostido);
            cmd.Parameters.AddWithValue("@SportoloId", ujEredmeny.SportoloId);

            cmd.ExecuteNonQuery();

            connection.Close();
            return "Új eredmény sikeresen rögzítve";
        }

        [HttpPut("{id}")]
        public string Update(int id, Eredmeny modositottEredmeny)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = "UPDATE eredmeny SET Competition = @Competition, Description = @Description, UpdateTime = @UpdateTime, SportoloId = @SportoloId WHERE Id = @Id;";
            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Competition", modositottEredmeny.Competition);
            cmd.Parameters.AddWithValue("@Description", modositottEredmeny.Description);
            cmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@SportoloId", modositottEredmeny.SportoloId);

            cmd.ExecuteNonQuery();

            connection.Close();
            return "Adat módosítva";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = "DELETE FROM eredmeny WHERE Id = @Id;";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();

            connection.Close();
            return "Adat törölve";
        }

        [HttpGet("sportolo/{id}")]
        public object GetSportolo(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"SELECT name, email FROM sportolo WHERE id = @Id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            var data = cmd.ExecuteReader();

            if (data.Read())
            {
                var sportolo = new
                {
                    Name = data.GetString("name"),
                    Email = data.GetString("email")
                };

                connection.Close();
                return sportolo;
            }
            connection.Close();

            return new
            {
                message = "Nincs ilyen sportoló."
            };
        }
        [HttpGet("sportolo/{id}/eredmenyek")]
        public object GetSportoloEredmenyek(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = @"SELECT sportolo.name, eredmeny.Competition, eredmeny.Description 
                         FROM sportolo INNER JOIN eredmeny ON sportolo.id = eredmeny.SportoloId 
                         WHERE sportolo.id = @Id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            var data = cmd.ExecuteReader();

            List<object> eredmenyek = new List<object>();

            while (data.Read())
            {
                eredmenyek.Add(new
                {
                    Name = data.GetString("name"),
                    Competition = data.GetString("Competition"),
                    Description = data.GetString("Description")
                });
            }
            connection.Close();

            return eredmenyek;
        }
        [HttpGet("eredmenyek/count")]
        public object GetEredmenyCount()
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = "SELECT COUNT(*) FROM eredmeny";
            var cmd = new MySqlCommand(sql, connection);
            long count = (long)cmd.ExecuteScalar();

            connection.Close();

            return new
            {
                count = count
            };
        }
    }
}
