using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.AulaService
{
    public class AulaService : GenericoService<Aula>, IAulaService
    {
        public AulaService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
