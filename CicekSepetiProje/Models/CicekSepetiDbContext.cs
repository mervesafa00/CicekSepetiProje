using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CicekSepetiProje.Models;

public partial class CicekSepetiDbContext : DbContext
{
    public CicekSepetiDbContext()
    {
    }

    public CicekSepetiDbContext(DbContextOptions<CicekSepetiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kategoriler> Kategorilers { get; set; }

    public virtual DbSet<SilinenUrunLog> SilinenUrunLogs { get; set; }

    public virtual DbSet<Siparisler> Siparislers { get; set; }

    public virtual DbSet<Urunler> Urunlers { get; set; }

    public virtual DbSet<Uyeler> Uyelers { get; set; }

    public virtual DbSet<VwSiparisDetaylari> VwSiparisDetaylaris { get; set; }

    public virtual DbSet<Yorumlar> Yorumlars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-FO840AB;Database=CicekSepetiDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategoriler>(entity =>
        {
            entity.HasKey(e => e.KategoriId).HasName("PK__Kategori__1782CC92C865779D");

            entity.ToTable("Kategoriler");

            entity.Property(e => e.KategoriId).HasColumnName("KategoriID");
            entity.Property(e => e.KategoriAdi).HasMaxLength(50);
        });

        modelBuilder.Entity<SilinenUrunLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__SilinenU__5E5499A89BC06711");

            entity.ToTable("SilinenUrunLog");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.SilinmeTarihi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UrunAdi).HasMaxLength(100);
        });

        modelBuilder.Entity<Siparisler>(entity =>
        {
            entity.HasKey(e => e.SiparisId).HasName("PK__Siparisl__C3F03BDDAACB60EA");

            entity.ToTable("Siparisler", tb => tb.HasTrigger("trg_StokDusur"));

            entity.Property(e => e.SiparisId).HasColumnName("SiparisID");
            entity.Property(e => e.Durum).HasMaxLength(50);
            entity.Property(e => e.SiparisDurumu)
                .HasMaxLength(50)
                .HasDefaultValue("Onay Bekliyor");
            entity.Property(e => e.SiparisNotu).HasMaxLength(250);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TeslimSaati).HasMaxLength(50);
            entity.Property(e => e.TeslimTarihi).HasMaxLength(50);
            entity.Property(e => e.ToplamTutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UrunId).HasColumnName("UrunID");
            entity.Property(e => e.UyeId).HasColumnName("UyeID");

            entity.HasOne(d => d.Urun).WithMany(p => p.Siparislers)
                .HasForeignKey(d => d.UrunId)
                .HasConstraintName("FK_Siparisler_Urunler");

            entity.HasOne(d => d.Uye).WithMany(p => p.Siparislers)
                .HasForeignKey(d => d.UyeId)
                .HasConstraintName("FK__Siparisle__UyeID__403A8C7D");
        });

        modelBuilder.Entity<Urunler>(entity =>
        {
            entity.HasKey(e => e.UrunId).HasName("PK__Urunler__623D364BDBEE38E5");

            entity.ToTable("Urunler", tb =>
                {
                    tb.HasTrigger("trg_FiyatKontrol");
                    tb.HasTrigger("trg_StokBitti");
                    tb.HasTrigger("trg_UrunSilmeYedek");
                });

            entity.Property(e => e.UrunId).HasColumnName("UrunID");
            entity.Property(e => e.Aciklama).HasMaxLength(500);
            entity.Property(e => e.Fiyat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.KategoriId).HasColumnName("KategoriID");
            entity.Property(e => e.OrtalamaPuan)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 2)");
            entity.Property(e => e.ResimUrl).HasMaxLength(250);
            entity.Property(e => e.UrunAdi).HasMaxLength(100);
            entity.Property(e => e.YorumSayisi).HasDefaultValue(0);

            entity.HasOne(d => d.Kategori).WithMany(p => p.Urunlers)
                .HasForeignKey(d => d.KategoriId)
                .HasConstraintName("FK__Urunler__Kategor__398D8EEE");
        });

        modelBuilder.Entity<Uyeler>(entity =>
        {
            entity.HasKey(e => e.UyeId).HasName("PK__Uyeler__76F7D9EF0C9BC851");

            entity.ToTable("Uyeler");

            entity.HasIndex(e => e.Email, "UQ__Uyeler__A9D105343C94A371").IsUnique();

            entity.Property(e => e.UyeId).HasColumnName("UyeID");
            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.Adres).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Sifre).HasMaxLength(50);
            entity.Property(e => e.Soyad).HasMaxLength(50);
        });

        modelBuilder.Entity<VwSiparisDetaylari>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_SiparisDetaylari");

            entity.Property(e => e.Durum).HasMaxLength(50);
            entity.Property(e => e.SiparisId).HasColumnName("SiparisID");
            entity.Property(e => e.Tarih).HasColumnType("datetime");
            entity.Property(e => e.ToplamTutar).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UrunAdi).HasMaxLength(100);
        });

        modelBuilder.Entity<Yorumlar>(entity =>
        {
            entity.HasKey(e => e.YorumId).HasName("PK__Yorumlar__F2BE14C87E061F95");

            entity.ToTable("Yorumlar");

            entity.Property(e => e.YorumId).HasColumnName("YorumID");
            entity.Property(e => e.AdSoyad).HasMaxLength(50);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UrunId).HasColumnName("UrunID");
            entity.Property(e => e.Yorum).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
