using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.DiaSemanaService
{
    public class DiaSemanaService : GenericoService<DiaSemana>, IDiaSemanaService
    {
        public DiaSemanaService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
