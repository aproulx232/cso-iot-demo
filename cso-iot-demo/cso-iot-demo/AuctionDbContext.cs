using Microsoft.EntityFrameworkCore;

namespace cso_iot_demo
{
    public class AuctionDbContext : DbContext
    {

        public AuctionDbContext(DbContextOptions options) : base(options) { }

        
    }
}
