using System.Text.Json;
using System.Text.Json.Serialization;

namespace database_backend.Classes
{
    public class Database
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; }

        public Database(string name)
        {
            Name = name;
            Tables = new List<Table>();
        }

        public void AddTable(Table table)
        {
            Tables.Add(table);
        }

        public void RemoveTable(string tableName)
        {
            Tables.RemoveAll(t => t.Name == tableName);
        }

        public void SaveToDisk(string filePath)
        {
            // Сохраняем базу данных и её таблицы в JSON формате
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static Database LoadFromDisk(string filePath)
        {
            // Загружаем базу данных из JSON файла
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Database>(json);
        }
    }
}
