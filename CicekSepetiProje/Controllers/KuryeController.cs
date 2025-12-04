using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CicekSepetiProje.Models;

namespace CicekSepetiProje.Controllers
{
    public class KuryeController : Controller
    {
        private readonly CicekSepetiDbContext _context;

        public KuryeController(CicekSepetiDbContext context)
        {
            _context = context;
        }

        // Sadece "Yola Çıkan" (Dağıtımdaki) paketleri göster
        public async Task<IActionResult> Index()
        {
            var dagitimdakiler = await _context.Siparislers
                .Include(s => s.Urun)
                .Include(s => s.Uye)
                .Where(s => s.Durum == "Yola Çıktı")
                .ToListAsync();

            return View(dagitimdakiler);
        }

        // "Teslim Et" Butonu
        [HttpPost]
        public async Task<IActionResult> TeslimEt(int id)
        {
            var siparis = await _context.Siparislers.FindAsync(id);
            if (siparis != null)
            {
                siparis.Durum = "Teslim Edildi";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}