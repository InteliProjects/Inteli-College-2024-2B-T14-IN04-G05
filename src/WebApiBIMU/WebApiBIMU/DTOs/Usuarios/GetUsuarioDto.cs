namespace WebApiBIMU.DTOs.Usuarios
{
    public class GetUsuarioDto
    {
        public int Id { get; set; }
        public int? Id_Pessoa { get; set; } = 0;
        public string Nome { get; set; } = string.Empty;
    }
}
