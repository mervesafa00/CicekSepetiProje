using Microsoft.AspNetCore.Mvc;

namespace CicekSepetiProje.Controllers
{
    public class AccountController : Controller
    {
        // GİRİŞ SAYFASI
        [HttpGet]
        public IActionResult Login(int UrunId, string Fiyat, string adres, string tarih, string saat)
        {
            // Fiyatı 'string' (yazı) olarak aldık ki hata vermesin
            TempData["UrunId"] = UrunId;
            TempData["Fiyat"] = Fiyat;
            TempData["Adres"] = adres;
            TempData["Tarih"] = tarih;
            TempData["Saat"] = saat;

            return View();
        }

        // MİSAFİR GİRİŞİ BUTONU
        public IActionResult GuestCheckout()
        {
            // Verileri koru
            TempData.Keep("UrunId");
            TempData.Keep("Fiyat");
            TempData.Keep("Adres");
            TempData.Keep("Tarih");
            TempData.Keep("Saat");

            // Ödeme sayfasına git
            return RedirectToAction("Index", "Checkout");
        }
    }
}