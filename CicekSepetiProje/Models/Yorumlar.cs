using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace CicekSepetiProje.Models;

public partial class Yorumlar
{
    public int YorumId { get; set; }

    public int? UrunId { get; set; }

    public string? AdSoyad { get; set; }

    public string? Yorum { get; set; }

    public int? Puan { get; set; }

    public DateTime? Tarih { get; set; }
   
   
    // direkt "UrunId" sütununu kullanır.
    [ForeignKey("UrunId")]
    public virtual Urunler? Urun { get; set; }

    
}
