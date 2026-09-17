using EnhanzerAssignment.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EnhanzerAssignment.API.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(
           DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<LocationDetail> LocationDetails { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LocationDetail>()
                .ToTable("Location_Details");

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(po => po.Items)
                .WithOne(item => item.PurchaseOrder)
                .HasForeignKey(item => item.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(x => x.NetAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.StandardCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.StandardPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.Discount)
                .HasPrecision(5, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.TotalCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(x => x.TotalSelling)
                .HasPrecision(18, 2);
        }
    }
}
