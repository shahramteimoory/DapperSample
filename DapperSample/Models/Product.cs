namespace DapperSample.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public int Size { get; set; }
        public string Nooeh { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public DateTime Insert_DateTime { get; set; } = DateTime.Now;
        public long Insert_ByUserID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Delete_DateTime { get; set; }
        public long Delete_ByUserID { get; set; }
        public DateTime Update_DateTime { get; set; }
        public long Update_ByUserID { get; set; }
    }
}
