namespace CacheAspect.Service
{
    public class SupplierDetailDto
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
    }

    public class SupplierDetailsDto
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
    }
}
