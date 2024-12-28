namespace WebApiBIMU.Models
{
    public class ResponsavelAluno
    {
        public int Id_Aluno { get; set; } = 0;
        public Pessoas Aluno { get; set; } = new();
        public int Id_Responsavel { get; set; } = 0;
        public Pessoas Responsavel { get; set; } = new();


    }
}
