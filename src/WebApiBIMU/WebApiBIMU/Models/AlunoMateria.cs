namespace WebApiBIMU.Models
{
    public class AlunoMateria
    {
        public int? Id_Aluno { get; set; } = 0;
        public Pessoas Aluno { get; set; } = null;
        public int Id_Materia { get; set; } = 0;
        public Materia Materia { get; set; } = null;

    }
}
