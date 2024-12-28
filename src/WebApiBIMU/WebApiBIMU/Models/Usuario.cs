namespace WebApiBIMU.Models
{
    public class Usuario
    {
        [Key]
        public int? Id { get; set; } = null;
        public int? Id_Pessoa { get; set; } = 0;
        public Pessoas? Pessoa { get; set; } = new();
        public string Nome { get; set; } = string.Empty;
        public byte[]? PskHash { get; set; }
        public byte[]? PskSalt { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public bool Ativado { get; set; } = false;
    }
}
