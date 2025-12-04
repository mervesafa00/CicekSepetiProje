using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CicekSepetiProje.Models;

namespace CicekSepetiProje.Controllers
{
    public class SiparislerimController : Controller
    {
        private readonly CicekSepetiDbContext _context;

        public SiparislerimController(CicekSepetiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1 Numaralı Üyenin Siparişlerini Getir
            
            var siparisler = await _context.Siparislers
                .Include(s => s.Urun) // Resimler için gerekli
                .Where(s => s.UyeId == 1)
                .OrderByDescending(s => s.Tarih)
                .ToListAsync();

            return View(siparisler);
        }
    }
}