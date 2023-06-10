using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Brand : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Ad"), StringLength(50)]
        public string? Name { get; set; }
        [Display(Name = "Açıklama"), DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Logo { get; set; }
        [Display(Name = "Aktif?")]
        public bool IsActive { get; set; }
        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)] // ScaffoldColumn(false) crud sayfaları oluşturulurken bu kolonun ekranda oluşmamasını sağlar
        public DateTime CreateDate { get; set; }
        public IList<Product>? Products { get; set; }
    }
}
/*
 * ICollection ve IEnumerable arasında bazı temel farklar vardır

IEnumerable - Enumerator'ı almak için yalnızca GetEnumerator yöntemini içerir ve döngüye izin verir
ICollection ek yöntemler içerir: Add, Remove, Contains, Count, CopyTo
ICollection, IEnumerable'dan devralınır
ICollection ile add/remove gibi yöntemleri kullanarak koleksiyonu değiştirebilirsiniz. IEnumerable ile aynı şeyi yapma özgürlüğüne sahip değilsiniz.
******************
IEnumerable, bir koleksiyondaki değerleri okumanıza izin veren, ancak ona yazmayan bir GetEnumerator() yöntemine sahiptir. Numaralandırıcıyı kullanmanın karmaşıklığının çoğu, C#'taki for each ifadesi tarafından bizim için halledilir. IEnumerable'ın bir özelliği vardır: Geçerli öğeyi döndüren Current.

ICollection, IEnumerable'ı uygular ve en çok kullanımı Count olan birkaç ek özellik ekler. ICollection'ın genel sürümü Add() ve Remove() yöntemlerini uygular.

IList hem IEnumerable hem de ICollection'ı uygular ve öğelere tamsayı dizin oluşturma erişimini ekler (sıralama veritabanında yapıldığı için genellikle gerekli değildir).
 */