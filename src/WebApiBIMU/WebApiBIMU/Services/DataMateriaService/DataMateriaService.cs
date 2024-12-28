using WebApiBIMU.Services.GenericosService;

namespace WebApiBIMU.Services.DataMateriaService
{
    public class DataMateriaService : GenericoService<DataMateria>, IDataMateriaService
    {
        public DataMateriaService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
            : base(mapper, context, httpContextAccessor)
        {
        }
    }
}
