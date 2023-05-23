namespace BlazorEcommerce.Server.Data
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}