
namespace WebApiBIMU.Models
{
    public class Aula
    {
        [Key]
        public int Id { get; set; } = 0;
        public int Id_Materia { get; set; } = 0;
        public Materia Materia { get; set; } = new();
        public int Id_Professor { get; set; } = 0;
        public Pessoas Professor { get; set; } = new();
        public DateTime Data { get; set; } = DateTime.Now;
        public TimeSpan Hora { get; set; } = TimeSpan.Zero;
        public List<FreqAula> FreqAula { get; set; } = new();
    }
}
