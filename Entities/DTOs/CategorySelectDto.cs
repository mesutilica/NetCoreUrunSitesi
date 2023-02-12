using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class CategorySelectDto
    {
        public int Id { get; set; }
        [Display(Name = "Ad"), StringLength(50)]
        public string? Name { get; set; }
    }
}
