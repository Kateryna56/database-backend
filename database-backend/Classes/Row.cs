using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace database_backend.Classes
{
    public class Row
    {
        public List<string> Fields { get; set; }

        public Row()
        {
            Fields = new List<string>();
        }

        public Row(List<string> fields)
        {
            Fields = fields;
        }

        public void AddField(string field)
        {
            Fields.Add(field);
        }

        public void Save(StreamWriter writer)
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            writer.Write(json);
        }

        public static Row Load(string json)
        {
            return JsonSerializer.Deserialize<Row>(json);
        }

        public bool Check(string value, string dataType)
        {
            switch (dataType.ToLower())
            {
                case "integer":
                    return int.TryParse(value, out _);

                case "real":
                    return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out _);

                case "char":
                    return value.Length == 1;

                case "string":
                    return true;

                case "textfile":
                    return true;

                case "integerinvl":
                    return CheckInterval(value);

                default:
                    return false;
            }
        }

        private bool CheckInterval(string value)
        {
            string pattern = @"^\[\s*(-?\d+(\.\d+)?)\s*;\s*(-?\d+(\.\d+)?)\s*\]$"; // [x; y], where x, y - integers
            var match = Regex.Match(value, pattern);

            if (!match.Success)
            {
                return false;
            }
            double x = double.Parse(match.Groups[1].Value);
            double y = double.Parse(match.Groups[3].Value);
            return x <= y;
        }
    }
}
