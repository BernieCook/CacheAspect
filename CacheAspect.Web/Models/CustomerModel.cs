using System.ComponentModel.DataAnnotations;

namespace CacheAspect.Web.Models.CustomerModel
{
    public class IndexModel
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
    }

    public class DetailModel
    {
        [Display(Name = "Id")]
        public string CustomerId { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact name")]
        public string ContactName { get; set; }

        [Display(Name = "Contact title")]
        public string ContactTitle { get; set; }
    }
}