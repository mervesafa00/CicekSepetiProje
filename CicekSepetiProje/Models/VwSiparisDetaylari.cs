using System;
using System.Collections.Generic;

namespace CicekSepetiProje.Models;

public partial class VwSiparisDetaylari
{
    public int SiparisId { get; set; }

    public string UrunAdi { get; set; } = null!;

    public decimal? ToplamTutar { get; set; }

    public DateTime? Tarih { get; set; }

    public string? Durum { get; set; }
}
