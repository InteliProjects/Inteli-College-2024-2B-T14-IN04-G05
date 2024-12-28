namespace WebApiBIMU.Models
{
    public class Materia
    {
        [Key]
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<DataMateria> DataMateria { get; set; } = new();
        public List<Pessoas> Alunos { get; set; } = new();
        public List<AlunoMateria> AlunoMaterias { get; set; } = new();
        public List<Aula> Aula { get; set; } = new();
    }
}
