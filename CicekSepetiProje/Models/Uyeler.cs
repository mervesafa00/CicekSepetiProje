using System;
using System.Collections.Generic;

namespace CicekSepetiProje.Models;

public partial class Uyeler
{
    public int UyeId { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? Email { get; set; }

    public string? Sifre { get; set; }

    public string? Adres { get; set; }

    public virtual ICollection<Siparisler> Siparislers { get; set; } = new List<Siparisler>();
}
