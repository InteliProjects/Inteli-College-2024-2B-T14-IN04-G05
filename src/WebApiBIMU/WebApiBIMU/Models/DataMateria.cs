
namespace WebApiBIMU.Models
{
    public class DataMateria
    {
        [Key]
        public int Id { get; set; } = 0;
        
        public int Id_Materia { get; set; } = 0;
        public Materia Materia { get; set; } = new();
        public int Id_DiaSemana { get; set; }
        public DiaSemana DiaSemana { get; set; } = new();
        public TimeSpan Horario { get; set; } = TimeSpan.Zero;
    }
}
