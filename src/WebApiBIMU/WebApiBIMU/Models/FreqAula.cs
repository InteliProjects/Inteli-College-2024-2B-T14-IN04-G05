namespace WebApiBIMU.Models
{
    public class FreqAula
    {
        [Key]
        public int Id { get; set; } = 0;
        public int Id_Aula { get; set;} = 0;
        public Aula Aula { get; set;} = new();
        public int Id_Aluno { get; set; } = 0;
        public Pessoas Aluno { get; set;} = new();
        public bool EstaPresente { get; set; } = false;
    }
}
