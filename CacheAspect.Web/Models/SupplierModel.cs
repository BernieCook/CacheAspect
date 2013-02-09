using System.ComponentModel.DataAnnotations;

namespace CacheAspect.Web.Models.SupplierModel
{
    public class IndexModel
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
    }

    public class DetailModel
    {
        [Display(Name = "Id")]
        public int SupplierId { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact name")]
        public string ContactName { get; set; }

        [Display(Name = "Contact title")]
        public string ContactTitle { get; set; }
    }
}