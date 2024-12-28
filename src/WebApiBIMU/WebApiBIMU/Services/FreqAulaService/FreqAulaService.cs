using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.FreqAulaService
{
    public class FreqAulaService : GenericoService<FreqAula>, IFreqAulaService
    {
        public FreqAulaService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
