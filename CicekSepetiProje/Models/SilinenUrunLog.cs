using System;
using System.Collections.Generic;

namespace CicekSepetiProje.Models;

public partial class SilinenUrunLog
{
    public int LogId { get; set; }

    public string? UrunAdi { get; set; }

    public DateTime? SilinmeTarihi { get; set; }
}
