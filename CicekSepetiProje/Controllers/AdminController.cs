using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CicekSepetiProje.Models;

namespace CicekSepetiProje.Controllers
{
    public class AdminController : Controller
    {
        private readonly CicekSepetiDbContext _context;

        public AdminController(CicekSepetiDbContext context)
        {
            _context = context;
        }

        // --- 1. SİPARİŞ YÖNETİMİ ---
        public async Task<IActionResult> Index()
        {
            var siparisler = await _context.Siparislers 
                .Include(s => s.Urun) 
                .Include(s => s.Uye)  
                .OrderByDescending(s => s.Tarih)
                .ToListAsync();
            return View(siparisler);
        }

        [HttpPost]
        public async Task<IActionResult> DurumDegistir(int siparisId, string yeniDurum)
        {
            var siparis = await _context.Siparislers.FindAsync(siparisId);
            if (siparis != null)
            {
                siparis.Durum = yeniDurum;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // --- 2. ÜRÜN YÖNETİMİ ---

        // A. Ürünleri Listele
        public async Task<IActionResult> UrunYonetimi()
        {
            var urunler = await _context.Urunlers.ToListAsync();
            return View(urunler);
        }

        // B. Yeni Ürün Ekleme Sayfasını Aç
        public IActionResult UrunEkle()
        {
            return View();
        }

        // C. Yeni Ürünü Kaydet (HATA GİZLEMEYEN VERSİSYON)
        [HttpPost]
        public async Task<IActionResult> UrunEkle(string UrunAdi, string Aciklama, string Fiyat, int Stok, string ResimUrl)
        {
            // 1. Yeni bir ürün kutusu oluştur
            Urunler urun = new Urunler();

            // 2. Bilgileri doldur
            urun.UrunAdi = UrunAdi;
            urun.Aciklama = Aciklama;
            urun.Stok = Stok;

            // 3. FİYAT DÜZELTME
            // Gelen "100.50" veya "100,50" fark etmez, düzeltir.
            if (!string.IsNullOrEmpty(Fiyat))
            {
                string fiyatDuzgun = Fiyat.Replace(".", ","); // Noktayı virgüle çevir (Türkçe sistem için)
                decimal f;
                if (decimal.TryParse(fiyatDuzgun, out f))
                {
                    urun.Fiyat = f;
                }
                else
                {
                    urun.Fiyat = 0; // Çeviremezse 0 yap
                }
            }

            // 4. Resim Yolu
            if (string.IsNullOrEmpty(ResimUrl))
            {
                urun.ResimUrl = "/img/gul.jpg";
            }
            else
            {
                urun.ResimUrl = ResimUrl;
            }

            // 5. Diğer Zorunlu Alanlar
            urun.OrtalamaPuan = 5;
            urun.YorumSayisi = 0;
            urun.KategoriId = null; // Kategori boş olabilir

            // 6. KAYDET
            _context.Urunlers.Add(urun);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(UrunYonetimi));
        }
        public async Task<IActionResult> UrunSil(int id)
        {
            try
            {
                var urun = await _context.Urunlers.FindAsync(id);
                if (urun != null)
                {
                    // 1. Önce Yorumları Sil
                    var yorumlar = _context.Yorumlars.Where(y => y.UrunId == id);
                    _context.Yorumlars.RemoveRange(yorumlar);

                    // 2. Sonra Siparişleri Sil
                    var siparisler = _context.Siparislers.Where(s => s.UrunId == id);
                    _context.Siparislers.RemoveRange(siparisler);

                    // 3. En Son Ürünü Sil
                    _context.Urunlers.Remove(urun);

                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                // Silinmezse bile hata verme, sayfada kal
            }

            return RedirectToAction("UrunYonetimi");
        }
    }
}