using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.TipoPessoaService
{
    public class TipoPessoaService : GenericoService<TipoPessoa>, ITipoPessoaService
    {
        public TipoPessoaService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
