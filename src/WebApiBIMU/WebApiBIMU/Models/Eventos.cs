namespace WebApiBIMU.Models
{
    public class Eventos
    {
        [Key]
        public int Id { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.Now;
        public TimeSpan Hora { get; set; } = TimeSpan.Zero;
        public string Descricao { get; set; } = string.Empty;
    }
}
