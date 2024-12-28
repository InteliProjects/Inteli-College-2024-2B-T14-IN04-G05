using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.UsuarioService
{
    public class UsuarioService : GenericoService<Usuario>, IUsuarioService
    {
        public UsuarioService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
