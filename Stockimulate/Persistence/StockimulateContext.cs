using Microsoft.EntityFrameworkCore;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Stockimulate.Persistence
{
    class StockimulateContext : DbContext
    {
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Security> Securities { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Trader> Traders { get; set; }
        public virtual DbSet<Trade> Trades { get; set; }

        public StockimulateContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(30);
            })

                .Entity<Security>(entity =>
            {
                entity.HasKey(e => e.Symbol);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price);

                entity.Property(e => e.LastChange);
            })

                .Entity<Team>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            })

                .Entity<Trader>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Traders)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Players_0");
            })

                .Entity<Trade>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BrokerId)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.BuyerId).HasColumnName("BuyerID");

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.TradesAsBuyer)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TradesBuyerID_ToTraders");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.TradesAsSeller)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TradesSellerID_ToTraders");

                entity.HasOne(d => d.Security)
                    .WithMany(p => p.Trades)
                    .HasForeignKey(d => d.Symbol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trades_ToInstruments");
            });
        }
    }
}
