namespace database_backend.Classes
{
    public class JoinTablesRequest
    {
        public string joinTable1 { get; set; }
        public string joinTable2 { get; set; }
        public string joinColumn1 { get; set; }
        public string joinColumn2 { get; set; }
    }
}
