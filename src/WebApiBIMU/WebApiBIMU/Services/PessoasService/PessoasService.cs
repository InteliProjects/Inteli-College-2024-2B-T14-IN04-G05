using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.PessoasService
{
    public class PessoasService : GenericoService<Pessoas>, IPessoasService
    {
        public PessoasService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
