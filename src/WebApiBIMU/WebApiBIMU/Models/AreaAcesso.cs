using AutoMapper;

namespace WebApiBIMU.Models
{
    public class AreaAcesso
    {
        [Key]
        public int Id { get; set; } = 0;
        public string Area { get; set; } = string.Empty;


        public List<HistoricoAcesso> HistoricoAcesso { get; set; } = null;
    }
}