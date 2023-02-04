using Entities;
using System.Net;
using System.Net.Mail;

namespace NetCoreUrunSitesi.Utils
{
    public class MailHelper
    {
        public static async Task SendMailAsync(Contact contact)
        {
            SmtpClient smtpClient = new("mail.siteadresi.com", 587); // 1. parametre mail sunucu adresi, 2. parametre mail sunucu port numarası
            smtpClient.Credentials = new NetworkCredential("email kullanıcı adı buraya", "email şifresi buraya yazılacak");
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
    }
}
