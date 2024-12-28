using System.Collections.Generic;

namespace WebApiBIMU.Models
{
    public class TipoPessoa
    {
        [Key]
        public int Id { get; set; } = 0;
        public string Tipo_Pessoa_Desc { get; set; } = string.Empty;

        public List<Pessoas> Pessoas { get; set; } = new();         
    }
}
