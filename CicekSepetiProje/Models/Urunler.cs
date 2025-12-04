using System;
using System.Collections.Generic;

namespace CicekSepetiProje.Models;

public partial class Urunler
{
    public int UrunId { get; set; }

    public string UrunAdi { get; set; } = null!;

    public string? Aciklama { get; set; }

    public decimal Fiyat { get; set; }

    public int Stok { get; set; }

    public string? ResimUrl { get; set; }

    public int? KategoriId { get; set; }

    public decimal? OrtalamaPuan { get; set; }

    public int? YorumSayisi { get; set; }

    public virtual Kategoriler? Kategori { get; set; }

    public virtual ICollection<Siparisler> Siparislers { get; set; } = new List<Siparisler>();
    // Ürünün yorumlarını tutacak liste
    public virtual ICollection<Yorumlar> Yorumlars { get; set; } = new List<Yorumlar>();
}
