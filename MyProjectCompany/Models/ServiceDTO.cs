namespace MyProjectCompany.Models
{
    //класс DTO предназначен для передачи данных на клиент
    //скрывает за собой реальную архитектуру доменной модели Service
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? Title { get; set; }
        public string? DescriptionShort { get; set; }
        public string? Description { get; set; }
        public string? PhotoFileName { get; set; }
        public string? Type { get; set; }
    }
}
