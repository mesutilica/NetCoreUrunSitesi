using Core.Entities;
using System.Net;
using System.Net.Mail;

namespace NetCoreUrunSitesi.Utils
{
    public class MailHelper
    {
        public static async Task SendMailAsync(Contact contact)
        {
            SmtpClient smtpClient = new("mail.siteadresi.com", 587); // 1. parametre mail sunucu adresi, 2. parametre mail sunucu port numarası
            smtpClient.Credentials = new NetworkCredential("email şifresi buraya yazılacak", "email şifresi buraya yazılacak");
            smtpClient.EnableSsl = true; // Eğer sunucu ssl kullanıyorsa true kullanmıyorsa false olmalı
            MailMessage message = new(); // Yeni bir email nesnesi oluşturduk
            message.From = new MailAddress("info@siteadi.com"); // Mailin gönderileceği adres
            message.To.Add("mailingidecegiadres@siteadi.com"); // Mail alıcı adresi
            message.Subject = "Siteden Mesaj Geldi"; // Mailin konusu
            message.Body = $"<p>Mail Bilgileri : </p> İsim : {contact.Name} <hr /> Soyisim : {contact.Surname} <hr /> Email : {contact.Email} <hr /> Telefon : {contact.Phone} <hr /> Mesaj : {contact.Message} <hr /> Gönderilme Tarihi : {contact.CreateDate}";
            message.IsBodyHtml = true; // Mail içeriğinde html elementleri kullanabilmek için 
            //client.Send(message); // Oluşturduğumuz maili gönderdiyoruz
            await smtpClient.SendMailAsync(message);
            smtpClient.Dispose();
        }
        public static async Task SendMailAsync(Contact contact, IConfiguration configuration)
        {
            SmtpClient smtpClient = new(configuration["MailSunucu"], 587); // 1. parametre mail sunucu adresi, 2. parametre mail sunucu port numarası
            smtpClient.Credentials = new NetworkCredential(configuration["MailKullanici:Username"], configuration["MailKullanici:Password"]);
            smtpClient.EnableSsl = true; // Eğer sunucu ssl kullanıyorsa true kullanmıyorsa false olmalı
            MailMessage message = new(); // Yeni bir email nesnesi oluşturduk
            message.From = new MailAddress(configuration["Email"]); // Mailin gönderileceği adres
            message.To.Add(configuration["AliciEmail"]); // Mail alıcı adresi
            message.Subject = "Siteden Mesaj Geldi"; // Mailin konusu
            message.Body = $"<p>Mail Bilgileri : </p> İsim : {contact.Name} <hr /> Soyisim : {contact.Surname} <hr /> Email : {contact.Email} <hr /> Telefon : {contact.Phone} <hr /> Mesaj : {contact.Message} <hr /> Gönderilme Tarihi : {contact.CreateDate}";
            message.IsBodyHtml = true; // Mail içeriğinde html elementleri kullanabilmek için 
            //client.Send(message); // Oluşturduğumuz maili gönderdiyoruz
            await smtpClient.SendMailAsync(message);
            smtpClient.Dispose();
        }
    }
}
