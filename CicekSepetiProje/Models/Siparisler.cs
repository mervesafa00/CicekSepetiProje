using System;
using System.Collections.Generic;

namespace CicekSepetiProje.Models;

public partial class Siparisler
{
    public int SiparisId { get; set; }

    public int? UyeId { get; set; }

    public DateTime? Tarih { get; set; }

    public decimal? ToplamTutar { get; set; }

    public string? Durum { get; set; }

    public int? UrunId { get; set; }

    public string? TeslimTarihi { get; set; }

    public string? TeslimSaati { get; set; }

    public string? SiparisNotu { get; set; }

    public string? SiparisDurumu { get; set; }

    public virtual Urunler? Urun { get; set; }

    public virtual Uyeler? Uye { get; set; }
}
