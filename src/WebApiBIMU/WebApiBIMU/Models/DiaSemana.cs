namespace WebApiBIMU.Models
{
    public class DiaSemana
    {
        [Key]
        public int Id { get; set; } = 0;
        public bool Domingo { get; set; } = false; 
        public bool Segunda { get; set; } = false; 
        public bool Terca { get; set; } = false;
        public bool Quarta { get; set; } = false;
        public bool Quinta { get; set; } = false;
        public bool Sexta { get; set; } = false;
        public bool Sabado { get; set; } = false;
        public List<DataMateria> DataMateria { get; set; } = new();
    }
}
