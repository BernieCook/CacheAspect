
namespace CacheAspect.DomainModel
{
    public partial class NorthwindContext : IUnitOfWork
    {
        public NorthwindContext(string connectionString)
            : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
