namespace WebApiBIMU.DTOs.Usuarios
{
    public class UpdateUsuarioDto
    {
        public int Id { get; set; }
        public string? Password { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativado { get; set; } = false;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}
