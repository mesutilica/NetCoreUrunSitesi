﻿using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class AppUser : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        //[Required(ErrorMessage = "{0} Boş Geçilemez!")]
        public string Email { get; set; }
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        public string? Username { get; set; }
        [Display(Name = "Şifre"), Required(ErrorMessage = "{0} Boş Geçilemez!")]
        public string Password { get; set; }
        [Display(Name = "Aktif?")]
        public bool IsActive { get; set; }
        [Display(Name = "Admin?")]
        public bool IsAdmin { get; set; }
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; }
        public Guid? UserGuid { get; set; } = Guid.NewGuid();
        // Jwt için propertyler
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
        public List<Favorite>? Favorites { get; set; }
        public List<Address>? Addresses { get; set; }
        [Display(Name = "Ad Soyad"), ScaffoldColumn(false)]
        public string? NameSurname
        {
            get
            {
                return this.Name + " " + this.Surname;
            }
        }
    }
}
