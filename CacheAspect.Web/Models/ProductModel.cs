using System.ComponentModel.DataAnnotations;

namespace CacheAspect.Web.Models.ProductModel
{
    public class IndexModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? UnitPrice { get; set; }

        public string UnitPriceWithCurrencySymbol
        {
            get { return UnitPrice.HasValue ? string.Concat("$", UnitPrice.Value.ToString("0.00")) : string.Empty; }
        }
    }

    public class DetailModel
    {
        [Display(Name = "Id")]
        public int ProductId { get; set; }
            
        [Display(Name = "Name")]
        public string ProductName { get; set; }

        [Display(Name = "Supplier id")]
        public int SupplierId { get; set; }

        [Display(Name = "Unit price")]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Units in stock")]
        public short? UnitsInStock { get; set; }

        [Display(Name = "Units on order")]
        public short? UnitsOnOrder { get; set; }
    }
}