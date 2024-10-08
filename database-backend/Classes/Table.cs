using System.Text.Json;
using System.Text.Json.Serialization;

namespace database_backend.Classes
{
    public class Table
    {
        public string Name { get; set; }
        public List<string> ColumnNames { get; set; }
        public List<string> ColumnTypes { get; set; }
        public List<Row> Rows { get; set; }

        public Table(string name)
        {
            Name = name;
            ColumnNames = new List<string>();
            ColumnTypes = new List<string>();
            Rows = new List<Row>();
        }

        public void AddColumn(string columnName, string columnType)
        {
            ColumnNames.Add(columnName);
            ColumnTypes.Add(columnType);
        }

        public void AddRow(Row row)
        {
            if (row.Fields.Count == ColumnNames.Count)
            {
                Rows.Add(row);
            }
            else
            {
                throw new Exception("Row does not match table schema.");
            }
        }

        public void Save(StreamWriter writer)
        {
            // Сохраняем таблицу в JSON формате
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            writer.Write(json);
        }

        public static Table Load(StreamReader reader, string tableName)
        {
            // Загружаем таблицу из JSON строки
            string json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<Table>(json);
        }

        public Table Join(Table otherTable, string thisColumnName, string otherColumnName)
        {
            int thisColumnIndex = ColumnNames.IndexOf(thisColumnName);
            int otherColumnIndex = otherTable.ColumnNames.IndexOf(otherColumnName);

            if (thisColumnIndex == -1 || otherColumnIndex == -1)
            {
                throw new Exception("One of the columns not found");
            }

            if (ColumnTypes[thisColumnIndex] != otherTable.ColumnTypes[otherColumnIndex])
            {
                throw new Exception("Column types does not match");
            }

            Table resultTable = new Table("JoinedTable" + DateTime.Now.ToString());

            foreach (var colName in ColumnNames)
            {
                resultTable.AddColumn($"{Name}.{colName}", ColumnTypes[ColumnNames.IndexOf(colName)]);
            }

            foreach (var colName in otherTable.ColumnNames)
            {
                resultTable.AddColumn($"{otherTable.Name}.{colName}", otherTable.ColumnTypes[otherTable.ColumnNames.IndexOf(colName)]);
            }

            foreach (var row in Rows)
            {
                var firstField = row.Fields[thisColumnIndex];

                foreach (var otherRow in otherTable.Rows)
                {
                    var secondField = otherRow.Fields[otherColumnIndex];
                    if (firstField == secondField)
                    {
                        var combinedFields = new List<string>();

                        foreach (var field in row.Fields)
                        {
                            combinedFields.Add(field);
                        }

                        foreach (var field in otherRow.Fields)
                        {
                            combinedFields.Add(field);
                        }

                        resultTable.AddRow(new Row(combinedFields));
                    }
                }
            }

            return resultTable;
        }
    }
}
