using database_backend.Classes;
using Microsoft.AspNetCore.Mvc;

namespace database_backend.Controllers
{
    [Route("api/database")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        // GET: api/database
        [HttpGet]
        public Database Get()
        {
            var database = Database.LoadFromDisk("db.json");
            return database;
        }

        // POST api/database
        [HttpPost]
        public void Post([FromBody] string dbName)
        {
            var database = new Database(dbName);
            database.SaveToDisk("db.json");
        }

        // PUT api/database/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/database/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
