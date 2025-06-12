using System.ComponentModel.DataAnnotations;

namespace MyProjectCompany.Domain.Entities
{
    public abstract class EntityBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage="введiть назву")]
        [MaxLength(100)]
        [Display(Name ="Назва")]
        public string? Title { get; set; }
        public DateTime DateCreated {  get; set; }= DateTime.UtcNow;
    }
}
