using Lesson3_CNLTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace Lesson3_CNLTWeb.Data
{
    public class RoomManagementDbContextBCS240032 : DbContext
    {
        public RoomManagementDbContextBCS240032(DbContextOptions<RoomManagementDbContextBCS240032> options) : base(options)
        {
        }

        public DbSet<RoomTypeBCS240032> RoomTypes { get; set; }
        public DbSet<RoomBCS240032> Rooms { get; set; }
        public DbSet<RoomImageBCS240032> RoomImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoomTypeBCS240032>(entity =>
            {
                entity.ToTable("RoomTypes_BCS240032");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasMany(e => e.Rooms)
                    .WithOne(r => r.RoomType)
                    .HasForeignKey(r => r.RoomTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RoomBCS240032>(entity =>
            {
                entity.ToTable("Rooms_BCS240032");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Area).HasColumnType("float");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.IsAvailable).HasDefaultValue(true);

                entity.HasIndex(e => new { e.Name, e.RoomTypeId }).IsUnique();

                entity.HasMany(e => e.RoomImages)
                    .WithOne(i => i.Room)
                    .HasForeignKey(i => i.RoomId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.RoomTypeId);
                entity.HasIndex(e => e.IsAvailable);
                entity.HasIndex(e => e.Price);
            });

            modelBuilder.Entity<RoomImageBCS240032>(entity =>
            {
                entity.ToTable("RoomImages_BCS240032");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ImageUrl).HasMaxLength(500).IsRequired();
                entity.Property(e => e.IsThumbnail).HasDefaultValue(false);

                entity.HasIndex(e => new { e.RoomId, e.IsThumbnail });
                entity.HasIndex(e => e.RoomId)
                    .HasFilter("[IsThumbnail] = 1")
                    .IsUnique();
            });
        }
    }
}
