
          WEB TABANLI ÇİÇEK SEPETİ VERİTABANI PROJESİ


         

PROJENİN KURULUM VE ÇALIŞTIRMA ADIMLARI

!!! Bu projenin sorunsuz çalışabilmesi için öncelikle veritabanının kurulması gerekmektedir. !!!



ADIM 1: VERİTABANI KURULUMU (SQL SERVER)

   1. Proje klasörü içerisinde bulunan "CicekSepeti_Veritabani.sql" dosyasını bulunuz.


   2. Microsoft SQL Server Management Studio'yu (SSMS) açınız.


   3. Bu dosyayı sürükleyip SSMS içine bırakınız veya File > Open menüsünden açınız.


   4. Yukarıdaki "Execute" (F5) butonuna basarak scripti çalıştırınız.


   5. Bu işlem sonucunda "CicekSepetiDB" veritabanı, tüm tablolar, veriler, Stored Procedure'ler ve Trigger'lar otomatik olarak oluşacaktır.


ADIM 2: PROJENİN BAŞLATILMASI (VISUAL STUDIO)

   1. "CicekSepetiProje" klasöründeki "CicekSepetiProje.sln" dosyasına çift tıklayarak projeyi Visual Studio 2022'de açınız.


   2. (Opsiyonel) Eğer veritabanı bağlantı hatası alırsanız; "appsettings.json" dosyasını açarak "Server=..." kısmını kendi bilgisayarınızın sunucu adıyla değiştiriniz.


   3. Yukarıdaki yeşil "IIS Express" (Play) butonuna basarak projeyi tarayıcıda başlatınız.




2. UYGULAMA MODÜLLERİ VE KULLANIM SENARYOLARI


!!! Proje, gerçek hayat senaryosuna uygun olarak 3 farklı kullanıcı arayüzü içermektedir !!!

KULLANICI ARAYÜZÜ

   - Erişim: Tarayıcı adres çubuğuna "/Urunlers" yazılarak erişilir.


   - Amaç: Kullanıcıların ürünleri incelemesi ve sipariş vermesi.


 
     * Gelişmiş ürün listeleme (Yuvarlak kategori yapısı).

     * Detaylı ürün sayfası (Yorumlar, Puanlama, Dinamik Fiyat Hesaplama).

     * Sipariş Süreci: Adres Girişi -> Tarih/Saat Seçimi -> Giriş/Misafir Seçimi -> Ödeme -> Onay.

     * "Siparişlerim" sayfasından kurye durumunun (Hazırlanıyor > Yola Çıktı > Teslim Edildi) anlık takibi.


FİRMA (ADMIN) PANELİ

   - Erişim: Tarayıcı adres çubuğuna "/Admin" yazılarak erişilir.
 
     * Gelen siparişlerin görüntülenmesi ve durumunun (Hazırlanıyor, Yola Çıktı, Teslim edildi) güncellenmesi.

     * "Ürün Yönetimi" menüsünden yeni çiçek ekleme, silme ve fiyat güncelleme.

     * "Raporlar" menüsünden sistem istatistiklerinin izlenmesi.


KURYE PANELİ

   - Erişim: Tarayıcı adres çubuğuna "/Kurye" yazılarak erişilir.
 
     * Sadece durumu "Yola Çıktı" olan paketlerin listelenmesi.

     * Teslimat adresi ve müşteri notunun görüntülenmesi.

     * "Teslim Et" butonu ile sipariş sürecinin sonlandırılması.


TEKNİK ALTYAPI

- Programlama Dili: C#

- Framework: ASP.NET Core 8.0 MVC

- Veritabanı: Microsoft SQL Server

- ORM: Entity Framework Core

- Frontend: HTML5, CSS3, Bootstrap 5, JavaScript

