using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.AlunoMateriaService
{
    public class AlunoMateriaService : GenericoService<AlunoMateria>, IAlunoMateriaService
    {
        public AlunoMateriaService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}