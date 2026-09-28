namespace SportoloApi.Models
{
    public class Eredmeny
    {
        public int Id { get; set; }
        public string Competition { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ResultTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int SportoloId { get; set; }
    }
}
