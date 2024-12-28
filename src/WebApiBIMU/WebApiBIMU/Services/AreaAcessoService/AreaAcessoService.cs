using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.AreaAcessoService
{
    public class AreaAcessoService : GenericoService<AreaAcesso>, IAreaAcessoService
    {
        public AreaAcessoService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
