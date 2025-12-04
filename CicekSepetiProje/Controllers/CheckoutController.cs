using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CicekSepetiProje.Models;

namespace CicekSepetiProje.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CicekSepetiDbContext _context;

        public CheckoutController(CicekSepetiDbContext context)
        {
            _context = context;
        }

        // 1. ADIM: TESLİMAT BİLGİLERİ EKRANI
        public IActionResult Index()
        {
            if (TempData["UrunId"] == null) return RedirectToAction("Index", "Urunlers");
            TempData.Keep();
            return View();
        }

        // 2. ARA ADIM: ADRESİ AL, ÖDEMEYE GİT
        [HttpPost]
        public IActionResult OdemeEkraninaGit(string AliciAd, string Telefon, string Sehir, string Ilce, string Mahalle, string AcikAdres)
        {
            // Adresi birleştirip hafızaya atalım
            string tamAdres = $"{Sehir} / {Ilce} / {Mahalle} / {AcikAdres}";

            TempData["AliciAd"] = AliciAd;
            TempData["Telefon"] = Telefon;
            TempData["TamAdres"] = tamAdres;

            // Diğer verileri de koru
            TempData.Keep("UrunId");
            TempData.Keep("Fiyat");
            TempData.Keep("Tarih");
            TempData.Keep("Saat");

            return RedirectToAction("Sepetim"); // Ödeme sayfasına git
        }

        // 3. ADIM: SEPETİM / ÖDEME EKRANI
        public IActionResult Sepetim()
        {
            // Kontrol et: Hafızada (TempData) ürün var mı?
            if (TempData.Peek("UrunId") == null)
            {
                // Sepet Boşsa, sayfaya "Bos" mesajı gönderelim
                ViewBag.SepetDurumu = "Bos";
            }
            else
            {
                // Sepet Doluysa verileri koru
                TempData.Keep();
                ViewBag.SepetDurumu = "Dolu";
            }

            return View();
        }

        // 4. FİNAL: SİPARİŞİ KAYDET
        [HttpPost]
        public async Task<IActionResult> Tamamla(string KartNo, string SonKullanma, string Cvv)
        {
            int urunId = Convert.ToInt32(TempData["UrunId"]);

            // FİYAT HATASINI ÇÖZEN KISIM:
            string fiyatStr = TempData["Fiyat"]?.ToString().Replace(".", ",");
            decimal fiyat = 0;
            decimal.TryParse(fiyatStr, out fiyat);

            string tarih = TempData["Tarih"]?.ToString();
            string saat = TempData["Saat"]?.ToString();

            // Not kısmına alıcı ve adresi yazalım
            string alici = TempData["AliciAd"]?.ToString();
            string tel = TempData["Telefon"]?.ToString();
            string adres = TempData["TamAdres"]?.ToString();
            string siparisNotu = $"Alıcı: {alici} ({tel}) - Adres: {adres}";

            // SQL'e Kaydet
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_SiparisVer {0}, {1}, {2}, {3}, {4}, {5}",
                urunId, 1, fiyat, tarih, saat, siparisNotu
            );

            return RedirectToAction("Success");
        }

        // BAŞARI SAYFASI
        public IActionResult Success()
        {
            return View();
        }
    }
}