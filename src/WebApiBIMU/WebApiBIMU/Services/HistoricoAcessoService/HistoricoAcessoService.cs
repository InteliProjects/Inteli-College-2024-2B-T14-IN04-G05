using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.HistoricoAcessoService
{
    public class HistoricoAcessoService : GenericoService<HistoricoAcesso>, IHistoricoAcessoService
    {
        public HistoricoAcessoService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
