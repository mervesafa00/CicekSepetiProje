using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CicekSepetiProje.Models;

namespace CicekSepetiProje.Controllers
{
    public class RaporlarController : Controller
    {
        private readonly CicekSepetiDbContext _context;

        public RaporlarController(CicekSepetiDbContext context)
        {
            _context = context;
        }

        // 1. RAPORLAR ANA SAYFASI (Menü)
        public IActionResult Index()
        {
            return View();
        }

        // 2. SQL VIEW RAPORU (Siparişleri Detaylı Gör)
        public async Task<IActionResult> SiparisRaporu()
        {
            // Veritabanındaki 'vw_SiparisDetaylari' View'ından veri çeker
            
            var rapor = await _context.VwSiparisDetaylaris.ToListAsync();
            return View(rapor);
        }

        // 3. SQL TRIGGER RAPORU (Silinen Ürünleri Gör)
        public async Task<IActionResult> SilinenUrunler()
        {
            // Trigger'ın doldurduğu 'SilinenUrunLog' tablosunu çeker
            var loglar = await _context.SilinenUrunLogs
                .OrderByDescending(x => x.SilinmeTarihi)
                .ToListAsync();

            return View(loglar);
        }
    }
}