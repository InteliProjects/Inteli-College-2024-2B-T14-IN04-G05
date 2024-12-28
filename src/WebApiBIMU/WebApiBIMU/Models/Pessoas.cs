namespace WebApiBIMU.Models
{
    public class Pessoas
    {
        [Key]
        public int Id { get; set; } = 0;
        public int? Id_Tipo_Pessoa { get; set; } = 0;
        public TipoPessoa TipoPessoa { get; set; } = null;
        public string Nome { get; set; } = string.Empty;
        public int? Data_Nascimento { get; set; }
        public int? RG { get; set; } = 0;
        public int? CPF { get; set; } = 0;
        public string Genero { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public int? CEP { get; set; } = 0;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? Celular { get; set; } = 0;


        public List<Materia> Materias { get; set; } = null;
        public List<AlunoMateria> AlunoMaterias { get; set; } = null;
        public List<Usuario> Usuario { get; set; } = null;
        public List<HistoricoAcesso> HistoricoAcesso { get; set; } = null;
        public List<FreqAula> FreqAula { get; set; } = null;
        
        public List<Pessoas> Responsavel { get; set; } = null;
        public List<Pessoas> Alunos { get; set; } = null;
        public List<ResponsavelAluno> ResponsavelDoAluno { get; set; } = null;
        public List<ResponsavelAluno> AlunoDoResponsavel { get; set; } = null;
        public List<Aula> Aula { get; set; } = null;
    }
}
