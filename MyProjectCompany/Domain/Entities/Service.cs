
using MyProjectCompany.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyProjectCompany.Domain.Entities
{
    public class Service:EntityBase
    {
        [Display(Name = "Виберіть категорію, до якої належить послуга")]
        public int? ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }

        [Display(Name ="Короткий опис")]
        [MaxLength(2000)]
        public string? DescriptionShort{ get; set; }

        [Display(Name = "Опис")]
        [MaxLength(10000)]
        public string? Description{ get; set; }

        [Display(Name = "Титульна картинка")]
        [MaxLength(260)]
        public string? Photo {  get; set; }

        [Display(Name = "Тип послуги")]
        public ServiceTypeEnum Type { get; set; }

    }
}
