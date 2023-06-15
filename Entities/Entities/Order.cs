using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; }
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
        public EnumOrderState OrderState { get; set; }

        public string Username { get; set; }
        public string AdresTanimi { get; set; }
        public string Adres { get; set; }
        public string Sehir { get; set; }
        public string Semt { get; set; }
        public string Telefon { get; set; }
        public virtual List<OrderLine>? OrderLines { get; set; }
    }
    public enum EnumOrderState
    {
        [Display(Name = "Onay Bekleniyor")]
        Waiting,

        [Display(Name = "Tamamlandı")]
        Completed
    }
}
