namespace WebApiBIMU.DTOs
{
    public class AddEventoDto
    {
        public DateTime Data { get; set; } = DateTime.Now;
        public TimeSpan Hora { get; set; } = TimeSpan.Zero;
        public string Descricao { get; set; } = string.Empty;
    }
}
