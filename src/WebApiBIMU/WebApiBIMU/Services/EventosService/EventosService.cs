using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.EventosService
{
    public class EventosService : GenericoService<Eventos>, IEventosService
    {
        public EventosService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor) : base(mapper, context, httpContextAccessor) { }
    }
}
