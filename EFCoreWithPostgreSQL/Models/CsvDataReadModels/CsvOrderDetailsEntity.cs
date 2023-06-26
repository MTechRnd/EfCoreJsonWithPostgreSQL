namespace EFCoreJsonApp.Models.CsvDataReadModels
{
    public class CsvOrderDetailsEntity
    {
        public Guid OrderId { get; set; }
        public string ItemName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}
