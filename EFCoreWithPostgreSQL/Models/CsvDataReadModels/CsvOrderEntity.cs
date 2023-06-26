namespace EFCoreJsonApp.Models.CsvDataReadModels
{
    public class CsvOrderEntity
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
