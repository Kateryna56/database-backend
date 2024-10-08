using database_backend.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace database_backend.Controllers
{
    [Route("api/tables")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        // GET: api/tables
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var database = Database.LoadFromDisk("db.json");
            var res = database.Tables.Select(table => table.Name);
            return res;
        }

        // GET api/tables/5
        [HttpGet("{name}")]
        public Table? Get(string name)
        {
            var database = Database.LoadFromDisk("db.json");
            var table = database.Tables.Find(x => x.Name == name);
            return table;
        }

        // POST api/tables/join
        [HttpPost("join")]
        public Table? JoinTables([FromBody] JoinTablesRequest request)
        {
            var database = Database.LoadFromDisk("db.json");

            var table1 = database.Tables.FirstOrDefault(t => t.Name == request.joinTable1);
            var table2 = database.Tables.FirstOrDefault(t => t.Name == request.joinTable2);

            var resultTable = table1?.Join(table2, request.joinColumn1, request.joinColumn2);

            if (resultTable != null)
            {
                database.AddTable(resultTable);
                database.SaveToDisk("db.json");
            }

            return resultTable;
        }

        // POST api/tables
        [HttpPost]
        public void Post([FromBody] string name)
        {
            var database = Database.LoadFromDisk("db.json");
            var newTable = new Table(name);
            database.AddTable(newTable);
            database.SaveToDisk("db.json");
        }

        // POST api/tables/column
        [HttpPost("{tableName}/column")]
        public void AddColumn(string tableName, string columnName, string dataType)
        {
            var database = Database.LoadFromDisk("db.json");
            var table = database.Tables.FirstOrDefault(t => t.Name == tableName);
            if (table != null)
            {
                table.AddColumn(columnName, dataType);
                database.SaveToDisk("db.json");
            }
        }

        // PUT api/tables/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/tables/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            var database = Database.LoadFromDisk("db.json");
            database.RemoveTable(name);
            database.SaveToDisk("db.json");
        }
    }
}
