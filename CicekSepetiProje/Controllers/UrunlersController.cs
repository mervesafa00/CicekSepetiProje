using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CicekSepetiProje.Models;

namespace CicekSepetiProje.Controllers
{
    public class UrunlersController : Controller
    {
        private readonly CicekSepetiDbContext _context;

        public UrunlersController(CicekSepetiDbContext context)
        {
            _context = context;
        }

        
        // GET: Urunlers
        public async Task<IActionResult> Index(string aramaKelimesi)
        {
            // 1. Durum: Arama kutusu boşsa hepsini getir
            if (string.IsNullOrEmpty(aramaKelimesi))
            {
                var cicekSepetiDbContext = _context.Urunlers.Include(u => u.Kategori);
                return View(await cicekSepetiDbContext.ToListAsync());
            }
            // 2. Durum: Arama yapıldıysa SP çalıştır
            else
            {
                var urunler = await _context.Urunlers
                    .FromSqlRaw("EXEC sp_UrunAra {0}", aramaKelimesi)
                    .ToListAsync();

                return View(urunler);
            }
        }

        

        // GET: Urunlers/Create
        public IActionResult Create()
        {
            ViewData["KategoriId"] = new SelectList(_context.Kategorilers, "KategoriId", "KategoriId");
            return View();
        }

        // POST: Urunlers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UrunId,UrunAdi,Aciklama,Fiyat,Stok,ResimUrl,KategoriId")] Urunler urunler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(urunler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategorilers, "KategoriId", "KategoriId", urunler.KategoriId);
            return View(urunler);
        }

        // GET: Urunlers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urunler = await _context.Urunlers.FindAsync(id);
            if (urunler == null)
            {
                return NotFound();
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategorilers, "KategoriId", "KategoriId", urunler.KategoriId);
            return View(urunler);
        }

        // POST: Urunlers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UrunId,UrunAdi,Aciklama,Fiyat,Stok,ResimUrl,KategoriId")] Urunler urunler)
        {
            if (id != urunler.UrunId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(urunler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunlerExists(urunler.UrunId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategorilers, "KategoriId", "KategoriId", urunler.KategoriId);
            return View(urunler);
        }

        // GET: Urunlers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urunler = await _context.Urunlers
                .Include(u => u.Kategori)
                .FirstOrDefaultAsync(m => m.UrunId == id);
            if (urunler == null)
            {
                return NotFound();
            }

            return View(urunler);
        }

        // POST: Urunlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urunler = await _context.Urunlers.FindAsync(id);
            if (urunler != null)
            {
                _context.Urunlers.Remove(urunler);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UrunlerExists(int id)
        {
            return _context.Urunlers.Any(e => e.UrunId == id);
        }
        // POST: Urunlers/SiparisVer
        [HttpPost]
        public async Task<IActionResult> SiparisVer(int UrunId, string teslimTarihi, string teslimSaati, string siparisNotu)
        {
            // 1. Ürünü ve Fiyatını Bul
            var urun = await _context.Urunlers.FindAsync(UrunId);
            if (urun == null) return NotFound();

            // 2. SQL'deki GÜNCELLENMİŞ SP'yi çağır
            // Parametreler: UrunID, UyeID(1), Fiyat, Tarih, Saat, Not
            // Not: UyeID'yi şimdilik 1 (Merve) olarak sabit gönderiyoruz.
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_SiparisVer {0}, {1}, {2}, {3}, {4}, {5}",
                UrunId, 1, urun.Fiyat, teslimTarihi, teslimSaati, siparisNotu
            );

            // 3. İşlem bitince Teşekkür sayfasına veya Ana sayfaya yönlendir
            // Şimdilik ana sayfaya atalım
            return RedirectToAction(nameof(Index));
        }
        // 1. DETAY SAYFASI (Yorumları ve Kategoriyi getirir)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var urun = await _context.Urunlers
                .Include(u => u.Kategori)
                .Include(u => u.Yorumlars) // Yorumları dahil et
                .FirstOrDefaultAsync(m => m.UrunId == id);

            if (urun == null) return NotFound();

            return View(urun);
        }

        // 2. YORUM YAPMA İŞLEMİ (Formdan gelen veriyi kaydeder)
        [HttpPost]
        public async Task<IActionResult> YorumYap(int UrunId, string AdSoyad, string YorumMetni, int Puan)
        {
            // Yorumu veritabanına ekle
            var yorum = new Yorumlar
            {
                UrunId = UrunId,
                AdSoyad = AdSoyad,
                Yorum = YorumMetni,
                Puan = Puan,
                Tarih = DateTime.Now
            };

            _context.Yorumlars.Add(yorum);

            // Ürünün ortalama puanını güncelle (Opsiyonel ama şık olur)
            var urun = await _context.Urunlers.Include(u => u.Yorumlars).FirstOrDefaultAsync(u => u.UrunId == UrunId);
            if (urun != null)
            {
                // Yeni yorumu listeye sanal olarak ekleyin  ki ortalamayı doğru hesaplayalım
                urun.Yorumlars.Add(yorum);
                urun.YorumSayisi = urun.Yorumlars.Count;
                // Ortalamayı hesapla
                double ortalama = urun.Yorumlars.Average(y => y.Puan ?? 0);
                urun.OrtalamaPuan = (decimal)ortalama;
            }

            await _context.SaveChangesAsync();

            // Sayfayı yenile (Detay sayfasına geri dön)
            return RedirectToAction("Details", new { id = UrunId });
        }
    }
}
