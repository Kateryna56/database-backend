using database_backend.Classes;
using NUnit.Framework;
using System;

namespace database_backend.Tests
{
    [TestFixture]
    public class MainTests
    {
        [Test]
        public void Validate_IntField_ValidInt_ReturnsTrue()
        {
            var row = new Row();

            row.AddField("1");

            row.AddField("text");

            row.AddField("[1; 10]");

            bool isIntegerValid = row.Check(row.Fields[0], "integer");

            bool isTextValid = row.Check(row.Fields[1], "string");

            bool isTextInteger = row.Check(row.Fields[1], "integer");

            bool isIntegerIntervalValid = row.Check(row.Fields[2], "integerinvl");

            Assert.That(isIntegerValid, Is.True);
            Assert.That(isTextValid, Is.True);
            Assert.That(isTextInteger, Is.False);
            Assert.That(isIntegerIntervalValid, Is.True);
        }

        [Test]
        public void Join_TablesWithMatchingFields_ReturnsCorrectJoinedTable()
        {
            // Arrange
            var table1 = new Table("Table1");
            table1.AddColumn("ID", "integer");
            table1.AddColumn("Name", "string");
            table1.AddRow(new Row(new List<string> { "1", "Alice" }));
            table1.AddRow(new Row(new List<string> { "2", "Bob" }));

            var table2 = new Table("Table2");
            table2.AddColumn("ID", "integer");
            table2.AddColumn("Age", "integer");
            table2.AddRow(new Row(new List<string> { "1", "25" }));
            table2.AddRow(new Row(new List<string> { "3", "30" }));

            // Act
            var joinedTable = table1.Join(table2, "ID", "ID");

            // Assert
            Assert.That(joinedTable.Rows.Count, Is.EqualTo(1));
            Assert.That(joinedTable.Rows[0].Fields.Count, Is.EqualTo(4));
            Assert.That(joinedTable.Rows[0].Fields[0], Is.EqualTo("1"));
            Assert.That(joinedTable.Rows[0].Fields[1], Is.EqualTo("Alice"));
            Assert.That(joinedTable.Rows[0].Fields[2], Is.EqualTo("1"));
            Assert.That(joinedTable.Rows[0].Fields[3], Is.EqualTo("25"));
        }

        [Test]
        public void SaveAndLoad_SimpleTable_ReturnsSameTable()
        {
            // Arrange
            var table = new Table("TestTable");
            table.AddColumn("ID", "integer");
            table.AddColumn("Name", "string");
            table.AddRow(new Row(new List<string> { "1", "Alice" }));
            table.AddRow(new Row(new List<string> { "2", "Bob" }));
            
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            table.Save(writer);
            writer.Flush();
            stream.Position = 0;
            
            var reader = new StreamReader(stream);
            var loadedTable = Table.Load(reader, "TestTable");

            // Assert
            Assert.That(loadedTable.Name, Is.EqualTo("TestTable"));
            Assert.That(loadedTable.ColumnNames, Is.EqualTo(table.ColumnNames));
            Assert.That(loadedTable.ColumnTypes, Is.EqualTo(table.ColumnTypes));
            Assert.That(loadedTable.Rows.Count, Is.EqualTo(2));
            Assert.That(loadedTable.Rows[0].Fields[0], Is.EqualTo("1"));
            Assert.That(loadedTable.Rows[0].Fields[1], Is.EqualTo("Alice"));
        }

    }
}
