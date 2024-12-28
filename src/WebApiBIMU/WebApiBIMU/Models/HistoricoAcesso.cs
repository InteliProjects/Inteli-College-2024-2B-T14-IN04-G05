using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBIMU.Models
{
    public class HistoricoAcesso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;
        public int Id_Pessoa { get; set; } = 0;
        public Pessoas Pessoa { get; set; } = null;
        public int Id_Area { get; set; } = 0;
        public AreaAcesso Area { get; set; } = null;
        public DateTime Data { get; set; } = DateTime.Now;
        public TimeSpan Horario { get; set; } = TimeSpan.Zero;
        public bool Entrada_Saida { get; set; } = false;
    }
}
