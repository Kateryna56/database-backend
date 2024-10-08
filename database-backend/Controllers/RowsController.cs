using database_backend.Classes;
using Microsoft.AspNetCore.Mvc;
using Table = database_backend.Classes.Table;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace database_backend.Controllers
{
    [Route("api/rows")]
    [ApiController]
    public class RowsController : ControllerBase
    {
        // GET: api/rows
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/rows/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/rows
        [HttpPost("{tableName}")]
        public Table? Post(string tableName, [FromBody] string[] rowStrings)
        {
            var database = Database.LoadFromDisk("db.json");
            var selectedTable = database.Tables.FirstOrDefault(t => t.Name == tableName);
            Row currentRow = new Row();
            if (selectedTable == null || selectedTable.ColumnNames.Count != rowStrings.Length)
            {
                return null;
            }
            else
            {
                for (var i = 0; i < rowStrings.Length; i++)
                {
                    string field = rowStrings[i];
                    var type = selectedTable.ColumnTypes[i];
                    if (!currentRow.Check(field, type))
                    {
                        return null;
                    }
                    currentRow.AddField(field);
                }
                selectedTable.AddRow(currentRow);
            }
            database.SaveToDisk("db.json");
            return selectedTable;
        }

        // PUT api/rows/5
        [HttpPut("{tableName}/{rowNumber}")]
        public Table? Put(string tableName, int rowNumber, [FromBody] string[] rowStrings)
        {
            var database = Database.LoadFromDisk("db.json");
            var selectedTable = database.Tables.FirstOrDefault(t => t.Name == tableName);
            if (selectedTable == null || selectedTable.ColumnNames.Count != rowStrings.Length)
            {
                return null;
            }
            else
            {
                try
                {
                    var currentRow = selectedTable.Rows.ElementAt(rowNumber);
                    for (var i = 0; i < currentRow.Fields.Count; i++)
                    {
                        string field = rowStrings[i];
                        var type = selectedTable.ColumnTypes[i];
                        if (!currentRow.Check(field, type))
                        {
                            return null;
                        }
                        currentRow.Fields[i] = field;
                    }
                    for(var i = currentRow.Fields.Count; i < rowStrings.Length ; i++)
                    {
                        string field = rowStrings[i];
                        var type = selectedTable.ColumnTypes[i];
                        if (!currentRow.Check(field, type))
                        {
                            return null;
                        }
                        currentRow.AddField(field);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            database.SaveToDisk("db.json");
            return selectedTable;
        }


        // DELETE api/rows
        [HttpDelete("{tableName}/{rowNumber}")]
        public Table? Delete(string tableName, int rowNumber)
        {
            try
            {
                var database = Database.LoadFromDisk("db.json");
                var selectedTable = database.Tables.FirstOrDefault(t => t.Name == tableName);
                if (selectedTable == null)
                {
                    return null;
                }
                else
                {
                    selectedTable.Rows.RemoveAt(rowNumber);
                }
                database.SaveToDisk("db.json");
                return selectedTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
