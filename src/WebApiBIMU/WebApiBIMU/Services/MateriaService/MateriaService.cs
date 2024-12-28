using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.MateriaService
{
    public class MateriaService : GenericoService<Materia>, IMateriaService
    {
        public MateriaService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
