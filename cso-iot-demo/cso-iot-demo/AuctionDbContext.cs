using Microsoft.EntityFrameworkCore;

namespace cso_iot_demo
{
    public class AuctionDbContext : DbContext
    {
        public DbSet<RepairItem> RepairItems { get; set; }
        public AuctionDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureRepairItemEntity(modelBuilder);
        }

        private void ConfigureRepairItemEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RepairItem>()
                .ToTable("repair_item")
                .HasKey(item => new { item.RepairOrderId, item.RepairItemId });

            modelBuilder.Entity<RepairItem>()
                .Property(ri => ri.RepairOrderId)
                .HasColumnName("repair_order_id");

            modelBuilder.Entity<RepairItem>()
                .Property(ri => ri.RepairItemId)
                .HasColumnName("repair_item_id");

            modelBuilder.Entity<RepairItem>()
                .Property(ri => ri.CompleteFlag)
                .HasColumnName("complete_flg");

            modelBuilder.Entity<RepairItem>()
                .Property(ri => ri.PsiSource)
                .HasColumnName("psi_source");

            modelBuilder.Entity<RepairItem>()
                .Property(ri => ri.InspActionId)
                .HasColumnName("insp_action_id");

            modelBuilder.Entity<RepairItem>()
                .Property(ri => ri.PsiResult)
                .HasColumnName("psi_result");

            modelBuilder.Entity<RepairItem>()
                .Property(ri => ri.ShopTypeId)
                .HasColumnName("shop_type_id");
        }
    }
}
